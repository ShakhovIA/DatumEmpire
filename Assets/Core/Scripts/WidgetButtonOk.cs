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

        #endregion
        
        #region [Fields]

        [field: SerializeField] private Image Icon { get; set; }
        private Vector3 StartPosition { get; set; }

        #endregion
        
        #region [Functions]
        
        public void Start()
        {
            StartPosition = transform.position;
        }


        public void Initialization()
        {
            
        }

        public void Click()
        {
            transform.position = StartPosition;
            transform.DOShakePosition(0.5f,new Vector3(20f,20f));
            
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector2(0.2f,0.2f),0.5f);
            StartCoroutine(RoutineOpen());
        }

        public IEnumerator RoutineOpen()
        {
            ManagerSound.PlayEffect(ManagerSound.AudioButtonClick);
            yield return new WaitForSecondsRealtime(0.2f);
            ViewSearchMatch.Open();
            ViewTotalizator.Close();
        }

        public void Refresh()
        {
            
        }

        #endregion
    }
}