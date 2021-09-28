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

        private bool CanDrawMenu => _activeModule != null && _drewGuiOnce;

        private SubmoduleMenuItems _currentSubmoduleMenuItems = new SubmoduleMenuItems();

        private Module _activeModule;
        private Submodule _selectedSubmodule;
        private bool _drewGuiOnce;
        private static bool _shouldRebuildTree = true;

        [MenuItem("Tools/The Hub")]
        public static void Open()
        {
            CreateInstance<TheHub>().Show();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Static_RebuildMenuTree();
        }

        protected override void Initialize()
        {
            SetupProfile();
        }

        protected override void OnGUI()
        {
            TryRebuildTree();
            DrawTitle();
            DrawModuleOptions();

            base.OnGUI();

            _drewGuiOnce = true;
        }

        protected override void OnBeginDrawEditors()
        {
            DrawTopToolbar();
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
            if (!_shouldRebuildTree)
            {
                return MenuTree;
            }

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

        private void SetupProfile()
        {
            string[] profileGuid = AssetDatabase.FindAssets($"t:{nameof(TheHubProfile)}");
            string profilePath = AssetDatabase.GUIDToAssetPath(profileGuid[0]);
            Profile = (TheHubProfile) AssetDatabase.LoadAssetAtPath(profilePath, typeof(TheHubProfile));
        }

        private void TryRebuildTree()
        {
            if (!_shouldRebuildTree ||
                Event.current.type != EventType.Layout)
            {
                return;
            }

            ForceMenuTreeRebuild();
            _shouldRebuildTree = false;
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
            bool isActive = CheckIsActiveModule(module);
            GUIContent guiContent = GetModuleGuiContent(module);

            if (!SirenixEditorGUI.ToolbarTab(isActive, guiContent) ||
                isActive) return;
                
            
            ActivateModule(module);
        }

        private bool CheckIsActiveModule(Module module)
        {
            if (_activeModule)
            {
                return module == _activeModule;
            }

            return false;
        }

        private static GUIContent GetModuleGuiContent(Module module)
        {
            Texture moduleIcon = module.Icon;
            GUIContent guiContent;
            string moduleTitle = $" {module.Title}";

            if (moduleIcon)
            {
                guiContent = new GUIContent(moduleTitle, moduleIcon);
            }
            else
            {
                guiContent = new GUIContent(moduleTitle);
            }

            return guiContent;
        }

        private void ActivateModule(Module module)
        {
            _activeModule = module;
            Static_RebuildMenuTree();
        }

        public static void Static_RebuildMenuTree()
        {
            _shouldRebuildTree = true;
        }

        public void RebuildMenuTree()
        {
            Static_RebuildMenuTree();
        }

        private void DrawSelectedModuleTree()
        {
            if (_activeModule)
            {
                _currentSubmoduleMenuItems = _activeModule.DrawTree(this);
            }
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

        private void DrawTopToolbar()
        {
            if (_activeModule == null) return;

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
    }
}