using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Plugins.OdinUtils.Editor.Utils.EditorWindows
{
	public class RevertPrefabInstanceEditorWindow : OdinEditorWindow
	{
		[SerializeField]
		private bool _keepTransform;

		[Title("Debug")]
		[ReadOnly]
		[ShowInInspector]
		private GameObject[] Selection => UnityEditor.Selection.gameObjects;

		[MenuItem("Tools/Revert Prefab Instance")]
		public static void Open()
		{
			RevertPrefabInstanceEditorWindow window = GetWindow<RevertPrefabInstanceEditorWindow>();
			window.Show();
		}

		[Button]
		private void RevertSelection()
		{
			foreach (GameObject instance in UnityEditor.Selection.gameObjects)
			{
				if (_keepTransform)
				{
					ComponentUtility.CopyComponent(instance.transform);
				}

				PrefabUtility.RevertPrefabInstance(instance, InteractionMode.UserAction);

				if (_keepTransform)
				{
					ComponentUtility.PasteComponentValues(instance.transform);
				}
			}
		}
	}
}