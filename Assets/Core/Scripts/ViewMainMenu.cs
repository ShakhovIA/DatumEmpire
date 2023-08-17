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
        private ContainerViewMainMenu SelectedContainer { get; set; }

        #endregion
        
        #region [Functions]
        
        public void SetResolution() => SelectedContainer = Containers.Find(x => x.TypeResolution == ManagerData.TypeResolution);

        public void Initialization()
        {
            
        }

        public void Open()
        {
            
        }
        
        public void Close()
        {
            
        }
        
        #endregion
    }
}
