using System;
using System.Collections.Generic;
using niscolas.UnityExtensions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

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

        public virtual IReadOnlyList<OdinMenuItem> DrawMenuTree(IHub hub, Module parentModule)
        {
            return default;
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