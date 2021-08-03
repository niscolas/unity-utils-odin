using UnityEngine;

namespace OdinUtils.TheHub
{
	public abstract class HubMod : ScriptableObject
	{
		public const string CreateAssetMenuPath = Constants.BaseCreateAssetMenuPath + "Mods/";
		public abstract void Draw(IHub hub);
	}
}