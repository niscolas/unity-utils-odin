using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Plugins.OdinUtils.Editor.Extensions
{
	public static class OdinMenuItemExtensions
	{
		public static void BecomeDraggable(this OdinMenuItem item, Type type = null)
		{
			Rect itemRect = item.Rect;
			int id = DragAndDropUtilities.GetDragAndDropId(itemRect);

			if (type != null)
			{
				DragAndDropUtilities.DragZone(
					itemRect,
					item.Value,
					type,
					false,
					false,
					id);
			}
			else
			{
				DragAndDropUtilities.DragZone(
					itemRect,
					item.Value,
					false,
					false,
					id);
			}
		}
	}
}