using System.Linq;
using niscolas.UnityExtensions;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Plugins.OdinUtils.Editor.Extensions
{
	public static class OdinMenuTreeSelectionExtensions
	{
		public static void DeleteAll(this OdinMenuTreeSelection selection)
		{
			if (!selection.IsValid()) return;

			Object[] selectedItems = selection.AsUnityObjects();

			if (selectedItems.IsNullOrEmpty()) return;

			selectedItems.ForEach(DeleteSelectedItem);
		}

		private static void DeleteSelectedItem(Object item)
		{
			item.SelfDelete();
		}

		public static Object[] AsUnityObjects(this OdinMenuTreeSelection selection)
		{
			Object[] selectedItems = selection.SelectedValues
				.Cast<Object>()
				.ToArray();

			return selectedItems;
		}
	}
}