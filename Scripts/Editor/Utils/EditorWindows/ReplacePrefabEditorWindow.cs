using niscolas.UnityExtensions;
using Plugins.OdinUtils.Editor.Extensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorWindows
{
	public class ReplacePrefabEditorWindow : OdinEditorWindow
	{
		[AssetsOnly]
		[SerializeField]
		private GameObject _oldPrefab;

		[SerializeField]
		private GameObject _newPrefab;

		[SerializeField]
		private bool _copyPosition = true;

		[SerializeField]
		private bool _copyRotation = true;

		[SerializeField]
		private bool _copyScale = true;

		[SerializeField]
		private bool _copyParent = true;

		private Transform _oldInstancesParent;

		private Transform OldInstancesParent
		{
			get
			{
				if (!_oldInstancesParent)
				{
					string oldInstancesParentName = nameof(OldInstancesParent);
					GameObject oldInstancesParentGameObject = GameObject.Find(oldInstancesParentName);
					if (!oldInstancesParentGameObject)
					{
						oldInstancesParentGameObject = new GameObject(oldInstancesParentName);
					}

					_oldInstancesParent = oldInstancesParentGameObject.transform;
				}

				return _oldInstancesParent;
			}
		}

		[MenuItem("Tools/Replace Prefabs")]
		public static void Open()
		{
			ReplacePrefabEditorWindow window = GetWindow<ReplacePrefabEditorWindow>();
			window.Show();
		}

		[Button]
		private void ReplaceAll()
		{
			GameObject[] allGameObjects = (GameObject[]) FindObjectsOfType(typeof(GameObject));

			foreach (GameObject currentGameObjectInstance in allGameObjects)
			{
				if (PrefabUtility.GetPrefabInstanceStatus(currentGameObjectInstance) == PrefabInstanceStatus.Connected)
				{
					Object currentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(currentGameObjectInstance);
					if (currentPrefab == _oldPrefab)
					{
						Replace(currentGameObjectInstance, _newPrefab);
					}
				}
			}
		}

		private void Replace(GameObject oldInstance, GameObject newPrefab)
		{
			if (!oldInstance) return;

			GameObject newInstanceGameObject = (GameObject) PrefabUtility.InstantiatePrefab(newPrefab);
			oldInstance.ReplaceWith(newInstanceGameObject, _copyPosition, _copyRotation, _copyScale, _copyParent);
			
			oldInstance.transform.parent = OldInstancesParent;
			oldInstance.SetActive(false);
		}
	}
}