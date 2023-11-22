using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
// Use plugin namespace
using HybridWebSocket;

public interface IWebSocketReceiver
{
    void OnReceivePacket(WebSocketBaseData socketBaseData);
}

// 출처 : https://usingsystem.tistory.com/202
public class WebSocketManager : MonoBehaviour
{
    public static WebSocketManager Instance { get; private set; }

    private WebSocket ws { get; set; }
    private Queue<string> receivedPacketQueue = new();
    
    public List<IWebSocketReceiver> Receivers { get; private set; } = new ();
    
    private readonly WaitForSeconds _waitForSeconds = new (0.1f);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Create WebSocket instance
        // var ws = WebSocketFactory.CreateInstance("ws://echo.websocket.org");
        // ws = WebSocketFactory.CreateInstance("ws://54.180.46.156:8000/api/game/ws/hnj");
        ws = WebSocketFactory.CreateInstance("wss://jhseodev1.site:8443/api/game/ws/hnj");

        // Add OnOpen event listener
        ws.OnOpen += () =>
        {
            Debug.Log("WS connected!");
            Debug.Log($"WS state: {ws.GetState()}");

            // ws.Send(Encoding.UTF8.GetBytes("Hello from Unity 3D!"));
            SendGameLogic(new RequestJoinData("wordguess").ObjectToJson());
        };

        // Add OnMessage event listener
        ws.OnMessage += (msg) =>
        {
            // Debug.Log($"WS received message: {Encoding.UTF8.GetString(msg)}");
            receivedPacketQueue.Enqueue(Encoding.UTF8.GetString(msg));

            // ws.Close();
        };

        // Add OnError event listener
        ws.OnError += (errMsg) => { Debug.LogError($"WS error: {errMsg}"); };

        // Add OnClose event listener
        ws.OnClose += (code) => { Debug.Log($"WS closed with code: {code}"); };

        StartCoroutine(UpdateReceivedPacket());

        // Connect to the server
        ws.Connect();
    }

    private IEnumerator UpdateReceivedPacket()
    {
        while (true)
        {
            if (receivedPacketQueue.Count <= 0)
            {
                yield return _waitForSeconds;
                continue;
            }

            ReceiveGameLogic(receivedPacketQueue.Dequeue());
        }
    }

    private void OnDestroy()
    {
        ws.Close();
    }

    public void SendGameLogic(string jsonData)
    {
        ws.Send(Encoding.UTF8.GetBytes(jsonData));
    }

    private void ReceiveGameLogic(string jsonData)
    {
        var receivedData = JsonUtility.FromJson<WebSocketBaseData>(jsonData);
        
        Receivers.ForEach(receiver =>
        {
            receiver.OnReceivePacket(receivedData);
        });
    }
}