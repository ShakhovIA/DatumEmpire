using UnityEngine;
using System.Collections.Generic;
using GameAnalyticsSDK;
using Tool;

public class AnalyticParam
{
    public string key;
    public int value;

    public AnalyticParam(string key, int value)
    {
        this.key = key;
        this.value = value;
    }
}

public enum Resource {coins, skin}

public static class Analytics
{
    public static void Init(string gameKey, string gameSecret)
        => GameAnalytics.Initialize(gameKey, gameSecret);

    public static void Init() => GameAnalytics.Initialize();

    public static void Custom(string eventName)
    {
        ManagerLog.Log($"{eventName} [analytics]");
        GameAnalytics.NewDesignEvent(eventName);
    }

    public static void Custom(string eventName, AnalyticParam keyValue)
    {
        ManagerLog.Log($"{eventName} [analytics]");
        Dictionary<string, object> customEventParams = new Dictionary<string, object>();
        customEventParams.Add(keyValue.key, keyValue.value);

        GameAnalytics.NewDesignEvent(eventName, 0, customEventParams);
    }

    public static void Custom(string eventName, AnalyticParam keyValue1, AnalyticParam keyValue2)
    {
        ManagerLog.Log($"{eventName} [analytics]");
        Dictionary<string, object> customEventParams = new Dictionary<string, object>();
        customEventParams.Add(keyValue1.key, keyValue1.value);
        customEventParams.Add(keyValue2.key, keyValue2.value);

        GameAnalytics.NewDesignEvent(eventName, 0, customEventParams);
    }

    public static void LevelStart(int level)
        => LevelProgress(GAProgressionStatus.Start, level);
    public static void LevelFinish(int level, int stars = 0)
        => LevelProgress(GAProgressionStatus.Complete, level, stars);
    public static void LevelFail(int level, int stars = 0)
        => LevelProgress(GAProgressionStatus.Fail, level, stars);

    static void LevelProgress(GAProgressionStatus status, int level, int score = -1)
    {
        ManagerLog.Log($"Level_{level} {status} [analytics]");
        if (score == -1)
            GameAnalytics.NewProgressionEvent(status, "Levels", level.ToString());
        else
            GameAnalytics.NewProgressionEvent(status, "Levels", level.ToString(), score);
    }

    public static void CustomStart(int level, string type)
         => CustomProgress(GAProgressionStatus.Start, level, type);
    public static void CustomFinish(int level, string type, int stars = 0)
        => CustomProgress(GAProgressionStatus.Complete, level, type, stars);
    public static void CustomFail(int level, string type, int stars = 0)
        => CustomProgress(GAProgressionStatus.Fail, level, type, stars);

    static void CustomProgress(GAProgressionStatus status, int level, string type, int score = -1)
    {
        ManagerLog.Log($"{type} Level_{level} {status} [analytics]");
        if (score == -1)
            GameAnalytics.NewProgressionEvent(status, type, level.ToString());
        else
            GameAnalytics.NewProgressionEvent(status, type, level.ToString(), score);
    }

    public static void Ads(string platform, string placement, GAAdType type, int reward = 0)
    {
        if (platform == null || platform == "") platform = "web";
        GameAnalytics.NewAdEvent(GAAdAction.Show, type, platform, placement);
    }

    public static void GetResource(Resource type, int amount, string source)
    {
        ManagerLog.Log($"Get {type} [analytics]");
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "coins", amount, type.ToString(), source);
    }

    public static void GetResource(string type, int amount, string source)
    {
        ManagerLog.Log($"Get {type} [analytics]");
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "coins", amount, type, source);
    }

    public static void ConsumeResource(Resource type, int amount, string source)
    {
        ManagerLog.Log($"Consume {type} [analytics]");
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "coins", amount, type.ToString(), source);
    }


    public static void BuyForRealMoney(int price, string pack)
    {
        GameAnalytics.NewBusinessEvent("RUB", price, "pack", pack, "shop");
    }
}
