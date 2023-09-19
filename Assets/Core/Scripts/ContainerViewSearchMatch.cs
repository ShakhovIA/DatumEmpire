using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class ContainerViewSearchMatch : MonoBehaviour
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

        [field: SerializeField] public GameObject ObjEnemyStrips { get; private set; }
        [field: SerializeField] public RectTransform[] EnemyStrips { get; private set; }
        [field: SerializeField] public GameObject ObjFoundedEnemy { get; private set; }
        [field: SerializeField] public Image ImgFoundedEnemy { get; private set; }
        [field: SerializeField] public TMP_Text TmpFoundedEnemyLvl { get; private set; }
        [field: SerializeField] public TMP_Text TmpFoundedEnemyName { get; private set; }
        [field: SerializeField] public Button BtnPlay { get; private set; }
        [field: SerializeField] public Button BtnRetry { get; private set; }
        [field: SerializeField] public GameObject Versus { get; private set; }
        [field: SerializeField] public Transform[] MarkersWidgetHeroesPanel { get; private set; }

        #endregion
    }
}
