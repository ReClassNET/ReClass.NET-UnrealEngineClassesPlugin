using System;
using System.Drawing;
using ReClassNET.Plugins;

namespace UnrealEngineClassesPlugin
{
	public class UnrealPluginExt : Plugin
	{
		private IPluginHost host;

		public override Image Icon => Properties.Resources.B16x16_Icon;

		public override bool Initialize(IPluginHost host)
		{
			System.Diagnostics.Debugger.Launch();

			if (this.host != null)
			{
				Terminate();
			}

			this.host = host ?? throw new ArgumentNullException(nameof(host));

			

			return true;
		}

		public override void Terminate()
		{
			
		}
	}
}
