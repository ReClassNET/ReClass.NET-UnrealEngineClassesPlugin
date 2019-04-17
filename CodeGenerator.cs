using System;
using ReClassNET.CodeGenerator;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using UnrealEngineClassesPlugin.Nodes;

namespace UnrealEngineClassesPlugin
{
	public class CodeGenerator : CustomCppCodeGenerator
	{
		public override bool CanHandle(BaseNode node)
		{
			switch (node)
			{
				case FDateTimeNode _:
				case FGuidNode _:
				case FQWordNode _:
				case FStringNode _:
				case TArrayNode _:
				case TSharedPtrNode _:
					return true;
			}

			return false;
		}

		public override string GetTypeDefinition(BaseNode node, GetTypeDefinitionFunc defaultGetTypeDefinitionFunc, ResolveWrappedTypeFunc defaultResolveWrappedTypeFunc, ILogger logger)
		{
			switch (node)
			{
				case FDateTimeNode _:
					return "FDateTime";
				case FGuidNode _:
					return "FGuid";
				case FQWordNode _:
					return "FQWord";
				case FStringNode _:
					return "FString";
				case TArrayNode arrayNode:
					return $"TArray<{defaultResolveWrappedTypeFunc(arrayNode.InnerNode, true, logger)}>";
				case TSharedPtrNode sharedPtrNode:
					return $"TSharedPtr<{defaultResolveWrappedTypeFunc(sharedPtrNode.InnerNode, true, logger)}>";
			}

			throw new InvalidOperationException("Can not handle node: " + node.GetType());
		}
	}
}
