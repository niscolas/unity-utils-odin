﻿using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityExtensions;

namespace OdinUtils.TheHub
{
	public abstract class DrawAssetCreator : IDrawAssetCreator
	{
		public abstract string FolderPath { get; set; }
		public abstract ScriptableObject Data { get; set; }
		public abstract Action<ScriptableObject> CreatedCallback { get; set; }
		public abstract void LinkHub(IHub hub);

		public static OdinMenuItem DrawMenuItem
		(
			IHub hub,
			Type type,
			string menuPathPrefix = "",
			string targetFolder = "",
			Action<ScriptableObject> createdCallback = null
		)
		{
			if (type == null)
			{
				return default;
			}

			OdinMenuTree tree = hub.Tree;
			Type genericType = typeof(DrawAssetCreator<>);
			string menuPath = $"{menuPathPrefix}/Create New {type.Name}";
			Color createNewItemColor = hub.Profile.CreateNewItemColor;

			OdinMenuStyle style = new OdinMenuStyle
			{
				SelectedColorDarkSkin = createNewItemColor,
				SelectedColorLightSkin = createNewItemColor,
				SelectedInactiveColorDarkSkin = createNewItemColor,
				SelectedInactiveColorLightSkin = createNewItemColor
			};

			IDrawAssetCreator drawAssetCreator = (IDrawAssetCreator) genericType.CreateInstance(type);
			drawAssetCreator.LinkHub(hub);
			drawAssetCreator.FolderPath = targetFolder;
			drawAssetCreator.CreatedCallback = createdCallback;

			hub.CurrentDrawAssetCreator = drawAssetCreator;

			OdinMenuItem menuItem = tree
				.AddObjectAtPath(menuPath, drawAssetCreator)
				.AddIcon(EditorIcons.Plus.Raw)
				.Last();
			menuItem.Style = style;

			// EditorUtility.DrawWithColor(DrawMenuItem, hub.Profile.CreateNewItemColor);

			return menuItem;
		}
	}

	public class DrawAssetCreator<T> : DrawAssetCreator where T : ScriptableObject
	{
		private const string HorizontalCreateNew = "Horizontal";

		[HorizontalGroup(HorizontalCreateNew, 0.85f, 0, 10)]
		[VerticalGroup(HorizontalCreateNew + "/Name")]
		[LabelWidth(100)]
		[SerializeField]
		private string _name;

		[Title("New Asset Data", "This asset will be created and placed on the right folder, automagically :)")]
		[InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
		[SerializeField]
		private T _data;

		public override ScriptableObject Data
		{
			get => _data;
			set => _data = (T) value;
		}

		public override Action<ScriptableObject> CreatedCallback { get; set; }

		public override string FolderPath { get; set; }

		private IHub _hub;

		public DrawAssetCreator()
		{
			_data = ScriptableObject.CreateInstance<T>();
		}

		public override void LinkHub(IHub hub)
		{
			_hub = hub;
		}

		[VerticalGroup(HorizontalCreateNew + "/CreateButton")]
		[GUIColor("@Color.green")]
		[Button("Create")]
		private void CreateNewData()
		{
			_data.Create($"{FolderPath}/{_name}.asset");

			CreatedCallback?.Invoke(_data);

			_hub?.RebuildMenuTree();
		}
	}
}