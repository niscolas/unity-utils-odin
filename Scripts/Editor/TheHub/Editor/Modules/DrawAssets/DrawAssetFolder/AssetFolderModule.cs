using OdinUtils.TheHub;
using UnityEngine;

namespace niscolas.TheHub
{
	[CreateAssetMenu(menuName = CreateAssetMenuPath + "Module")]
	public class AssetFolderModule : Module<AssetFolderSubmodule>
	{
		public const string CreateAssetMenuPath = Constants.BaseCreateAssetMenuPath + "Draw Asset Folder/";
	}
}