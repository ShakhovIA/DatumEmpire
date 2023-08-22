using System.Collections.Generic;
using Injection;
using TMPro;
using UnityEngine;

namespace Core.Scripts
{
    public class WidgetLimbsPanel : MonoBehaviour
    {
        #region [Injections]

        [Inject] private ViewTotalizator ViewTotalizator { get; set; }
        [Inject] private ManagerAccount ManagerAccount { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] public List<WidgetButtonCoinMultiplier> WidgetButtonCoinMultipliers { get; private set; }
        [field: SerializeField] private TMP_Text CoinText { get;  set; }

        #endregion
        
        #region [Functions]
        
        private void Initialization()
        {
            SetX(1);
        }

        public void SetX(int multiplier)
        {
            WidgetButtonCoinMultipliers.ForEach(x =>
            {
                if (x.Multiplier == multiplier)
                {
                    x.Select();
                    CoinText.text = (1000 * multiplier).ToString();
                    ManagerAccount.DataBet = new DataBet() { TypeBet = EnumBet.Coin, Count = 1000 * multiplier };
                }
                else
                {
                    x.Deselect();
                }
            });
        }

        public void Open()
        {
            gameObject.SetActive(true);
            Initialization();
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}