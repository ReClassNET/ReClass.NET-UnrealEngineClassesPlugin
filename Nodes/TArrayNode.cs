using System;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Nodes;

namespace UnrealEngineClassesPlugin.Nodes
{
	public class TArrayNode : BaseWrapperArrayNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size + sizeof(int) * 2;

		protected override bool PerformCycleCheck => false;

		public TArrayNode()
		{
			IsReadOnly = true;
		}

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "TArray";
			icon = null;
		}

		public override void Initialize()
		{
			var node = IntPtr.Size == 4 ? (BaseNode)new Hex32Node() : new Hex64Node();
			ChangeInnerNode(node);
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			Count = context.Memory.ReadInt32(Offset + IntPtr.Size);

			return Draw(context, x, y, "TArray");
		}

		protected override Size DrawChild(DrawContext context, int x, int y)
		{
			var ptr = context.Memory.ReadIntPtr(Offset);
			if (!ptr.IsNull())
			{
				ptr = context.Process.ReadRemoteIntPtr(ptr + CurrentIndex * IntPtr.Size);
			}

			memory.Size = InnerNode.MemorySize;
			memory.UpdateFrom(context.Process, ptr);

			var innerContext = context.Clone();
			innerContext.Address = ptr;
			innerContext.Memory = memory;

			return InnerNode.Draw(innerContext, x, y);
		}
	}
}
