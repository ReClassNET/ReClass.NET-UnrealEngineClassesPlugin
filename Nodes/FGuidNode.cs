using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace UnrealEngineClassesPlugin.Nodes
{
	public class FGuidNode : BaseNode
	{
		public override int MemorySize => sizeof(int) * 4;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "FGuid";
			icon = null;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(context, x, y);
			}

			var a = context.Memory.ReadUInt32(Offset);
			var b = context.Memory.ReadUInt32(Offset + 4);
			var c = context.Memory.ReadUInt32(Offset + 8);
			var d = context.Memory.ReadUInt32(Offset + 12);

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);
			x = AddIcon(context, x, y, context.IconProvider.Text, HotSpot.NoneId, HotSpotType.None);

			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "FGuid") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;

			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, "=") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, $"{{ {a:08X}-{b >> 16:04X}-{b & 0xFFFF:04X}-{c >> 16:04X}-{c & 0xFFFF:04X}{d:08X} }}") + context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			return new Size(x - origX, context.Font.Height);
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			return IsHidden ? HiddenHeight : context.Font.Height;
		}
	}
}
