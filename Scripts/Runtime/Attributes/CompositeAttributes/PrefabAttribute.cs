using System;
using Sirenix.OdinInspector;

namespace Plugins.OdinUtils.Editor.Attributes.CompositeAttributes {
	[IncludeMyAttributes]
	[AssetSelector(Paths ="Assets/Prefabs", DropdownWidth = 500)]
	[AssetsOnly]
	public class PrefabAttribute : Attribute { }
}