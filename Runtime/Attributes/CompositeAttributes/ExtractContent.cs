using System;
using Sirenix.OdinInspector;

namespace Plugins.OdinUtils.Editor.Attributes.CompositeAttributes {
	[IncludeMyAttributes]
	[HideReferenceObjectPicker]
	[HideLabel]
	[InlineProperty]
	public class ExtractContent : Attribute{ }
}