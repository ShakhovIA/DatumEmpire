using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class WidgetButtonBack : MonoBehaviour, IWidget
    {
        #region [Injections]

        [Inject] private ViewTotalizator ViewTotalizator { get; set; }
        [Inject] private ViewMainMenu ViewMainMenu { get; set; }
        [Inject] private WidgetsMainMenu WidgetsMainMenu { get; set; }
        [Inject] private ManagerSound ManagerSound { get; set; }

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
            transform.DOPunchPosition(new Vector3(-50f,0f),0.5f,1,0.2f);
            
            //transform.localScale = Vector3.one;
            //transform.DOPunchScale(new Vector2(0.2f,0.2f),0.5f);
            
            
            StartCoroutine(RoutineClose());
        }

        public IEnumerator RoutineClose()
        {
            ManagerSound.PlayEffect(ManagerSound.AudioButtonClick);
            yield return new WaitForSecondsRealtime(0.4f);
            ViewTotalizator.Close();
            ViewMainMenu.Open();
            WidgetsMainMenu.Open();
        }

        public void Refresh()
        {
            
        }

        #endregion
    }
}