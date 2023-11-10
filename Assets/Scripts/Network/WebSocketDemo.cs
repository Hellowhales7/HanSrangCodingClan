using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Use plugin namespace
using HybridWebSocket;

[Serializable]
public class UserData
{
    // userType은 tetris 또는 wordguess
    public string userType = "tetris";

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}

// 출처 : https://usingsystem.tistory.com/202
public class WebSocketDemo : MonoBehaviour
{
    public static WebSocketDemo Instance { get; private set; }

    public WebSocket ws { get; private set; }
    public Queue<string> wsQueue = new Queue<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    private void Start()
    {
        // Create WebSocket instance
        // var ws = WebSocketFactory.CreateInstance("ws://echo.websocket.org");
        ws = WebSocketFactory.CreateInstance("ws://13.209.164.126:8000/api/game/ws/hnj");

        // Add OnOpen event listener
        ws.OnOpen += () =>
        {
            Debug.Log("WS connected!");
            Debug.Log($"WS state: {ws.GetState()}");

            // ws.Send(Encoding.UTF8.GetBytes("Hello from Unity 3D!"));
            SendGameLogic(new UserData().SaveToString());
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) =>
        {
            Debug.Log($"WS received message: {Encoding.UTF8.GetString(msg)}");
            wsQueue.Enqueue(Encoding.UTF8.GetString(msg));

            // ws.Close();
        };

        // Add OnError event listener
        ws.OnError += (string errMsg) => { Debug.Log($"WS error: {errMsg}"); };

        // Add OnClose event listener
        ws.OnClose += (WebSocketCloseCode code) => { Debug.Log($"WS closed with code: {code}"); };

        // Connect to the server
        ws.Connect();
    }

    // Update is called once per frame
    private void Update()
    {
        if (wsQueue.Count <= 0)
            return;
        
        //var str = wsQueue.Dequeue();
            
        //Debug.Log($"[KHU] * str: {str}");
            
        // txt.text = str;
        // txtbtn.text = str;
    }

    private void OnDestroy()
    {
        ws.Close();
    }

    public void SendGameLogic(string jsonData)
    {
        ws.Send(Encoding.UTF8.GetBytes(jsonData));
    }
}