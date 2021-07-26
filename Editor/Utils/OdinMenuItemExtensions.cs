using System;
using System.Collections.Generic;
using Plugins.OdinUtils.Editor.Extensions;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityExtensions;
using Object = UnityEngine.Object;

namespace Plugins.OdinUtils.Editor
{
	public static class OdinMenuItemExtensions
	{
		public static void PingAsset(this OdinMenuItem item)
		{
			if (item.Value is Object unityObj)
			{
				EditorGUIUtility.PingObject(unityObj);
			}
		}

		public static void OnRightClick(this IEnumerable<OdinMenuItem> items, Action<OdinMenuItem> action)
		{
			items = items.AsArray();
			foreach (OdinMenuItem item in items)
			{
				item.OnRightClick += action;
			}
		}

		public static void BecomeDraggable(this IEnumerable<OdinMenuItem> odinMenuItems, Type type = null)
		{
			void OnMenuItemDraw(OdinMenuItem item)
			{
				item.BecomeDraggable(type);
			}

			foreach (OdinMenuItem menuItem in odinMenuItems)
			{
				menuItem.OnDrawItem += OnMenuItemDraw;
			}
		}
	}
}