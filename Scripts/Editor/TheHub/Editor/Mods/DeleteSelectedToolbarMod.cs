using niscolas.OdinUtils.Editor;
using Plugins.OdinUtils.Editor;
using Plugins.OdinUtils.Editor.Extensions;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace OdinUtils.TheHub
{
	[CreateAssetMenu(menuName = ToolbarModCreateAssetMenuPath + "Delete Selected Assets")]
	public class DeleteSelectedToolbarMod : ToolbarMod
	{
		public override void Draw(IHub hub)
		{
			void DrawButton()
			{
				if (SirenixEditorGUI.ToolbarButton("Delete Selected"))
				{
					DeleteSelectedItems(hub);
				}
			}

			Color drawColor = hub.Profile.DeleteItemColor;
			EditorUtility.DrawWithColor(DrawButton, drawColor);
		}

		private void DeleteSelectedItems(IHub hub)
		{
			OdinMenuTreeSelection selection = hub.Tree.Selection;
			selection.DeleteAll();

			hub.Targets = null;
			hub.RebuildMenuTree();
		}
	}
}