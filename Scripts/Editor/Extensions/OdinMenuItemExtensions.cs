using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace niscolas.UnityUtils.Odin.Editor
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

        public static IEnumerable<OdinMenuItem> OnRightClick(this IEnumerable<OdinMenuItem> menuItems,
            Action<OdinMenuItem> action)
        {
            foreach (OdinMenuItem menuItem in menuItems)
            {
                menuItem.OnRightClick += action;

                yield return menuItem;
            }
        }

        public static IEnumerable<OdinMenuItem> BecomeDraggable(
            this IEnumerable<OdinMenuItem> menuItems, Type type = null)
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