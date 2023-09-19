using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class WidgetButtonOk : MonoBehaviour, IWidget
    {
        #region [Injections]

        [Inject] private ManagerSound ManagerSound { get; set; }
        [Inject] private ViewMainMenu ViewMainMenu { get; set; }
        [Inject] private ViewSearchMatch ViewSearchMatch { get; set; }
        [Inject] private ViewTotalizator ViewTotalizator { get; set; }
        [Inject] private WidgetsMainMenu WidgetsMainMenu { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private Image Icon { get; set; }
        private Sequence sequenceClickAnim;

        #endregion

        #region [Functions]

        public void Initialization()
        {
            
        }

        public void Click()
        {
            if (sequenceClickAnim != null && sequenceClickAnim.active)
            {
                sequenceClickAnim.Restart();
                return;
            }

            Vector3 initialPos = transform.position;
            sequenceClickAnim = DOTween.Sequence();
            sequenceClickAnim
                .Append(transform.DOShakePosition(0.5f, new Vector3(20f, 20f)))
                .Join(transform.DOPunchScale(new Vector2(0.2f, 0.2f), 0.5f))
                .OnComplete(() =>
                {
                    transform.localScale = Vector3.one;
                    transform.position = initialPos;
                });

            StartCoroutine(RoutineOpen());
        }

        public IEnumerator RoutineOpen()
        {
            ManagerSound.PlayEffect(ManagerSound.AudioButtonClick);
            yield return new WaitForSecondsRealtime(0.2f);
            ViewTotalizator.Close();
            WidgetsMainMenu.Close();
            ViewSearchMatch.Open();
        }

        public void Refresh()
        {
            
        }

        #endregion
    }
}