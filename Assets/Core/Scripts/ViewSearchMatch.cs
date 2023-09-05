using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePush;
using Injection;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Core.Scripts
{
    public class ViewSearchMatch : MonoBehaviour, IView
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private List<ContainerViewSearchMatch> Containers { get; set; }
        public ContainerViewSearchMatch SelectedContainer { get; private set; }

        #endregion
        
        #region [Functions]
        
        public void SetResolution() => SelectedContainer = Containers.Find(x => x.TypeResolution == ManagerData.TypeResolution);

        public void Initialization()
        {
            // SelectedContainer.WidgetButtonHero.Initialization(this);
            // SelectedContainer.WidgetButtonCoins.Initialization(this);
            // SelectedContainer.WidgetButtonLimbs.Initialization(this);
            //SetDefault();
            SelectedContainer.gameObject.SetActive(true);
            // SelectedContainer.WidgetButtonCoins.transform.DOPunchPosition(new Vector3(0f, 2000f, 0f), 1f, 1);
            // SelectedContainer.WidgetButtonLimbs.transform.DOPunchPosition(new Vector3(0f, 2000f, 0f), 1f, 1);
            // SelectedContainer.WidgetButtonHero.transform.DOPunchPosition(new Vector3(0f, 2000f, 0f), 1f, 1);
            // SelectedContainer.Background.transform.DOPunchPosition(new Vector3(0f, 3000f, 0f), 1f, 2);

            StartCoroutine(MoveStripEnemies());
        }

        public void Open()
        {
            Initialization();
        }
        
        public void Close()
        {
            SelectedContainer.gameObject.SetActive(false);
        }

        public void OpenCoins()
        {
            SelectedContainer.WidgetButtonHero.Deselect();
            SelectedContainer.WidgetButtonCoins.Select();
            SelectedContainer.WidgetButtonLimbs.Deselect();
            SelectedContainer.WidgetCoinsPanel.Open();
            //SelectedContainer.WidgetCoinsPanel.Open();
        }
        
        public void OpenLimbs()
        {
            SelectedContainer.WidgetButtonHero.Deselect();
            SelectedContainer.WidgetButtonCoins.Deselect();
            SelectedContainer.WidgetButtonLimbs.Select();
            SelectedContainer.WidgetCoinsPanel.Close();
        }
        
        public void OpenHero()
        {
            SelectedContainer.WidgetButtonHero.Select();
            SelectedContainer.WidgetButtonCoins.Deselect();
            SelectedContainer.WidgetButtonLimbs.Deselect();
            SelectedContainer.WidgetCoinsPanel.Close();
        }

        private void SetDefault()
        {
            OpenCoins();
        }

        private IEnumerator MoveStripEnemies()
        {
            float maxSpeed = 3000f;
            float intervalSpeed = 30f;
            float currentSpeed = 0;

            RectTransform[] enemyStrips = SelectedContainer.EnemyStrips;
            HorizontalLayoutGroup group = enemyStrips[0].GetComponent<HorizontalLayoutGroup>();

            float spacing = group.spacing;
            float widthStrip = enemyStrips[0].rect.width;
            Vector2 initialPos = enemyStrips[0].anchoredPosition;
            float endX = initialPos.x - widthStrip;

            Vector2 moveDirection = Vector3.zero;

            bool isCaseFindingEnemy = true;
            float testTimer = 4;
            bool isCaseMovementToCenter = false;
            float offsetDirection = 0;
            float tweenValueX = 0;
            while (true)
            {
                if (isCaseFindingEnemy)
                {
                    currentSpeed = Mathf.Clamp(currentSpeed += intervalSpeed, 0, maxSpeed);
                    moveDirection = Vector2.left * Mathf.Clamp(currentSpeed += intervalSpeed, 0, maxSpeed) * Time.deltaTime;

                    if ((testTimer -= Time.deltaTime) < 0 && enemyStrips[0].anchoredPosition.x > widthStrip - 100)
                    {
                        isCaseFindingEnemy = false;
                        isCaseMovementToCenter = true;

                        float startX = enemyStrips[0].anchoredPosition.x;
                        DOTween.To(() => enemyStrips[0].anchoredPosition.x, newX =>
                        {
                            if(enemyStrips[0].anchoredPosition.x - newX != startX)
                                offsetDirection = enemyStrips[0].anchoredPosition.x - newX;

                            tweenValueX = newX;
                        }, 0, 3).SetEase(Ease.OutBack);
                    }
                }
                else if (isCaseMovementToCenter)
                {
                    moveDirection = Vector2.left * offsetDirection;

                    if (tweenValueX == 0)
                    {
                        break;
                    }
                }

                for (int i = 0; i < enemyStrips.Length; i++)
                {
                    enemyStrips[i].anchoredPosition += moveDirection;

                    if (enemyStrips[i].anchoredPosition.x < endX)
                    {
                        int prevIndex = i - 1 >= 0 ? i - 1 : enemyStrips.Length - 1;
                        Vector2 prevStrip = enemyStrips[prevIndex].anchoredPosition;
                        enemyStrips[i].anchoredPosition = new Vector2(prevStrip.x + widthStrip + spacing, prevStrip.y);
                        break;
                    }
                }

                yield return null;
            }
        }

        #endregion
    }
}
