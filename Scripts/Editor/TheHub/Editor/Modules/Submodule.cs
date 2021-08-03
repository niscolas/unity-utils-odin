using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityExtensions;

namespace OdinUtils.TheHub
{
	public abstract class Submodule : ScriptableObject
	{
		[SerializeField]
		private string title;

		[SerializeField]
		private Texture icon;

		[Title("Mods")]
		[SerializeField]
		private ToolbarMod[] _toolbarMods;

		[Title("Debug")]
		public string Title => title;

		public Texture Icon => icon;

		public virtual IEnumerable<OdinMenuItem> DrawMenuTree(IHub hub, Module parentModule)
		{
			throw new NotImplementedException();
		}

		public void DrawToolbarMods(IHub hub)
		{
			object[] drawTargets = hub.Targets;

			if (!ShouldDrawToolbarMods(drawTargets)) return;

			if (_toolbarMods.IsNullOrEmpty()) return;

			foreach (ToolbarMod toolbarMod in _toolbarMods)
			{
				toolbarMod.Draw(hub);
			}
		}

		private static bool ShouldDrawToolbarMods(object[] drawTargets)
		{
			if (!drawTargets.IsValid())
			{
				return false;
			}

			foreach (object drawTarget in drawTargets)
			{
				if (drawTarget.GetType() == typeof(EmptyDraw))
				{
					return false;
				}
			}

			return true;
		}
	}
}