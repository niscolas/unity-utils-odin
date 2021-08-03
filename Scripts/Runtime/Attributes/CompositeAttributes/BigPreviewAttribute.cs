using System;
using Sirenix.OdinInspector;

namespace Plugins.OdinUtils.Editor.Attributes.CompositeAttributes {
	[IncludeMyAttributes]
	[PreviewField(200, ObjectFieldAlignment.Left)]
	[HideLabel]
	public class BigPreviewAttribute : Attribute { }
}