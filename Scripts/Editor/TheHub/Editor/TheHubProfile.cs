using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OdinUtils.TheHub
{
    [CreateAssetMenu(
        menuName = Constants.BaseCreateAssetMenuPath + "Profile",
        order = Constants.CreateAssetMenuOrder)]
    public class TheHubProfile : ScriptableObject
    {
        [SerializeField]
        private string title;

        [SerializeField]
        private string subTitle;

        [SerializeField]
        private List<Module> modules;

        [Title("Appearance")]
        [ColorUsage(true, true)]
        [SerializeField]
        private Color _createNewItemColor = new Color(0.1f, 0.7f, 0.3f);

        [ColorUsage(true, true)]
        [SerializeField]
        private Color _deleteItemColor = Color.red;

        public string Title => title;

        public string SubTitle => subTitle;

        public List<Module> Modules => modules;

        public Color CreateNewItemColor => _createNewItemColor;

        public Color DeleteItemColor => _deleteItemColor;
    }
}