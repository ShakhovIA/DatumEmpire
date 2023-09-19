using System;
using System.Collections;
using DG.Tweening;
using Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class WidgetButtonPlay : MonoBehaviour, IWidget
    {
        #region [Injections]

        [Inject] private ViewTotalizator ViewTotalizator { get; set; }
        [Inject] private ViewMainMenu ViewMainMenu { get; set; }
        [Inject] private ManagerSound ManagerSound { get; set; }

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
            ViewTotalizator.Open();
            ViewMainMenu.Close();
        }

        public void Refresh()
        {
            
        }

        #endregion
    }
}