using System;
using System.Collections.Generic;
using niscolas.UnityExtensions;
using Plugins.OdinUtils.Editor.Extensions;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
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

		public static IEnumerable<OdinMenuItem> OnRightClick(this IEnumerable<OdinMenuItem> menuItems, Action<OdinMenuItem> action)
		{
			foreach (OdinMenuItem menuItem in menuItems)
			{
				menuItem.OnRightClick += action;
				
				yield return menuItem;
			}
		}

		public static IEnumerable<OdinMenuItem> BecomeDraggable(this IEnumerable<OdinMenuItem> menuItems, Type type = null)
		{
			void OnMenuItemDraw(OdinMenuItem menuItem)
			{
				menuItem.BecomeDraggable(type);
			}

			foreach (OdinMenuItem menuItem in menuItems)
			{
				menuItem.OnDrawItem += OnMenuItemDraw;
				
				yield return menuItem;
			}
		}
	}
}