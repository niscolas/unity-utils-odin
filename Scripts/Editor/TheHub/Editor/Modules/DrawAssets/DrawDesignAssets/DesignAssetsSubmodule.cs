using System.Collections.Generic;
using System.Linq;
using niscolas.UnityExtensions;
using Plugins.OdinUtils.Editor.Attributes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace OdinUtils.TheHub
{
	[CreateAssetMenu(menuName = DesignAssetsModule.CreateAssetMenuPath + "Submodule")]
	public class DesignAssetsSubmodule : Submodule
	{
		[Title("Design Assets Settings")]
		[NotEmpty]
		[Required]
		[AssetsOnly]
		[SerializeField]
		private List<Object> _designAssets = new List<Object>();

		private string FolderPath => $"{Title}";
		private string DropZoneTitle => $"Add new Design Asset in {Title}";

		public override IEnumerable<OdinMenuItem> DrawMenuTree(IHub hub, Module parentModule)
		{
			if (!_designAssets.IsValid()) return default;

			OdinMenuTree tree = hub.Tree;

			List<OdinMenuItem> menuItems = new List<OdinMenuItem>();

			menuItems.Add(DrawFolder(tree));
			menuItems.AddRange(DrawDesignAssets(tree));

			return menuItems;
		}

		private IEnumerable<OdinMenuItem> DrawDesignAssets(OdinMenuTree tree)
		{
			foreach (Object designAsset in _designAssets)
			{
				yield return DrawDesignAsset(tree, designAsset);
			}
		}

		private OdinMenuItem DrawDesignAsset(OdinMenuTree tree, Object designAsset)
		{
			string menuPath = $"{FolderPath}/{designAsset.name}";
			OdinMenuItem menuItem = tree.AddObjectAtPath(menuPath, designAsset).First();

			return menuItem;
		}

		private OdinMenuItem DrawFolder(OdinMenuTree tree)
		{
			OdinMenuItem folderItem = new OdinMenuItem(tree, FolderPath, new EmptyDraw())
			{
				Icon = Icon
			};
			tree.MenuItems.Add(folderItem);

			return folderItem;
		}
	}
}