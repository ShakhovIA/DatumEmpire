using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WheelFortune : MonoBehaviour
{
    private bool isSpin = false;
    private float timeSpin = 2f;
    private float defaultTimeSpin = 2f;
    private float defaultSpeedSpin = 3f;

    public WheelFortuneRewardData triggeringReward;

    public List<WheelFortuneRewardData> allRewards;
    public List<WheelFortuneRewardPrefabData> allPrefabs;

    public WinPanel winPanel;

    public Image wheel;

    public Image twist;
    public bool isTwist = false;
    public bool isTrigger;

    public Image selector;
    public GameObject wheelFortuneWindow;

    // Start is called before the first frame update
    void Start()
    {
        TwistInit();
        isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        wheelFortuneWindow.SetActive(true);
        TwistInit();
        isTrigger = false;
    }

    public void Close()
    {
        wheelFortuneWindow.SetActive(false);
    }

    public void OnEnable()
    {
        triggeringReward = null;
        foreach (var pref in allPrefabs)
        {
            pref.data = allRewards[Random.Range(0, allRewards.Count)];
            pref.icon.sprite = pref.data.icon;
            foreach (var reward in allRewards)
            {
                if (Random.Range(0, 100) < reward.reward.chanceDrop)
                {
                    pref.data = allRewards[Random.Range(0, allRewards.Count)];
                    pref.icon.sprite = pref.data.icon;
                    break;
                }
            }

            
                switch (pref.data.reward.type)
                {
                    case EnumWheelRewards.Diamonds:pref.text.text = "+" + pref.data.reward.value.ToString();
                        break;
                    case EnumWheelRewards.Kills:pref.text.text = "+" + pref.data.reward.value.ToString()+" Kills";
                        break;
                    case EnumWheelRewards.SpeedBuff:pref.text.text = "x" + pref.data.reward.value.ToString()+" "+pref.data.reward.duration/60+" min";
                        break;
                    case EnumWheelRewards.GoldBuff:pref.text.text = "x" + pref.data.reward.value.ToString()+" "+pref.data.reward.duration/60+" min";
                        break;
                }
        }
    }

    public void Spin()
    {
        if (!isSpin)
        {
            // if (ManagerData.instance.SpendDiamond(70))
            // {
            //     ManagerSound.instance.PlayEffect(ManagerSound.instance.WheelSpinButton, 3);
            //     OnEnable();
            //     selector.enabled = false;
            //     selector.DOFade(0, 0f);
            //     isSpin = true;
            //     isTrigger = false;
            //     triggeringReward = null;
            //     StartCoroutine(RoutineSpin());
            //     ManagerSound.instance.PlayEffect(ManagerSound.instance.WheelSpin, 1);
            // }
        }

    }

    public IEnumerator RoutineSpin()
    {
        yield return new WaitForEndOfFrame();
        TwistStart();
        timeSpin = defaultTimeSpin;

        float t_rotation_speed = 21 + Random.Range(-2f, 8f);

        while (timeSpin > 0)
        {
            timeSpin -= 0.7f * Time.deltaTime;

            if (timeSpin < 0.15f)
            {
                isTrigger = true;
                if (triggeringReward != null)
                {
                    timeSpin = 0;
                    break;
                }
                else
                {
                    timeSpin = 0.14f;
                }
            }

            if (timeSpin < 0.6f)
            {
                wheel.transform.Rotate((Vector3.back * t_rotation_speed) / (4f / timeSpin));
                TwistEnd();
            }
            else if (timeSpin < 1.5f)
            {
                wheel.transform.Rotate((Vector3.back * t_rotation_speed) / (1.2f / timeSpin));
            }
            else if (timeSpin < 2f)
            {
                wheel.transform.Rotate((Vector3.back * t_rotation_speed) / (0.8f / timeSpin));
            }

            yield return new WaitForEndOfFrame();

        }

        selector.enabled = true;
        selector.DOFade(1, 0.5f);
        yield return new WaitForSecondsRealtime(0.25f);
        triggeringReward.GetReward();
        //ManagerSound.instance.PlayEffect(ManagerSound.instance.WheelSpinReward, 4);
        isSpin = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger)
        {
            if (collision.tag.Equals("WheelRewardData"))
            {
                //Debug.Log("OKKK");
                //triggeringReward
                triggeringReward = collision.transform.parent.GetComponent<WheelFortuneRewardPrefabData>().data;
            }
        }
    }


    public void TwistInit()
    {
        selector.enabled = false;
        selector.DOFade(0, 0f);
        isTwist = false;
        twist.DOFade(0, 0f);
        twist.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(0, 0f);
        twist.transform.GetChild(1).gameObject.GetComponent<Image>().DOFade(0, 0f);
    }

    public void TwistStart()
    {
        if (!isTwist)
        {
            isTwist = true;
            twist.DOFade(1, 0.5f);
            twist.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(1, 0.5f);
            twist.transform.GetChild(1).gameObject.GetComponent<Image>().DOFade(1, 0.5f);
            wheel.GetComponent<RectTransform>().DOPunchScale(new Vector3(1.1f,1.1f,1.1f), 1.2F, 1, 1F);
        }
    }

    public void TwistEnd()
    {
        if (isTwist)
        {
            isTwist = false;
            twist.DOFade(0, 1f);
            twist.transform.GetChild(0).gameObject.GetComponent<Image>().DOFade(0, 1f);
            twist.transform.GetChild(1).gameObject.GetComponent<Image>().DOFade(0, 1f);
        }
    }

    [System.Serializable]
    public sealed class WinPanel
    {
        public GameObject window;
        public TextMeshProUGUI textMeshProUGUI;
        public Image icon;
    }
}

