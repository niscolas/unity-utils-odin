using System;
using System.Collections.Generic;
using System.Linq;
using niscolas.UnityExtensions;
using Plugins.OdinUtils.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace OdinUtils.TheHub
{
	[CreateAssetMenu(menuName = AssetFolderModule.CreateAssetMenuPath + "Submodule")]
	public class AssetFolderSubmodule : Submodule
	{
		[Title("Settings")]
		[InlineButton(nameof(PrintTypeName), "Validate")]
		[SerializeField]
		private string _fullTypeName;

		[SerializeField]
		private bool _autoFolder;

		[HideIf(nameof(_autoFolder))]
		[Required]
		[FolderPath]
		[SerializeField]
		private string _folder;

		[SerializeField]
		private bool _includeSubFolders;

		public Type Type => _fullTypeName.FindType();

		public bool IncludeSubFolders => _includeSubFolders;

		private string TitleMenuPath => $"{Title}";

		private string FolderPath => _folder;

		private string GetFolderPath(IHub hub, Module parentModule)
		{
			if (!_autoFolder)
			{
				return FolderPath;
			}

			string assetsFolderPath = hub.Profile.AssetsFolderPath;
			string parentModuleName = $"{parentModule.Title.WithoutSpaces()}Module";
			string submoduleName = Title.WithoutSpaces();
			string autoFolderPath = $"{assetsFolderPath}/{parentModuleName}/{submoduleName}";

			return autoFolderPath;
		}

		private void PrintTypeName()
		{
			Type type = Type;
			if (type != null)
			{
				Debug.Log(type.AssemblyQualifiedName);
			}
			else
			{
				Debug.LogWarning("The Type for the given TypeName wasn't found :(");
			}
		}

		public override IEnumerable<OdinMenuItem> DrawMenuTree(IHub hub, Module parentModule)
		{
			List<OdinMenuItem> menuItems = new List<OdinMenuItem>();

			DrawFolderMenuItem(hub, menuItems);
			DrawTreeItems(hub, parentModule, menuItems);

			string folderPath = GetFolderPath(hub, parentModule);
			OdinMenuItem createNewMenuItem = DrawAssetCreator.DrawMenuItem(hub, Type, TitleMenuPath, folderPath);
			menuItems.Add(createNewMenuItem);

			return menuItems;
		}

		private void DrawFolderMenuItem(IHub hub, ICollection<OdinMenuItem> menuItems)
		{
			OdinMenuItem menuItem = hub.Tree
				.AddObjectAtPath(TitleMenuPath, new EmptyDraw())
				.AddIcon(Icon)
				.Last();

			menuItems.Add(menuItem);
		}

		private void DrawTreeItems(IHub hub, Module parentModule, List<OdinMenuItem> menuItems)
		{
			Type type = Type;

			if (type == null) return;

			string folderPath = GetFolderPath(hub, parentModule);

			IEnumerable<OdinMenuItem> newMenuItems = hub.Tree
				.AddAllAssetsAtPath(TitleMenuPath, folderPath, type, IncludeSubFolders)
				.SortMenuItemsByName(false)
				.AddThumbnailIcons(true)
				.AsArray();

			newMenuItems.OnRightClick(item => item.PingAsset());
			newMenuItems.BecomeDraggable(type);

			menuItems.AddRange(newMenuItems);
		}
	}
}