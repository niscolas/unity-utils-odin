using UnityEngine;

namespace OdinUtils.TheHub
{
	[CreateAssetMenu(menuName = CreateAssetMenuPath + "Module")]
	public class DesignAssetsModule : Module<DesignAssetsSubmodule>
	{
		public const string CreateAssetMenuPath = Constants.BaseCreateAssetMenuPath + "Design Assets/";
	}
}