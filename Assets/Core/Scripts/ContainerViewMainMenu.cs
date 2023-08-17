using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class ContainerViewMainMenu : MonoBehaviour
    {
        #region [Fields]
        [field: SerializeField] public EnumResolution TypeResolution { get; private set; }
        [field: SerializeField] public Image ImageCharacter { get; private set; }
        [field: SerializeField] public GameObject ParentCharacter { get; private set; }
        
        #endregion
    }
}
