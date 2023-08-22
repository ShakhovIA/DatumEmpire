using System;
using System.Collections.Generic;
using DG.Tweening;
using Injection;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts
{
    public class WidgetButtonLimbs : MonoBehaviour, IWidget
    {
        #region [Injections]

        [Inject] private ViewTotalizator ViewTotalizator { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private Image Icon { get; set; }
        [field: SerializeField] private Sprite SpriteSelect { get; set; }
        [field: SerializeField] private Sprite SpriteDeSelect { get; set; }
        private Vector3 StartPosition { get; set; }

        #endregion
        
        #region [Functions]
        
        public void Start()
        {
            StartPosition = transform.position;
        }


        public void Initialization(ViewTotalizator totalizator)
        {
            ViewTotalizator = totalizator;
        }

        public void Click()
        {
            transform.position = StartPosition;
            transform.DOShakePosition(0.5f,new Vector3(20f,20f));
            
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector2(0.2f,0.2f),0.5f);
            
            ViewTotalizator.OpenLimbs();
        }

        public void Refresh()
        {
            
        }
        
        public void Select()
        {
            Icon.sprite = SpriteSelect;
        }
        
        public void Deselect()
        {
            Icon.sprite = SpriteDeSelect;
        }

        #endregion
    }
}