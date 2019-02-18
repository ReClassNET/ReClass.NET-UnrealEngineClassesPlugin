using System.Drawing;
using ReClassNET.Plugins;
using UnrealEngineClassesPlugin.Nodes;

namespace UnrealEngineClassesPlugin
{
	public class UnrealEngineClassesPluginExt : Plugin
	{
		public override Image Icon => Properties.Resources.B16x16_Icon;

		public override CustomNodeTypes GetCustomNodeTypes()
		{
			return new CustomNodeTypes
			{
				CodeGenerator = new CodeGenerator(),
				Serializer = new NodeConverter(),
				NodeTypes = new[] { typeof(TArrayNode), typeof(TSharedPtrNode), typeof(FStringNode), typeof(FQWordNode), typeof(FGuidNode), typeof(FDateTimeNode) }
			};
		}
	}
}
