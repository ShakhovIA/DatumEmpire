using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class ContainerWidgetsMainMenu : MonoBehaviour
    {
        #region [Fields]
        [field: SerializeField] public EnumResolution TypeResolution { get; private set; }
        [field: SerializeField] public List<WidgetCurrency> WidgetsCurrencies { get; private set; }
        [field: SerializeField] public WidgetOffers WidgetOffers { get; private set; }
        [field: SerializeField] public WidgetSettings WidgetSettings { get; private set; }
        [field: SerializeField] public WidgetBattlePass WidgetBattlePass { get; private set; }
        [field: SerializeField] public WidgetDailyQuest WidgetDailyQuest { get; private set; }
        [field: SerializeField] public WidgetLuckyTicket WidgetLuckyTicket { get; private set; }
        [field: SerializeField] public WidgetLuckyWheel WidgetLuckyWheel { get; private set; }
        [field: SerializeField] public WidgetSlotMachine WidgetSlotMachine { get; private set; }
        [field: SerializeField] public WidgetButtonPlay WidgetButtonPlay { get; private set; }

        #endregion
    }
}
