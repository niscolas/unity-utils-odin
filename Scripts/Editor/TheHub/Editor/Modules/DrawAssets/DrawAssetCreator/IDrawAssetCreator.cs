using System;
using UnityEngine;

namespace OdinUtils.TheHub
{
	public interface IDrawAssetCreator
	{
		string FolderPath { get; }
		ScriptableObject Data { get; }
		Action<ScriptableObject> CreatedCallback { get; set; }

		void LinkHub(IHub hub);
	}
}