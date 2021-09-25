using System;
using System.Collections.Generic;
using System.Linq;
using niscolas.UnityExtensions;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace OdinUtils.TheHub
{
    [CreateAssetMenu(menuName = SettingsModule.CreateAssetMenuPath + "Submodule")]
    public class SettingsSubmodule : Submodule
    {
        public override IReadOnlyList<OdinMenuItem> DrawMenuTree(IHub hub, Module parentModule)
        {
            List<OdinMenuItem> menuItems = new List<OdinMenuItem>();

            DrawHubProfile(hub, menuItems);
            DrawAllModuleTypes(hub, menuItems);

            return menuItems;
        }

        private static void DrawHubProfile(IHub hub, ICollection<OdinMenuItem> menuItems)
        {
            OdinMenuItem menuItem = hub.Tree.AddObjectAtPath("The Hub", hub.Profile).First();
            menuItems.Add(menuItem);
        }

        private static void DrawAllModuleTypes(IHub hub, List<OdinMenuItem> menuItems)
        {
            Module[] modules = hub.Profile.Modules.ToArray();

            IEnumerable<Type> moduleConcreteTypes = typeof(Module).GetAllConcreteTypes();

            foreach (Type moduleConcreteType in moduleConcreteTypes)
            {
                IEnumerable<Module> modulesOfCurrentType = modules
                    .Where(
                        module => module.GetType() == moduleConcreteType);

                IEnumerable<OdinMenuItem> currentModuleMenuItems =
                    DrawModuleType(hub, moduleConcreteType, modulesOfCurrentType);

                menuItems.AddRange(currentModuleMenuItems);
            }
        }

        private static IEnumerable<OdinMenuItem> DrawModuleType
        (
            IHub hub, Type moduleType, IEnumerable<Module> modules
        )
        {
            List<OdinMenuItem> menuItems = new List<OdinMenuItem>();

            OdinMenuTree tree = hub.Tree;

            string menuPath = DrawModuleTypeFolder(moduleType, tree, menuItems);
            DrawCreateNewModule(hub, moduleType, menuPath, menuItems);
            DrawModules(hub, modules, menuItems, menuPath);

            return menuItems;
        }

        private static void DrawCreateNewModule(
            IHub hub,
            Type moduleType,
            string folderMenuPath,
            ICollection<OdinMenuItem> menuItems)
        {
            void OnModuleCreated(ScriptableObject newModule)
            {
                hub.Profile.Modules.Add(newModule as Module);
                EditorUtility.SetDirty(hub.Profile);
            }

            OdinMenuItem createModuleMenuItem = DrawAssetCreator.DrawMenuItem(
                hub, moduleType, folderMenuPath, OnModuleCreated);

            menuItems.Add(createModuleMenuItem);
        }

        private static string DrawModuleTypeFolder(
            Type moduleType, OdinMenuTree tree, ICollection<OdinMenuItem> menuItems)
        {
            string menuPath = $"All {moduleType.Name}s";

            OdinMenuItem moduleTypeFolderMenuItem = new OdinMenuItem(tree, menuPath, new EmptyDraw())
            {
                Icon = EditorIcons.Folder.Raw
            };
            tree.MenuItems.Add(moduleTypeFolderMenuItem);
            menuItems.Add(moduleTypeFolderMenuItem);

            return menuPath;
        }

        private static void DrawModules(
            IHub hub, IEnumerable<Module> modules, List<OdinMenuItem> menuItems, string menuPath)
        {
            modules = modules.ToArray();

            if (modules.IsNullOrEmpty()) return;

            foreach (Module module in modules)
            {
                DrawModule(hub, module, menuItems, menuPath);
            }
        }

        private static void DrawModule(IHub hub, Module module, List<OdinMenuItem> menuItems, string pathPrefix)
        {
            string menuPath = $"{pathPrefix}/{module.Title} [Module]";

            OdinMenuItem menuItem = hub.Tree
                .AddObjectAtPath(menuPath, module)
                .AddIcon(module.Icon)
                .First();

            menuItems.Add(menuItem);

            DrawCreateNewSubmodule(hub, module, menuItems, menuPath);
            DrawSubmodules(hub, module, menuItems, menuPath);
        }

        private static void DrawCreateNewSubmodule(
            IHub hub, Module module, ICollection<OdinMenuItem> menuItems, string pathPrefix)
        {
            Type submoduleType = module.SubmoduleType;

            void OnCreated(ScriptableObject newSubmodule)
            {
                module.AddSubmodule(newSubmodule as Submodule);
                EditorUtility.SetDirty(module);
            }

            OdinMenuItem createNewMenuItem = DrawAssetCreator.DrawMenuItem(
                hub, submoduleType, pathPrefix, OnCreated);

            menuItems.Add(createNewMenuItem);
        }

        private static void DrawSubmodules (
            IHub hub, Module module, List<OdinMenuItem> menuItems, string moduleMenuPath
        )
        {
            OdinMenuTree tree = hub.Tree;

            foreach (Submodule submodule in module.Submodules)
            {
                string submoduleMenuPath = $"{moduleMenuPath}/{submodule.Title} [Submodule]";
                IEnumerable<OdinMenuItem> submodulesMenuItems = tree
                    .AddObjectAtPath(submoduleMenuPath, submodule);

                menuItems.AddRange(submodulesMenuItems);
            }
        }
    }
}