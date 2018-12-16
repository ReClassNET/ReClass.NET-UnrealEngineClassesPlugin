using System;
using ReClassNET.CodeGenerator;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using UnrealEngineClassesPlugin.Nodes;

namespace UnrealEngineClassesPlugin
{
	public class CodeGenerator : ICustomCodeGenerator
	{
		/// <summary>Checks if the language is C++ and the node is a WeakPtrNode.</summary>
		/// <param name="node">The node to check.</param>
		/// <param name="language">The language to check.</param>
		/// <returns>True if we can generate code, false if not.</returns>
		public bool CanGenerateCode(BaseNode node, Language language)
		{
			if (language != Language.Cpp)
			{
				return false;
			}

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

		/// <summary>Gets the member definition of the node.</summary>
		/// <param name="node">The member node.</param>
		/// <param name="language">The language to generate.</param>
		/// <returns>The member definition of the node.</returns>
		public MemberDefinition GetMemberDefinition(BaseNode node, Language language, ILogger logger)
		{
			switch (node)
			{
				case FDateTimeNode _:
					return new MemberDefinition(node, "FDateTime");
				case FGuidNode _:
					return new MemberDefinition(node, "FGuid");
				case FQWordNode _:
					return new MemberDefinition(node, "FQWord");
				case FStringNode _:
					return new MemberDefinition(node, "FString");
				case TArrayNode arrayNode:
					return new MemberDefinition(node, $"TArray<{arrayNode.InnerNode.Name}>");
				case TSharedPtrNode sharedPtrNode:
					return new MemberDefinition(node, $"TSharedPtr<{sharedPtrNode.InnerNode.Name}>");
			}

			throw new InvalidOperationException("Can not handle node: " + node.GetType());
		}
	}
}
