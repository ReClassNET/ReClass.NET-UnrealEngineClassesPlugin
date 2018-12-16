using System;
using System.Drawing;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace UnrealEngineClassesPlugin.Nodes
{
	public class TArrayNode : BaseArrayNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size + sizeof(int) * 2;
		public override bool PerformCycleCheck => false;

		public override void Intialize()
		{
			var node = ClassNode.Create();
			node.Intialize();
			node.AddBytes(64);
			InnerNode = node;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			Count = view.Memory.ReadInt32(Offset + IntPtr.Size);

			return Draw(view, x, y, "TArray", HotSpotType.ChangeType);
		}

		protected override Size DrawChild(ViewInfo view, int x, int y)
		{
			var ptr = view.Memory.ReadIntPtr(Offset);
			if (!ptr.IsNull())
			{
				ptr = view.Memory.Process.ReadRemoteIntPtr(ptr + CurrentIndex * IntPtr.Size);
			}

			memory.Size = InnerNode.MemorySize;
			memory.Process = view.Memory.Process;
			memory.Update(ptr);

			var v = view.Clone();
			v.Address = ptr;
			v.Memory = memory;

			return InnerNode.Draw(v, x, y);
		}
	}
}
