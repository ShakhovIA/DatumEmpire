using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class ContainerViewTotalizator : MonoBehaviour
    {
        #region [Fields]
        [field: SerializeField] public EnumResolution TypeResolution { get; private set; }
        [field: SerializeField] public Image Background { get; private set; }
        [field: SerializeField] public GameObject ParentCharacter { get; private set; }
        
        [field: SerializeField] public WidgetButtonCoins WidgetButtonCoins { get; private set; }
        [field: SerializeField] public WidgetButtonLimbs WidgetButtonLimbs { get; private set; }
        [field: SerializeField] public WidgetButtonHero WidgetButtonHero { get; private set; }
        [field: SerializeField] public WidgetButtonOk WidgetButtonOk { get; private set; }
        [field: SerializeField] public WidgetButtonBack WidgetButtonBack { get; private set; }
        [field: SerializeField] public WidgetCoinsPanel WidgetCoinsPanel { get; private set; }
        
        #endregion
    }
}
