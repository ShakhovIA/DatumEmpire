using DG.Tweening;
using Injection;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Scripts
{
    public class WidgetButtonBack : MonoBehaviour, IWidget
    {
        #region [Injections]

        [Inject] private ManagerSound ManagerSound { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private UnityEvent OnClick { get; set; }
        private Sequence sequenceClickAnim;

        #endregion

        #region [Functions]
        public void Initialization()
        {
            
        }

        public void Click() => StartCoroutine(CoroutineClick());

        private IEnumerator CoroutineClick()
        {
            //ManagerSound.PlayEffect(ManagerSound.AudioButtonClick);

            if (sequenceClickAnim != null && sequenceClickAnim.active)
            {
                sequenceClickAnim.Restart();
                OnClick?.Invoke();
                yield break;
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

            yield return new WaitForSecondsRealtime(.4f);

            OnClick?.Invoke();
        }

        public void Refresh()
        {
            
        }

        #endregion
    }
}