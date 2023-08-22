using System.Collections;
using System.Collections.Generic;
using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public class WidgetsMainMenu : MonoBehaviour, IView
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private List<ContainerWidgetsMainMenu> Containers { get; set; }
        public ContainerWidgetsMainMenu SelectedContainer { get; private set; }

        #endregion
        
        #region [Functions]
        
        public void SetResolution() => SelectedContainer = Containers.Find(x => x.TypeResolution == ManagerData.TypeResolution);

        public void Initialization()
        {
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetOffers);
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetsCurrencies);
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetSettings);
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetBattlePass);
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetDailyQuest);
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetLuckyTicket);
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetLuckyWheel);
            // ManagerData.GamePipeline.BindComponent(SelectedContainer.WidgetButtonPlay);
        }

        public void Open()
        {
            SelectedContainer.gameObject.SetActive(true);
        }
        
        public void Close()
        {
            SelectedContainer.gameObject.SetActive(false);
        }
        
        #endregion
    }
}
