using UnityEngine;

namespace OdinUtils.TheHub
{
	[CreateAssetMenu(menuName = CreateAssetMenuPath + "Module")]
	public class AssetFolderModule : Module<AssetFolderSubmodule>
	{
		public const string CreateAssetMenuPath = Constants.BaseCreateAssetMenuPath + "Draw Asset Folder/";
	}
}