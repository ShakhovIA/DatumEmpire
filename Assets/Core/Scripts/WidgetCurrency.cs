using System;
using System.Collections.Generic;
using DG.Tweening;
using Injection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class WidgetCurrency : MonoBehaviour, IWidget
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private TMP_Text TextCount { get; set; }
        [field: SerializeField] private EnumCurrency TypeCurrency { get; set; }
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
        }

        public void Refresh()
        {
            
        }

        #endregion
    }
}