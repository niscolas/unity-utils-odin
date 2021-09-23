using System;
using System.Collections.Generic;
using System.Linq;
using niscolas.UnityExtensions;
using niscolas.UnityUtils.Core;
using niscolas.UnityUtils.Core.Editor;
using niscolasPlugins.UnityUtils.Core;
using OdinUtils.TheHub;
using Plugins.OdinUtils.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace niscolas.TheHub
{
    [CreateAssetMenu(menuName = AssetFolderModule.CreateAssetMenuPath + "Submodule")]
    public class AssetFolderSubmodule : Submodule
    {
        [Title("Settings")]
        [InlineButton(nameof(PrintTypeName), "Validate")]
        [SerializeField]
        private string _fullTypeName;

        private string TitleMenuPath => $"{Title}";

        private void PrintTypeName()
        {
            TryFindType(out Type _);
        }

        private bool TryFindType(out Type type)
        {
            return TypeUtility.TryFindType(_fullTypeName, out type);
        }

        public override IReadOnlyList<OdinMenuItem> DrawMenuTree(IHub hub, Module parentModule)
        {
            List<OdinMenuItem> menuItems = new List<OdinMenuItem>();
            
            OdinMenuTree tree = hub.Tree;

            if (!TryFindType(out Type type))
            {
                return default;
            }
            
            IEnumerable<Object> assets = AssetDatabaseUtility.FindAllAssetsOfType(type);

            DrawFolderMenuItem(tree, menuItems);
            
            DrawCreateNewItemMenuItem(hub, type, menuItems);

            DrawTreeItems(tree, menuItems, assets, type);

            return menuItems;
        }

        private void DrawCreateNewItemMenuItem(
            IHub hub, Type type, List<OdinMenuItem> menuItems)
        {
            OdinMenuItem createNewMenuItem = DrawAssetCreator.DrawMenuItem(hub, type, TitleMenuPath);
            menuItems.Add(createNewMenuItem);
        }

        private void DrawFolderMenuItem(OdinMenuTree tree, ICollection<OdinMenuItem> menuItems)
        {
            OdinMenuItem menuItem = tree
                .AddObjectAtPath(TitleMenuPath, new EmptyDraw())
                .AddIcon(Icon)
                .Last();

            menuItems.Add(menuItem);
        }

        private void DrawTreeItems(
            OdinMenuTree tree, 
            List<OdinMenuItem> menuItems, 
            IEnumerable<Object> assets, 
            Type type)
        {
            foreach (Object asset in assets)
            {
                tree.AddAssetAtPath(
                    $"{TitleMenuPath}/{asset.name}", 
                    asset.Path(), 
                    type);
            }

            OdinMenuItem[] newMenuItems = tree
                .SortMenuItemsByName(false)
                .AddThumbnailIcons(true)
                .OnRightClick(item => item.PingAsset())
                .BecomeDraggable(type)
                .ToArray();

            menuItems.AddRange(newMenuItems);
        }
    }
}