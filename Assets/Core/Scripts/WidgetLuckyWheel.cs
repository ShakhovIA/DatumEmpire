using System;
using System.Collections.Generic;
using DG.Tweening;
using Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class WidgetLuckyWheel : MonoBehaviour, IWidget
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }

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
        }

        public void Refresh()
        {
            
        }

        #endregion
    }
}