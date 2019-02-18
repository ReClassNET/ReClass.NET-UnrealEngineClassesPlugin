using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using UnrealEngineClassesPlugin.Nodes;

namespace UnrealEngineClassesPlugin
{
	public class NodeConverter : ICustomNodeSerializer
	{
		/// <summary>Name of the type used in the XML data.</summary>
		private const string XmlTypePrefix = "UnrealEngineClasses.";

		public bool CanHandleNode(BaseNode node) => node is FDateTimeNode || node is FGuidNode || node is FQWordNode || node is FStringNode || node is TArrayNode || node is TSharedPtrNode;

		public bool CanHandleElement(XElement element) => element.Attribute(ReClassNetFile.XmlTypeAttribute)?.Value.StartsWith(XmlTypePrefix) == true;

		public bool TryCreateNodeFromElement(XElement element, BaseNode parent, IEnumerable<ClassNode> classes, ILogger logger, CreateNodeFromElementHandler defaultHandler, out BaseNode node)
		{
			node = null;

			var type = element.Attribute(ReClassNetFile.XmlTypeAttribute)?.Value;
			switch (type)
			{
				case XmlTypePrefix + "FDateTime":
					node = new FDateTimeNode();
					break;
				case XmlTypePrefix + "FGuid":
					node = new FGuidNode();
					break;
				case XmlTypePrefix + "FQWord":
					node = new FQWordNode();
					break;
				case XmlTypePrefix + "FString":
					node = new FStringNode();
					break;
				case XmlTypePrefix + "TArray":
				case XmlTypePrefix + "TSharedPtr":
				{
					if (type == XmlTypePrefix + "TArray")
					{
						node = new TArrayNode();
					}
					else
					{
						node = new TSharedPtrNode();
					}

					BaseNode innerNode = null;
					var innerElement = element.Elements().FirstOrDefault();
					if (innerElement != null)
					{
						innerNode = defaultHandler(innerElement, node, logger);
					}

					var wrapperNode = (BaseWrapperNode)node;
					if (wrapperNode.CanChangeInnerNodeTo(innerNode))
					{
						var rootWrapperNode = node.GetRootWrapperNode();
						if (rootWrapperNode.ShouldPerformCycleCheckForInnerNode()
							&& innerNode is ClassNode classNode
							&& ClassUtil.IsCyclicIfClassIsAccessibleFromParent(node.GetParentClass(), classNode, classes))
						{
							logger.Log(LogLevel.Error, $"Skipping node with cyclic class reference: {node.GetParentClass().Name}->{rootWrapperNode.Name}");

							return false;
						}

						wrapperNode.ChangeInnerNode(innerNode);
					}
					else
					{
						return false;
					}

					break;
				}
				default:
					throw new InvalidOperationException("Can not handle node type: " + type);
			}

			node.Name = element.Attribute(ReClassNetFile.XmlNameAttribute)?.Value ?? string.Empty;
			node.Comment = element.Attribute(ReClassNetFile.XmlCommentAttribute)?.Value ?? string.Empty;

			return true;
		}

		public XElement CreateElementFromNode(BaseNode node, ILogger logger, CreateElementFromNodeHandler defaultHandler)
		{
			var element = new XElement(
				ReClassNetFile.XmlNodeElement,
				new XAttribute(ReClassNetFile.XmlNameAttribute, node.Name ?? string.Empty),
				new XAttribute(ReClassNetFile.XmlCommentAttribute, node.Comment ?? string.Empty)
			);

			switch (node)
			{
				case FDateTimeNode _:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "FDateTime");
					break;
				case FGuidNode _:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "FGuid");
					break;
				case FQWordNode _:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "FQWord");
					break;
				case FStringNode _:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "FString");
					break;
				case TArrayNode _:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "TArray");
					break;
				case TSharedPtrNode _:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "TSharedPtr");
					break;
			}

			if (node is BaseWrapperNode wrapperNode)
			{
				element.Add(defaultHandler(wrapperNode.InnerNode, logger));
			}

			return element;
		}
	}
}
