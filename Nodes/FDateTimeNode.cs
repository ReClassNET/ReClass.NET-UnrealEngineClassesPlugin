using System;
using System.Drawing;
using System.Globalization;
using ReClassNET.Controls;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace UnrealEngineClassesPlugin.Nodes
{
	public class FDateTimeNode : BaseNode
	{
		public override int MemorySize => sizeof(long);

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "FDateTime";
			icon = null;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(context, x, y);
			}

			var ticks = context.Memory.ReadInt64(Offset);

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);
			x = AddIcon(context, x, y, context.IconProvider.Text, HotSpot.NoneId, HotSpotType.None);

			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "FDateTime") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;

			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, "=") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, new DateTime(ticks).ToString(CultureInfo.CurrentCulture)) + context.Font.Width;

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
