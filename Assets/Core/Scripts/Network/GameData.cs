// using System;
// using System.Collections.Generic;
// using GameAnalyticsSDK;
// using UnityEngine;
// using GamePush;
// using Tool;
// using Core.Scripts.Networking;
//
// [DefaultExecutionOrder(-10)]
// public class GameData : MonoBehaviour
// {
//     //TODO DESCRIPTION: Ссылка на сервер для подключения
//     private string serverAddress = "https://router.fumydata.ru";
//
//     //TODO DESCRIPTION: Данные о игре, которые мы отправляем на сервер, для итендификации
//     private SendData sendData = new SendData { game = "bear", platform = "VK", creds = default };
//
//     private List<AnalyticsData> analyticsDatas = new List<AnalyticsData>()
//     {
//         new AnalyticsData(){platform = "VK",gameKey = "1142f9f95c18fa9fcd41bb8f9031a49d",gameSecret = "7b3440518c4bbe7fc05c38949891401f1d075928"},
//         new AnalyticsData(){platform = "OK",gameKey = "ea186cd116d2eb7509bddb8ea5c8ed5b",gameSecret = "129165f5aa0a9703b1c54e6e48bb7ccfcdc6e6a3"},
//         new AnalyticsData(){platform = "YANDEX",gameKey = "f7777ff0fe618f4d191f2414831e7117",gameSecret = "bd9305c7dbeba3b7edc9832cb2e6264c584a1d19"}
//     };
//
//     public void Awake()
//     {
//         Application.targetFrameRate = 60;
//         QualitySettings.vSyncCount = 1;
//
//         GetTime(); //Test server connection
//     }
//
//     private void OnEnable()
//     {
//         GP_SDK.OnReady += OnReady;
//         SetUpPlayer();
//     }
//
//     private void OnDisable()
//     {
//         GP_SDK.OnReady -= OnReady;
//     }
//
//     void OnReady()
//     {
//         ManagerLog.LogWarning("GamePush ready");
//
//         string platform = GP_Platform.Type();
//         InitAnalytics(platform);
//     }
//
//     void InitAnalytics(string platform)
//     {
//         ManagerLog.LogWarning($"Init analytics for platform {platform}");
//         foreach(AnalyticsData data in analyticsDatas)
//         {
//             if(data.platform == platform)
//             {
//                 ManagerLog.LogWarning("GameAnalitycs init start");
//                 Analytics.Init(data.gameKey,data.gameSecret);
//                 ManagerLog.LogWarning("GameAnalitycs init end");
//                 break;
//             }
//         }
//     }
//
//     void SetUpPlayer()
//     {
//         ManagerLog.Log($"Player init: {GP_Player.IsReady}");
//
//         SendServerData();
//     }
//
//     async void GetTime()
//     {
//         ManagerLog.Log("Get time from server");
//         var req = await WebMessage.Get($"{serverAddress}/time");
//         ManagerLog.Log("Get time answer = " + req);
//     }
//
//
//     async void SendServerData()
//     {
//         sendData.platform = GP_Platform.Type();
//         sendData.creds = GP_Player.GetString("credentials");
//
//         if (sendData.creds != "")
//         {
//             ManagerLog.Log("User creds: " + sendData.creds.ToString());
//             //TODO WARNING: Нужно достать данные из тела запроса на сервере иначе нужно отправлять данные в параметрах
//             //TODO WARNING: senddata пока не работает нужно разбираться с чанками
//             var req = await WebMessage.Post($"{serverAddress}/api/{sendData.platform}/{sendData.game}/{sendData.creds}", sendData);
//             //var req2 = await WebMessage.Post($"{serverAddress}/authorization", sendData); 
//             ManagerLog.Log("User creds req=" + req);
//         }
//     }
// }
//
// [Serializable]
// public struct AnalyticsData
// {
//     public string platform;
//     public string gameKey;
//     public string gameSecret;
// }
//
// [Serializable]
// public struct SendData
// {
//     public string game;
//     public string platform;
//     public string creds;
// }
//
//
//
//
//
