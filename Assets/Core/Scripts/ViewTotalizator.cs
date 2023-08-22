using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public class ViewTotalizator : MonoBehaviour, IView
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private List<ContainerViewTotalizator> Containers { get; set; }
        public ContainerViewTotalizator SelectedContainer { get; private set; }

        #endregion
        
        #region [Functions]
        
        public void SetResolution() => SelectedContainer = Containers.Find(x => x.TypeResolution == ManagerData.TypeResolution);

        public void Initialization()
        {
            // SelectedContainer.WidgetButtonHero.Initialization(this);
            // SelectedContainer.WidgetButtonCoins.Initialization(this);
            // SelectedContainer.WidgetButtonLimbs.Initialization(this);
            SetDefault();
            SelectedContainer.gameObject.SetActive(true);
            // SelectedContainer.WidgetButtonCoins.transform.DOPunchPosition(new Vector3(0f, 2000f, 0f), 1f, 1);
            // SelectedContainer.WidgetButtonLimbs.transform.DOPunchPosition(new Vector3(0f, 2000f, 0f), 1f, 1);
            // SelectedContainer.WidgetButtonHero.transform.DOPunchPosition(new Vector3(0f, 2000f, 0f), 1f, 1);
            // SelectedContainer.Background.transform.DOPunchPosition(new Vector3(0f, 3000f, 0f), 1f, 2);
        }

        public void Open()
        {
            Initialization();
        }
        
        public void Close()
        {
            SelectedContainer.gameObject.SetActive(false);
        }

        public void OpenCoins()
        {
            SelectedContainer.WidgetButtonHero.Deselect();
            SelectedContainer.WidgetButtonCoins.Select();
            SelectedContainer.WidgetButtonLimbs.Deselect();
            SelectedContainer.WidgetCoinsPanel.Open();
            //SelectedContainer.WidgetCoinsPanel.Open();
        }
        
        public void OpenLimbs()
        {
            SelectedContainer.WidgetButtonHero.Deselect();
            SelectedContainer.WidgetButtonCoins.Deselect();
            SelectedContainer.WidgetButtonLimbs.Select();
            SelectedContainer.WidgetCoinsPanel.Close();
        }
        
        public void OpenHero()
        {
            SelectedContainer.WidgetButtonHero.Select();
            SelectedContainer.WidgetButtonCoins.Deselect();
            SelectedContainer.WidgetButtonLimbs.Deselect();
            SelectedContainer.WidgetCoinsPanel.Close();
        }

        private void SetDefault()
        {
            OpenCoins();
        }

        #endregion
    }
}
