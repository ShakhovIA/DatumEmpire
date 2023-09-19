using DG.Tweening;
using Injection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Scripts
{
    public class ViewSearchMatch : MonoBehaviour, IView
    {
        #region [Injections]

        [Inject] private ManagerData ManagerData { get; set; }
        [Inject] private ViewGame ViewGame { get; set; }
        [Inject] private InjectionInstantiator InjectionInstantiator { get; set; }
        [Inject] private ViewTotalizator ViewTotalizator { get; set; }
        [Inject] private ManagerSound ManagerSound { get; set; }

        #endregion
        
        #region [Fields]

        [field: SerializeField] private List<ContainerViewSearchMatch> Containers { get; set; }
        [field: SerializeField] public Sprite SpriteTestFoundedEnemy { get; private set; }
        public ContainerViewSearchMatch SelectedContainer { get; private set; }

        private Vector3[] enemyStripsInitialPos;
        private bool isCoroutineMoveStripEnemiesWorking;
        private Coroutine coroutineMoveStripEnemies;

        #endregion

        #region [Functions]

        public void SetResolution() => SelectedContainer = Containers.Find(x => x.TypeResolution == ManagerData.TypeResolution);

        public void Initialization()
        {
            SelectedContainer.gameObject.SetActive(true);
            coroutineMoveStripEnemies = StartCoroutine(MoveStripEnemies());

            SelectedContainer.ObjFoundedEnemy.SetActive(false);
            SelectedContainer.BtnPlay.gameObject.SetActive(false);
            SelectedContainer.BtnRetry.gameObject.SetActive(false);

            InjectionInstantiator.Instantiate(ManagerData.PrefabWidgetHeroesPanel, SelectedContainer.MarkersWidgetHeroesPanel[0]);
        }

        public void Open()
        {
            Initialization();
        }
        
        public void Close()
        {
            // удалить виджеты героев
            foreach (var marker in SelectedContainer.MarkersWidgetHeroesPanel)
            {
                for (int i = 0; i < marker.childCount; i++)
                {
                    Destroy(marker.GetChild(i).gameObject);
                }
            }

            SelectedContainer.ObjEnemyStrips.SetActive(true);
            SelectedContainer.ObjFoundedEnemy.SetActive(false);

            if(isCoroutineMoveStripEnemiesWorking)
            {
                StopCoroutine(coroutineMoveStripEnemies);

                for (int i = 0; i < SelectedContainer.EnemyStrips.Length; i++)
                    SelectedContainer.EnemyStrips[i].anchoredPosition = enemyStripsInitialPos[i];
            }

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

        public void OnPlay()
        {
            Close();
            ViewGame.Open();
        }

        public void OnRetry()
        {
            // списываем 500 монет и крутим заного
            SelectedContainer.BtnRetry.gameObject.SetActive(false);
            SelectedContainer.BtnPlay.gameObject.SetActive(false);

            Destroy(SelectedContainer.MarkersWidgetHeroesPanel[1].GetChild(0).gameObject);

            SelectedContainer.ObjFoundedEnemy.SetActive(false);
            SelectedContainer.ObjEnemyStrips.SetActive(true);

            for (int i = 0; i < SelectedContainer.EnemyStrips.Length; i++)
                SelectedContainer.EnemyStrips[i].anchoredPosition = enemyStripsInitialPos[i];

            coroutineMoveStripEnemies = StartCoroutine(MoveStripEnemies());
        }

        public void OnBack()
        {
            Close();
            ViewTotalizator.Open();
        }

        private void SetDefault()
        {
            OpenCoins();
        }

        private IEnumerator MoveStripEnemies()
        {
            isCoroutineMoveStripEnemiesWorking = true;

            float maxSpeed = 5000f;
            float intervalSpeed = 50f;
            float currentSpeed = 0;

            RectTransform[] enemyStrips = SelectedContainer.EnemyStrips;
            HorizontalLayoutGroup group = enemyStrips[0].GetComponent<HorizontalLayoutGroup>();

            float spacing = group.spacing;
            float widthStrip = enemyStrips[0].rect.width;
            Vector2 initialPos = enemyStrips[0].anchoredPosition;
            float endX = initialPos.x - widthStrip;

            Vector2 moveDirection = Vector3.zero;

            enemyStripsInitialPos = new Vector3[]
            {
                enemyStrips[0].anchoredPosition,
                enemyStrips[1].anchoredPosition
            };

            bool isCaseFindingEnemy = true;
            float testTimer = 2;
            bool isCaseMovementToCenter = false;
            float offsetDirection = 0;
            float tweenValueX = 0;

            // Заполняем спрайтами врагов виджеты в ленте
            List<Sprite> tempEnemySprites = new List<Sprite>(ManagerData.HeroSprites);
            foreach (var strip in enemyStrips)
            {
                foreach (WidgetStripEnemy widgetEnemy in strip.GetComponentsInChildren<WidgetStripEnemy>())
                {
                    int randomIndex = Random.Range(0, tempEnemySprites.Count);
                    widgetEnemy.SetEnemySprite(tempEnemySprites[randomIndex]);
                    tempEnemySprites.RemoveAt(randomIndex);
                }
            }

            WidgetStripEnemy findingEnemy = enemyStrips[0].GetChild(1).GetComponent<WidgetStripEnemy>();
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
                        }, 0, 1.5f).SetEase(Ease.OutBack);

                        findingEnemy.SetEnemySprite(SpriteTestFoundedEnemy);
                    }
                }
                else if (isCaseMovementToCenter)
                {
                    moveDirection = Vector2.left * offsetDirection;

                    if (tweenValueX == 0) break;
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

            yield return new WaitForSeconds(0.75f);
            SelectedContainer.ImgFoundedEnemy.sprite = SpriteTestFoundedEnemy;
            SelectedContainer.ObjEnemyStrips.SetActive(false);
            SelectedContainer.Versus.SetActive(false);
            SelectedContainer.ObjFoundedEnemy.SetActive(true);

            yield return new WaitForSeconds(1f);
            SelectedContainer.ObjFoundedEnemy.SetActive(false);
            SelectedContainer.BtnPlay.gameObject.SetActive(true);
            SelectedContainer.BtnRetry.gameObject.SetActive(true);
            WidgetHeroesPanel enemyWidgetPanel = InjectionInstantiator.Instantiate(ManagerData.PrefabWidgetHeroesPanel, SelectedContainer.MarkersWidgetHeroesPanel[1]);
            enemyWidgetPanel.Sprite = SpriteTestFoundedEnemy;

            isCoroutineMoveStripEnemiesWorking = false;
        }

        #endregion
    }
}
