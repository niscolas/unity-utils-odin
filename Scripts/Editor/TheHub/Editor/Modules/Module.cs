using System;
using System.Collections.Generic;
using niscolas.UnityExtensions;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace OdinUtils.TheHub
{
	public abstract class Module : ScriptableObject
	{
		[SerializeField]
		private string _title;

		[SerializeField]
		private Texture _icon;

		public abstract Type SubmoduleType { get; }
		public string Title => _title;

		public Texture Icon => _icon;

		public abstract IEnumerable<Submodule> Submodules { get; }

		public abstract void AddSubmodule(Submodule submodule);
		public abstract void RemoveSubmodule(Submodule submodule);

		public virtual SubmoduleMenuItems DrawTree(IHub hub)
		{
			throw new NotImplementedException();
		}
	}

	public abstract class Module<T> : Module where T : Submodule
	{
		[SerializeField]
		private List<T> _submodules;

		public override IEnumerable<Submodule> Submodules => _submodules;
		public override Type SubmoduleType => typeof(T);

		public override void AddSubmodule(Submodule submodule)
		{
			_submodules.Add(submodule as T);
		}

		public override void RemoveSubmodule(Submodule submodule)
		{
			_submodules.Remove(submodule as T);
		}

		public override SubmoduleMenuItems DrawTree(IHub hub)
		{
			SubmoduleMenuItems submoduleMenuItems = new SubmoduleMenuItems();

			foreach (T submodule in _submodules)
			{
				IEnumerable<OdinMenuItem> menuItems = DrawSubmodule(hub, submodule);
				submoduleMenuItems.AddManyKeys(menuItems, submodule);
			}

			return submoduleMenuItems;
		}

		protected virtual IEnumerable<OdinMenuItem> DrawSubmodule(IHub hub, T submodule)
		{
			if (!submodule)
			{
				return default;
			}

			return submodule.DrawMenuTree(hub, this);
		}
	}
}