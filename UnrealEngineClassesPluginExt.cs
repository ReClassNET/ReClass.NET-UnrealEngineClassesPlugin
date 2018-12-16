using System;
using System.Drawing;
using ReClassNET.Plugins;
using UnrealEngineClassesPlugin.Nodes;

namespace UnrealEngineClassesPlugin
{
	public class UnrealEngineClassesPluginExt : Plugin
	{
		private IPluginHost host;

		private readonly NodeConverter converter = new NodeConverter();
		private readonly CodeGenerator generator = new CodeGenerator();

		public override Image Icon => Properties.Resources.B16x16_Icon;

		public override bool Initialize(IPluginHost host)
		{
			System.Diagnostics.Debugger.Launch();

			if (this.host != null)
			{
				Terminate();
			}

			this.host = host ?? throw new ArgumentNullException(nameof(host));

			host.RegisterNodeType(typeof(TArrayNode), "TArray", Icon, converter, generator);

			return true;
		}

		public override void Terminate()
		{
			
		}
	}
}
