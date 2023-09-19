using Injection;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts
{
    public class ViewGame : MonoBehaviour, IView
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }

        #endregion

        #region [Fields]

        [field: SerializeField] private List<ContainerViewGame> Containers { get; set; }
        public ContainerViewGame SelectedContainer { get; private set; }

        #endregion

        #region [Functions]

        public void Close() => SelectedContainer.gameObject.SetActive(false);

        public void Initialization()
        {

        }

        public void Open()
        {
            SelectedContainer.gameObject.SetActive(true);
            Initialization();
        }

        public void SetResolution() => SelectedContainer = Containers.Find(x => x.TypeResolution == ManagerData.TypeResolution);

        #endregion
    }
}
