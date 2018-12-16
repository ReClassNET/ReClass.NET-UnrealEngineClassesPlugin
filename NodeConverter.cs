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
	public class NodeConverter : ICustomNodeConverter
	{
		/// <summary>Name of the type used in the XML data.</summary>
		private const string XmlTypePrefix = "UnrealEngineClasses.";

		public bool CanHandleNode(BaseNode node) => node is FDateTimeNode || node is FGuidNode || node is FQWordNode || node is FStringNode || node is TArrayNode || node is TSharedPtrNode;

		public bool CanHandleElement(XElement element) => element.Attribute(ReClassNetFile.XmlTypeAttribute)?.Value.StartsWith(XmlTypePrefix) == true;

		public bool TryCreateNodeFromElement(XElement element, ClassNode parent, IEnumerable<ClassNode> classes, ILogger logger, out BaseNode node)
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
					var reference = NodeUuid.FromBase64String(element.Attribute(ReClassNetFile.XmlReferenceAttribute)?.Value, false);
					var innerClass = classes.FirstOrDefault(c => c.Uuid.Equals(reference));
					if (innerClass == null)
					{
						logger.Log(LogLevel.Warning, $"Skipping node with unknown reference: {reference}");
						logger.Log(LogLevel.Warning, element.ToString());

						return false;
					}

					if (type == XmlTypePrefix + "TArray")
					{
						node = new TArrayNode();
					}
					else
					{
						node = new TSharedPtrNode();
					}
					((BaseReferenceNode)node).ChangeInnerNode(innerClass);

					break;
				}
				default:
					throw new InvalidOperationException("Can not handle node type: " + type);
			}

			node.Name = element.Attribute(ReClassNetFile.XmlNameAttribute)?.Value ?? string.Empty;
			node.Comment = element.Attribute(ReClassNetFile.XmlCommentAttribute)?.Value ?? string.Empty;

			return true;
		}

		public XElement CreateElementFromNode(BaseNode node, ILogger logger)
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
				case TArrayNode arrayNode:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "TArray");
					element.SetAttributeValue(ReClassNetFile.XmlReferenceAttribute, arrayNode.InnerNode.Uuid.ToBase64String());
					break;
				case TSharedPtrNode sharedPtrNode:
					element.SetAttributeValue(ReClassNetFile.XmlTypeAttribute, XmlTypePrefix + "TSharedPtr");
					element.SetAttributeValue(ReClassNetFile.XmlReferenceAttribute, sharedPtrNode.InnerNode.Uuid.ToBase64String());
					break;
			}

			return element;
		}
	}
}
