using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WheelFortuneReward", menuName = "ScriptableObjects/WheelFortuneReward", order = 5)]
public class WheelFortuneRewardData : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public Sprite icon;
    //[SerializeField] public Currency reward;
    public WheelReward reward;

    public void GetReward()
    {
        switch (reward.type)
        {
            case EnumWheelRewards.Kills:
            {
                //ManagerData.instance.AddGold(reward.value);
            }break;
            case EnumWheelRewards.Diamonds:
            {
                //ManagerData.instance.AddDiamond(reward.value);
            }break;
            case EnumWheelRewards.GoldBuff:
            {
                //ManagerData.instance.AddGoldBuff(reward.duration, reward.value);
            }break;
            case EnumWheelRewards.SpeedBuff:
            {
                //ManagerData.instance.AddSpeedBuff(reward.duration, reward.value);
            }break;
        }
    }
}

[System.Serializable]
public sealed class WheelReward
{
    public double value;
    public EnumWheelRewards type;
    public double duration;
    public double chanceDrop = 15;
}
[System.Serializable]
public enum EnumWheelRewards
{
    None,
    Kills,
    Diamonds,
    GoldBuff,
    SpeedBuff
}

