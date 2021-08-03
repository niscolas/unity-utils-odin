using System;
using Sirenix.OdinInspector;

namespace Plugins.OdinUtils.Editor.Attributes.CompositeAttributes {
	[IncludeMyAttributes]
	[AssetsOnly]
	[PreviewField(100, ObjectFieldAlignment.Left)]
	[HideLabel]
	public class SmallPreviewAttribute : Attribute { }
}