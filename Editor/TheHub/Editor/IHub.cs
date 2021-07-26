using Sirenix.OdinInspector.Editor;

namespace OdinUtils.TheHub
{
	public interface IHub
	{
		IDrawAssetCreator CurrentDrawAssetCreator { get; set; }
		TheHubProfile Profile { get; }
		OdinMenuTree Tree { get; }
		object[] Targets { get; set; }

		void RebuildMenuTree();
	}
}