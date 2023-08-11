using GameAnalyticsSDK;
using GamePush;
using System.Collections.Generic;
using Tool;
using UnityEngine;

namespace Core.Scripts
{
    public class ManagerAnalytic
    {
        private static string _platfrom;

        public static void Initialize(IEnumerable<GameAnalyticsPlatformData> gameAnalyticsPlatformDatas)
        {
            _platfrom = GP_Platform.Type();

            bool isInitialize = false;

            foreach (GameAnalyticsPlatformData data in gameAnalyticsPlatformDatas)
            {
                if (data.Platform == _platfrom)
                {
                    GameAnalytics.Initialize(data.Key, data.Secret);

                    isInitialize = true;
                    break;
                }
            }

            if (isInitialize)
               ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> ANALYTICS INITIALIZED");
        }

        #region DESIGN
        public static void SendDesign(string eventName)
        {
            GameAnalytics.NewDesignEvent(eventName);
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Design EVENT: <color=yellow>{eventName}</color>");
        }
        public static void SendDesign(string eventName, int value)
        {
            GameAnalytics.NewDesignEvent(eventName, value);
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Design EVENT: <color=yellow>{eventName}</color> VALUE: <color=yellow>{value}</color>");
        }
        public static void SendDesign(string eventName, AnalyticParam keyValue1, AnalyticParam keyValue2)
        {
            var result = $"{eventName}:{keyValue1.Key}:{keyValue1.Value}:{keyValue2.Key}:{keyValue2.Value}";
            GameAnalytics.NewDesignEvent(result);
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Design EVENT: <color=yellow>{eventName}</color>");
        }
        public static void SendDesign(string eventName, AnalyticParam keyValue)
        {
            GameAnalytics.NewDesignEvent($"{eventName}:{keyValue.Key}:{ keyValue.Value}");
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Design EVENT: <color=yellow>{eventName}</color>");
        }
        public static void SendDesignWithCounting(string eventName)
        {
            var clickCount = PlayerPrefs.GetInt(eventName, 1);
            GameAnalytics.NewDesignEvent(eventName, clickCount);
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Design Counting EVENT: <color=yellow>{eventName}</color> CLICKS: <color=yellow>{clickCount}</color>");
            PlayerPrefs.SetInt(eventName, clickCount + 1);
        }
        #endregion

        #region PROGRESSION
        public static void SendProgression(GAProgressionStatus status, string progression01, string progression02, string progression03, int score = -1)
        {
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Progression STATUS: <color=yellow>{status}</color>");

            if (score == -1)
            {
                GameAnalytics.NewProgressionEvent(status, progression01, progression02, progression03);
                return;
            }

            GameAnalytics.NewProgressionEvent(status, progression01, progression02, progression03, score);
        }
        public static void SendProgression(GAProgressionStatus status, string progression01, string progression02, int score = -1)
        {
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Progression STATUS: <color=yellow>{status}</color>");

            if (score == -1)
            {
                GameAnalytics.NewProgressionEvent(status, progression01, progression02);
                return;
            }

            GameAnalytics.NewProgressionEvent(status, progression01, progression02, score);
        }
        public static void SendProgression(GAProgressionStatus status, string progression01, int score = -1)
        {
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Progression STATUS: <color=yellow>{status}</color>");

            if (score == -1)
            {
                GameAnalytics.NewProgressionEvent(status, progression01);
                return;
            }

            GameAnalytics.NewProgressionEvent(status, progression01, score);
        }

        #endregion

        #region ADVERTISING
        public static void SendAdsShow(string placement, GAAdType type)
        {
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Ads Show PLACEMENT: <color=yellow>{placement}</color> TYPE: <color=pink>{type}</color>");
            GameAnalytics.NewAdEvent(GAAdAction.Show, type, _platfrom, placement);
        }
        public static void SendAdsFailedShow(string placement, GAAdType type)
        {
            ManagerLog.Log($"<color=red>[{nameof(ManagerAnalytic)}]</color> Send Ads Failed Show <color=yellow>{placement}</color> TYPE: <color=pink>{type}</color>");
            GameAnalytics.NewAdEvent(GAAdAction.FailedShow, type, _platfrom, placement);
        }
        public static void SendAdsClicked(string placement, GAAdType type)
        {
            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Ads Clicked <color=yellow>{placement}</color> TYPE: <color=pink>{type}</color>");
            GameAnalytics.NewAdEvent(GAAdAction.Clicked, type, _platfrom, placement);
        }
        #endregion

        #region RESOURCE
        public static void SendSourceResource(GAResourceType type, int amount, GASourceResourcePlacementType placement, string currency = "coins")
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, currency, amount, type.ToString(), placement.ToString());

            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Source Resource " +
                $"TYPE: <color=yellow>{type}</color> " +
                $"AMOUNT: <color=yellow>{amount}</color>" +
                $"PLACEMENT: <color=yellow>{placement}</color>");
        }

        public static void SendSinkResource(GAResourceType type, int amount, GASinkResourcePlacementType placement, string currency = "coins")
        {
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, currency, amount, type.ToString(), placement.ToString());

            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Sink Resource " +
                $"TYPE: <color=yellow>{type}</color> " +
                $"AMOUNT: <color=yellow>{amount}</color>" +
                $"PLACEMENT: <color=yellow>{placement}</color>");
        }
        #endregion

        #region BUSINESS
        public static void SendBusiness(int price, string packType, string packId, string from = "shop")
        {
            var currency = "None";

            switch (_platfrom)
            {
                case Platform.YANDEX:
                    currency = "yan";
                    break;
                case Platform.VK:
                    currency = "vk";
                    break;
                case Platform.OK:
                    currency = "ok";
                    break;
                case Platform.VK_PLAY:
                    currency = "rub";
                    break;
            }

            GameAnalytics.NewBusinessEvent(currency, price, packType, packId, from);

            ManagerLog.Log($"<color=#04bc04>[{nameof(ManagerAnalytic)}]</color> Send Business " +
                $"CURRENCY: <color=yellow>{currency}</color> " +
                $"PRICE: <color=yellow>{price}</color>" +
                $"PACKTYPE: <color=yellow>{packType}</color>" +
                $"PACKID: <color=yellow>{packId}</color>" +
                $"FROM: <color=yellow>{from}</color>");
        }
        #endregion

        #region COMMON
        public static void SendJoinCommunity() => SendDesign("Guiclick:Community");
        public static void SendInviteFriend() => SendDesign("Guiclick:AddFriend");
        public static void SendOpenShop() => SendDesign("Gotoshop");
        public static void SendGameOpened() => SendDesignWithCounting("Gameopened");
        #endregion

        #region PUZZLE
        public static void SendLevelStart(GAPuzzleType type, int level, string choicePuzzle, int complexityId) =>
            SendProgression(GAProgressionStatus.Start, type.ToString(), choicePuzzle.ToString(), level.ToString(), complexityId);
        public static void SendLevelComplete(GAPuzzleType type, int level, string choicePuzzle, int complexityId) =>
            SendProgression(GAProgressionStatus.Complete, type.ToString(), choicePuzzle.ToString(), level.ToString(), complexityId);
        public static void SendCompletedPuzzleCount(int count) => SendDesign("Finish_puzzles", count);
        public static void SendPuzzleBoardClick() => SendDesignWithCounting("click_puzzleboard");
        public static void SendPuzzleViewClick() => SendDesignWithCounting("click_puzzleview");
        public static void SendClickLots(int id) => SendDesign("click_lots", id);
        public static void SendFreeSub() => SendDesign("free_sub");
        public static void SendBuySub() => SendDesign("buy_sub");
        public static void SendShareOnWall() => SendDesign("to_wall");
        public static void SendDailyReward(int day) => SendDesign("daily_reward", day);
        public static void SendAchivment(int id) => SendDesign("achivment", id);
        public static void SendDailyMission(int id) => SendDesign("daily_mission", id);
        #endregion
    }

    public struct AnalyticParam
    {
        public readonly string Key;
        public readonly int Value;

        public AnalyticParam(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
    public struct GameAnalyticsPlatformData
    {
        public string Platform;
        public string Key;
        public string Secret;
    }

    public enum GAPuzzleType { normal, everyday }
    public enum GAResourceType { clue, coins, flapper, firefly }
    public enum GASinkResourcePlacementType { open_puzzle, on_level, to_buy_buster }
    public enum GASourceResourcePlacementType { rewarded_adds, dailyreward, daily_missions, achievments, shop_offers, coins_spend, sub }
}