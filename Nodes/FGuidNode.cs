using System.Drawing;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace UnrealEngineClassesPlugin.Nodes
{
	public class FGuidNode : BaseNode
	{
		public override int MemorySize => sizeof(int) * 4;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			var a = view.Memory.ReadUInt32(Offset);
			var b = view.Memory.ReadUInt32(Offset + 4);
			var c = view.Memory.ReadUInt32(Offset + 8);
			var d = view.Memory.ReadUInt32(Offset + 12);

			DrawInvalidMemoryIndicator(view, y);

			var origX = x;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;
			x = AddIcon(view, x, y, Icons.Text, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "FGuid") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, "=") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, $"{{ {a:08X}-{b >> 16:04X}-{b & 0xFFFF:04X}-{c >> 16:04X}-{c & 0xFFFF:04X}{d:08X} }}") + view.Font.Width;

			x = AddComment(view, x, y);

			AddTypeDrop(view, y);
			AddDelete(view, y);

			return new Size(x - origX, view.Font.Height);
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			return IsHidden ? HiddenHeight : view.Font.Height;
		}
	}
}
