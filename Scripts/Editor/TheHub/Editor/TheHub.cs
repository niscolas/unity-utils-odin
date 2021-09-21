using System.Collections.Generic;
using System.Linq;
using niscolas.UnityExtensions;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace OdinUtils.TheHub
{
    public class TheHub : OdinMenuEditorWindow, IHub
    {
        public TheHubProfile Profile { get; private set; }

        public IDrawAssetCreator CurrentDrawAssetCreator { get; set; }

        public OdinMenuTree Tree { get; private set; }

        public object[] Targets { get; set; }

        private IEnumerable<Module> PossibleModules => Profile.Modules;

        private bool CanDrawMenu => _selectedModule != null && _drewGuiOnce;

        private SubmoduleMenuItems _currentSubmoduleMenuItems = new SubmoduleMenuItems();

        private Module _selectedModule;
        private Submodule _selectedSubmodule;
        private bool _drewGuiOnce;
        private bool _shouldRebuildTree = true;

        [MenuItem("Tools/The Hub")]
        public static void Open()
        {
            CreateInstance<TheHub>().Show();
        }

        protected override void Initialize()
        {
            SetupProfile();
        }

        protected override void OnGUI()
        {
            CheckFocus();
            TryRebuildTree();
            DrawTitle();
            DrawModuleOptions();

            base.OnGUI();

            _drewGuiOnce = true;
        }

        private void CheckFocus()
        {
            if (!hasFocus)
            {
                RebuildMenuTree();
            }
        }

        protected override void OnBeginDrawEditors()
        {
            DrawTopToolbar();
        }

        public void RebuildMenuTree()
        {
            _shouldRebuildTree = true;
        }

        private void TryRebuildTree()
        {
            if (_shouldRebuildTree && Event.current.type == EventType.Layout)
            {
                ForceMenuTreeRebuild();
                _shouldRebuildTree = false;
            }
        }

        protected override IEnumerable<object> GetTargets()
        {
            if (!Targets.IsNullOrEmpty())
            {
                yield return Targets;
            }
            else
            {
                yield return null;
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            if (!_shouldRebuildTree) return MenuTree;

            Tree = new OdinMenuTree(true, OdinMenuStyle.TreeViewStyle);
            Tree.Selection.SelectionChanged -= UpdateTargets;
            Tree.Selection.SelectionChanged += UpdateTargets;

            if (!CanDrawMenu)
            {
                return Tree;
            }

            Tree.Config = new OdinMenuTreeDrawingConfig
            {
                AutoHandleKeyboardNavigation = false,
                AutoScrollOnSelectionChanged = true,
                DrawSearchToolbar = true
            };

            DrawSelectedModuleTree();

            return Tree;
        }

        private void DrawSelectedModuleTree()
        {
            if (_selectedModule)
            {
                _currentSubmoduleMenuItems = _selectedModule.DrawTree(this);
            }
        }

        private void SetupProfile()
        {
            string[] profileGuid = AssetDatabase.FindAssets($"t:{nameof(TheHubProfile)}");
            string profilePath = AssetDatabase.GUIDToAssetPath(profileGuid[0]);
            Profile = (TheHubProfile) AssetDatabase.LoadAssetAtPath(profilePath, typeof(TheHubProfile));
        }

        private void UpdateTargets(SelectionChangedType selectionChangedType)
        {
            OdinMenuTreeSelection selection = Tree.Selection;
            Targets = selection.SelectedValues.ToArray();

            DetermineSelectedSubmodule();
        }

        private void DetermineSelectedSubmodule()
        {
            OdinMenuTreeSelection selection = Tree.Selection;
            if (selection.Count == 1)
            {
                OdinMenuItem selectedItem = selection[0];
                _currentSubmoduleMenuItems.TryGetValue(selectedItem, out _selectedSubmodule);
            }
        }

        private void DrawTitle()
        {
            SirenixEditorGUI.Title(Profile.Title, Profile.SubTitle, TextAlignment.Center, true);
            EditorGUILayout.Space();
        }

        private void DrawModuleOptions()
        {
            if (_drewGuiOnce)
            {
                SirenixEditorGUI.BeginHorizontalToolbar(GUIStyle.none);
                DrawPossibleModuleToolbarButtons();
                SirenixEditorGUI.EndHorizontalToolbar();
            }

            EditorGUILayout.Space();
        }

        private void DrawPossibleModuleToolbarButtons()
        {
            PossibleModules.ForEach(DrawPossibleModuleToolbarButton);
        }

        private void DrawPossibleModuleToolbarButton(Module module)
        {
            bool isModuleActive = CheckIsModuleActive(module);
            GUIContent moduleGuiContent = GetModuleGuiContent(module);

            if (SirenixEditorGUI.ToolbarTab(isModuleActive, moduleGuiContent))
            {
                ActivateModule(module);
            }
        }

        private static GUIContent GetModuleGuiContent(Module module)
        {
            Texture moduleIcon = module.Icon;
            GUIContent moduleGuiContent;
            string moduleTitle = $" {module.Title}";

            if (moduleIcon)
            {
                moduleGuiContent = new GUIContent(moduleTitle, moduleIcon);
            }
            else
            {
                moduleGuiContent = new GUIContent(moduleTitle);
            }

            return moduleGuiContent;
        }

        private void ActivateModule(Module module)
        {
            if (_selectedModule != module)
            {
                _selectedModule = module;
                RebuildMenuTree();
            }
        }

        private bool CheckIsModuleActive(Module module)
        {
            bool isModuleActive = false;

            if (_selectedModule != null)
            {
                isModuleActive = module == _selectedModule;
            }

            return isModuleActive;
        }

        private void DrawTopToolbar()
        {
            if (_selectedModule == null) return;

            SirenixEditorGUI.BeginHorizontalToolbar();
            GUILayout.FlexibleSpace();

            DrawTopToolbarMods();

            SirenixEditorGUI.EndHorizontalToolbar();
        }

        private void DrawTopToolbarMods()
        {
            if (Tree == null || !_selectedSubmodule) return;

            _selectedSubmodule.DrawToolbarMods(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (MenuTree != null)
            {
                MenuTree.Selection.SelectionChanged -= UpdateTargets;
            }

            if (CurrentDrawAssetCreator != null && CurrentDrawAssetCreator.Data != null)
            {
                DestroyImmediate(CurrentDrawAssetCreator.Data);
            }
        }
    }
}