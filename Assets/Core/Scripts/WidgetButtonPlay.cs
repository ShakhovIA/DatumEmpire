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
            transform.DOPunchScale(new Vector2(0.2f,0.2f),0.4f);

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