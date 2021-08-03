using UnityEngine;

namespace OdinUtils.TheHub
{
	[CreateAssetMenu(menuName = CreateAssetMenuPath + "Module")]
	public class SettingsModule : Module<SettingsSubmodule>
	{
		public const string CreateAssetMenuPath = Constants.BaseCreateAssetMenuPath + "Settings/";
	}
}