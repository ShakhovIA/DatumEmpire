using System;
using System.Collections;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using NativeWebSocket;
using Tool;

namespace Core.Scripts.Network
{ 
	public class NetworkManager : MonoBehaviour
	{
		public static NetworkManager Instance { get; private set; }
		public WebSocket Socket = new WebSocket("wss://router.fumydata.ru");

		private string urlConnection = "wss://router.fumydata.ru";
		//private string urlConnection = "wss://fumydata.ru/game/bear";

		private bool _isRoutinePingStarted = false;

		private void Awake()
		{
			Instance = this;

		}

		public void Start()
		{
			//Init();
		}

		public void Update()
		{
			// if (Input.GetKeyDown(KeyCode.A))
			// {
			// 	SchemaTestMessage testMessage = new();
			// 	testMessage.dataFloat = 13.5f;
			// 	testMessage.dataInt = 199;
			// 	testMessage.dataString = "tyt nasha stroka";
			// 	ManagerLog.Log("A KEY data start");
			// 	Send(EnumNetworkEvents.OnTestMessage, testMessage);
			// 	ManagerLog.Log("A KEY data end");
			// }
		}


		private void Init()
		{
			Socket = WebSocketFactory.CreateInstance(urlConnection);
			SubscribeToWebSocketEvents();
			Socket.Connect();
		}

		private void SubscribeToWebSocketEvents()
		{
			Socket.OnOpen += OnWebSocketOpen;
			Socket.OnError += OnWebSocketError;
			Socket.OnClose += OnWebSocketClose;
			Socket.OnMessage += OnWebSocketMessage;
		}

		private void OnWebSocketMessage(byte[] bytes)
		{
			//DataManager.Log("OnWebSocketMessage get");
			string serverMessage = Encoding.UTF8.GetString(bytes);
			//DataManager.Log(serverMessage);
			OnServerMessageEvent(serverMessage);
			//ServerMessageEvent?.Invoke(serverMessage);
		}

		private void OnWebSocketOpen()
		{
			ManagerLog.Log("Web Socket Connection Opened!");
		}

		private void OnWebSocketError(string error)
		{
			ManagerLog.Log($"Web Socket Error: {error}");
		}

		private void OnWebSocketClose(WebSocketCloseCode webSocketCloseCode)
		{
			ManagerLog.Log($"Web Socket Close: {webSocketCloseCode}");
		}

		private IEnumerator RoutinePing(float interval)
		{
			if (_isRoutinePingStarted)
			{
			}
			else
			{
				_isRoutinePingStarted = true;
				//DataManager.Log("Routine Ping Started");
				// while (true)
				// {
				// 	yield return new WaitForSecondsRealtime(interval);
				// 	MessagePing tempMessage = new MessagePing()
				// 	{
				// 		ControllerId = //DataManager.Instance.Account.ControllerId,
				// 			BattleId =  //DataManager.Instance.Room.BattleId
				// 	};
				// 	Send(EnumNetworkSendEvents.OnPing, tempMessage);
				// }
			}

			yield return new WaitForEndOfFrame();
		}

		public void Send(EnumNetworkEvents dataEvent, object dataObject)
		{
			ManagerLog.Log("MessageSendStart");
			Encoding enc = new UTF8Encoding(true, true);
			SchemaNetworkPackage tempPackage = new SchemaNetworkPackage()
			{
				action = Enum.GetName(typeof(EnumNetworkEvents), dataEvent),
				data = JsonUtility.ToJson(dataObject)
			};
			////DataManager.Log("message action =" + tempPackage.action);
			////DataManager.Log("message data =" + tempPackage.data);
			Socket.Send(enc.GetBytes(JsonUtility.ToJson(tempPackage)));
			////DataManager.Log("MessageSendDone");
			ManagerLog.Log("MessageSendEnd");
		}

		private void OnBattleConnect(string data)
		{
			ManagerLog.Log("1OnBattleConnect data пришла === " + data);
			ManagerLog.Log("2OnBattleConnect data пришла === " + JsonUtility.ToJson(data));
			//SchemaRoom tempRoom = JsonUtility.FromJson<SchemaRoom>(data);
			ManagerLog.Log("OnbattleEnd");
			//GraphicManager.Instance.OnBattleConnect();
			//запустить загрузку всех бандлов проекта
			// foreach (var shemaBundles in tempRoom.Bundles)
			// {
			// 	BundleManager.Instance.AddBundle(shemaBundles.Url, shemaBundles.Name);
			// }

			BundleManager.Instance.LoadBundles();

			ManagerLog.Log("Все бандлы добавленны");
			//DataManager.Instance.UpdateSchemaRoom(tempRoom);

			StartCoroutine(RoutinePing(0.5f));

		}

		private void OnBattleEndGame(string data)
		{
			//DataManager.Log("");
			ManagerLog.Log("OnBattleEndGame response");
			ManagerLog.Log("" + data);
			//SchemaEndGame tempSchemaEndGame = JsonUtility.FromJson<SchemaEndGame>(data);
			//Очистить все анные
			//Отправить сообщение в браузер о том что бы закрыть окно юнити
			//DataManager.Instance.QuitAndClose();
		}

		private void OnBattleData(string data)
		{
			//SchemaBattleData tempSchemaBattleData = JsonUtility.FromJson<SchemaBattleData>(data);
		}

		private void OnServerMessageEvent(string serverMessage)
		{
			ManagerLog.Log("Catch server event: " + serverMessage);
			//  SchemaSocketPackage serverPackage = TryGetSocketPackageModel(serverMessage);
			//
			//  EnumNetworkResponseEvents enumPackage = (EnumNetworkResponseEvents)System.Enum.Parse(typeof(EnumNetworkResponseEvents), serverPackage.action);
			// DataManager.Log("Event:" + enumPackage.ToString());
			// switch (enumPackage)
			// {
			// 	case EnumNetworkResponseEvents.OnAuthorization:{}
			// 		break;
			// 	case EnumNetworkResponseEvents.OnBattleConnect:
			// 	{
			// 		OnBattleConnect(serverPackage.data);
			// 	}
			// 		break;
			// 	case EnumNetworkResponseEvents.OnBattleNextTurn:
			// 	{
			// 		OnBattleNextTurn(serverPackage.data);
			// 	}
			// 		break;
			// 	case EnumNetworkResponseEvents.OnBattleEndTurn:
			// 	{
			// 		OnBattleEndTurn(serverPackage.data);
			// 	}
			// 		break;
			// 	case EnumNetworkResponseEvents.OnBattleEndGame:
			// 	{
			// 		OnBattleEndGame(serverPackage.data);
			// 	}
			// 		break;
			// 	case EnumNetworkResponseEvents.OnBattleData:
			// 	{
			// 		OnBattleData(serverPackage.data);
			// 	}
			// 		break;
			// }
		}




	}
}
