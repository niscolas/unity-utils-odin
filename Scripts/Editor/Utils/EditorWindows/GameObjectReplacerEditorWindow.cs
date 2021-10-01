using niscolas.UnityExtensions;
using niscolas.UnityUtils.Core;
using niscolas.UnityUtils.Core.Editor;
using Plugins.OdinUtils.Editor.Extensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityUtils;

namespace Editor.EditorWindows
{
    public class GameObjectReplacerEditorWindow : OdinEditorWindow
    {
        private const float FirstColumnWidth = 0.6f;

        private const int LabelWidth = 100;
        
        private const string BaseHorizontalGroup = "base";
        private const string SettingsVerticalGroup = BaseHorizontalGroup + "/settings";
        private const string TargetsVerticalGroup = BaseHorizontalGroup + "/targets";
        private const string UndoLabel = "Replace GameObject";

        [HorizontalGroup(BaseHorizontalGroup, FirstColumnWidth)]
        [VerticalGroup(SettingsVerticalGroup)]
        [LabelWidth(LabelWidth)]
        [SerializeField]
        private GameObject _replacement;

        [VerticalGroup(SettingsVerticalGroup)]
        [LabelWidth(LabelWidth)]
        [SerializeField]
        private bool _copyPosition = true;

        [VerticalGroup(SettingsVerticalGroup)]
        [LabelWidth(LabelWidth)]
        [SerializeField]
        private bool _copyRotation = true;

        [VerticalGroup(SettingsVerticalGroup)]
        [LabelWidth(LabelWidth)]
        [SerializeField]
        private bool _copyScale = true;

        [VerticalGroup(SettingsVerticalGroup)]
        [LabelWidth(LabelWidth)]
        [SerializeField]
        private bool _copyParent = true;

        [VerticalGroup(TargetsVerticalGroup)]
        [LabelWidth(LabelWidth)]
        [ShowInInspector]
        private Transform[] Targets => Selection.transforms;

        private Transform OldInstancesParent => TheGameObjectUtility
            .FindOrCreate(nameof(OldInstancesParent)).transform;

        private static readonly ISpawnService SpawnService = new UnityInstantiateService();

        [MenuItem(Constants.ToolsMenuItemPrefix + "GameObject Replacer")]
        public static void Open()
        {
            GetWindow<GameObjectReplacerEditorWindow>().Show();
        }

        [Button("Replace")]
        private void ReplaceAll()
        {
            foreach (Transform currentTarget in Targets)
            {
                Replace(currentTarget, _replacement);
            }
        }

        private void Replace(Transform target, GameObject replacement)
        {
            if (!target)
            {
                return;
            }

            Transform replacementTransform = SpawnService.Spawn(replacement).transform;
            Undo.RegisterCreatedObjectUndo(replacementTransform.gameObject, UndoLabel);

            Undo.RecordObject(replacementTransform, UndoLabel);
            replacementTransform.Replace(target, _copyPosition, _copyRotation, _copyScale, _copyParent);

            Undo.SetTransformParent(target, OldInstancesParent, UndoLabel);
            target.parent = OldInstancesParent;
            
            Undo.RecordObject(target.gameObject, UndoLabel);
            target.gameObject.SetActive(false);
        }
    }
}