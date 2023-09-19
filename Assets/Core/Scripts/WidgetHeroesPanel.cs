using System.Collections.Generic;
using Injection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class WidgetHeroesPanel : MonoBehaviour
    {
        #region [Injections]

        [Inject] private ViewTotalizator ViewTotalizator { get; set; }
        [Inject] private ManagerAccount ManagerAccount { get; set; }

        #endregion
        
        #region [Fields]
        public Sprite Sprite { set => imgHero.sprite = value; }

        [field: SerializeField] public List<WidgetButtonCoinMultiplier> WidgetButtonCoinMultipliers { get; private set; }
        [field: SerializeField] private TMP_Text CoinText { get;  set; }
        [field: SerializeField] private Image imgHero { get;  set; }
        [field: SerializeField] private Slider sliderExperience { get;  set; }
        [field: SerializeField] private TMP_Text tmpExperience { get;  set; }
        [field: SerializeField] private TMP_Text tmpNickname { get;  set; }
        [field: SerializeField] private TMP_Text tmpLvl { get;  set; }
        [field: SerializeField] private GameObject characterPart { get;  set; }

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

        public void ShowMenuWidgets()
        {
            SetActiveExperience(true);
            SetActiveCharacterPart(false);
            SetActiveNickname(false);
            SetActiveLvl(false);
        }

        private void SetExperience(float value)
        {
            sliderExperience.value = Mathf.Clamp01(0.2f);
            tmpExperience.text = "200/9999";
        }

        private void SetActiveExperience(bool value) => sliderExperience.gameObject.SetActive(value);

        private void SetActiveCharacterPart(bool value) => characterPart.SetActive(value);

        private void SetActiveNickname(bool value) => tmpNickname.gameObject.SetActive(value);

        private void SetActiveLvl(bool value) => tmpLvl.gameObject.SetActive(value);

        #endregion
    }
}