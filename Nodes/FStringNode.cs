using System;
using System.Drawing;
using System.Text;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace UnrealEngineClassesPlugin.Nodes
{
	public class FStringNode : BaseNode
	{
		public override int MemorySize => IntPtr.Size + sizeof(int) * 2;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "FString";
			icon = null;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var ptr = context.Memory.ReadIntPtr(Offset);
			var length = context.Memory.ReadInt32(Offset + IntPtr.Size);
			var text = context.Process.ReadRemoteString(ptr, Encoding.Unicode, length);

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);
			x = AddIcon(context, x, y, context.IconProvider.Text, HotSpot.NoneId, HotSpotType.None);

			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "FString") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;

			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, "= '");
			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.ReadOnlyId, text);
			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, "'") + context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			return new Size(x - origX, context.Font.Height);
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			return IsHidden && !IsWrapped ? HiddenHeight : context.Font.Height;
		}
	}
}
