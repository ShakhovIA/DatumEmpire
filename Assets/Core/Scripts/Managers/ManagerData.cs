using System.Collections.Generic;
using System.Linq;
using Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public sealed class ManagerData : MonoBehaviour
    {
        #region [Injections]
        
        [Inject] private ViewMainMenu ViewMainMenu { get; set; }
        [Inject] private WidgetsMainMenu WidgetsMainMenu { get; set; }
        
        #endregion
        
        
        #region [Fields]
        [field: SerializeField] public EnumResolution TypeResolution { get; set; }
        [field:SerializeField] public CanvasScaler CanvasScaleGame { get; private set; }
        private List<IView> IViews { get; set; } = new List<IView>();

        #endregion
        
        
        #region [Functions]
        

        public void Start()
        {
            // IViews = new List<IView>();
            // var tempViews = FindObjectsOfType<MonoBehaviour>().OfType<IView>();
            // foreach (var view in tempViews)
            // {
            //     IViews.Add(view);
            // }
        }
        
        public void AddIView(IView data)
        {
            IViews.Add(data);
        }

        public void SelectPlatformMobile()
        {
            TypeResolution = EnumResolution.Vertical;
            CanvasScaleGame.referenceResolution = new Vector2(1080, 1920);
            IViews.ForEach(x=>x.SetResolution());
        }

        public void SelectPlatformDesktop()
        {
            TypeResolution = EnumResolution.Horizontal;
            CanvasScaleGame.referenceResolution = new Vector2(1920, 1080);
            IViews.ForEach(x=>x.SetResolution());
        }

        public void GameStart()
        {
            IViews.ForEach(x=>x.Close());
            ViewMainMenu.Open();
            WidgetsMainMenu.Open();
        }
        
        
        
        #endregion
    }
}