using Core.Scripts;
using UnityEngine;

public class ContainerViewGame : MonoBehaviour
{
    #region [Fields]
    [field: SerializeField] public EnumResolution TypeResolution { get; private set; }
    [field: SerializeField] public RectTransform MarkerWidgetHeroPanel { get; private set; }

    #endregion
}
