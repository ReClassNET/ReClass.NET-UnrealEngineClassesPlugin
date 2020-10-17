using System;
using System.Drawing;
using ReClassNET;
using ReClassNET.Controls;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace UnrealEngineClassesPlugin.Nodes
{
	public class TSharedPtrNode : BaseWrapperNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size * 2;

		protected override bool PerformCycleCheck => false;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "TSharedPtr";
			icon = null;
		}

		public override bool CanChangeInnerNodeTo(BaseNode node)
		{
			switch (node)
			{
				case ClassNode _:
				case VirtualMethodNode _:
					return false;
			}

			return true;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(context, x, y, context.Font.Height);

			if (InnerNode != null)
			{
				x = AddOpenCloseIcon(context, x, y);
			}
			else
			{
				x = AddIconPadding(context, x);
			}
			x = AddIcon(context, x, y, context.IconProvider.Pointer, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "TSharedPtr") + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}
			if (InnerNode == null)
			{
				x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, "<void>") + context.Font.Width;
			}
			x = AddIcon(context, x, y, context.IconProvider.Change, 4, HotSpotType.ChangeWrappedType) + context.Font.Width;

			var ptr = context.Memory.ReadIntPtr(Offset);

			x = AddText(context, x, y, context.Settings.OffsetColor, HotSpot.NoneId, "->") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.ValueColor, 0, "0x" + ptr.ToString(Constants.AddressHexFormat)) + context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			y += context.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[context.Level] && InnerNode != null)
			{
				memory.Size = InnerNode.MemorySize;
				memory.UpdateFrom(context.Process, ptr);

				var v = context.Clone();
				v.Address = ptr;
				v.Memory = memory;

				var innerSize = InnerNode.Draw(v, tx, y);

				size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
				size.Height += innerSize.Height;
			}

			return size;
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = context.Font.Height;
			if (LevelsOpen[context.Level] && InnerNode != null)
			{
				height += InnerNode.CalculateDrawnHeight(context);
			}
			return height;
		}
	}
}
