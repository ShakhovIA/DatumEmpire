using System.Collections;
using System.Collections.Generic;
using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public class ViewMainMenu : MonoBehaviour, IView
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private List<ContainerViewMainMenu> Containers { get; set; }
        public ContainerViewMainMenu SelectedContainer { get; private set; }

        #endregion
        
        #region [Functions]
        
        public void SetResolution() => SelectedContainer = Containers.Find(x => x.TypeResolution == ManagerData.TypeResolution);

        public void Initialization()
        {
            
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
