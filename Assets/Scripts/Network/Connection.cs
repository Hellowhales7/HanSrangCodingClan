using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using NativeWebSocket;

[Serializable]
public class UserData
{
    public string userType = "wordguess";
    
    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}

public class Connection : MonoBehaviour
{
    private WebSocket websocket;

    private void Awake()
    {
        int pm = Application.absoluteURL.IndexOf("?");
        
        var sceneName = pm != -1 ? Application.absoluteURL.Split("?"[0])[1] : "Default";
        
        Debug.Log($"[KHU] * sceneName : {sceneName}");
        
        // URLParameters.Instance.RegisterOnDone((url)=> {
        //     Debug.Log($"search parameters: {url.Search}");
        //     Debug.Log($"hash parameters: {url.Hash}");
        // });
        
        // string p = url.SearchParameters["myParameter"];
        //
        // Debug.Log($" * myParameter: {p}");
        
        double d = URLParameters.GetSearchParameters().GetDouble("keyName", 42d);
        
        Debug.Log($"[KHU] * keyName: {d}");
    }

    // Start is called before the first frame update
    private async UniTaskVoid Start()
    {
        // websocket = new WebSocket("ws://echo.websocket.org");
        // websocket = new WebSocket("ws://localhost:3000");
        websocket = new WebSocket("wss://jhseodev1.site/api/game/ws/:hnj");
        
        websocket.OnOpen += () =>
        {
            Debug.Log("[KHU] websocket.OnOpen - Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError($"[KHU] websocket.OnError - message: {e}");
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log($"[KHU] websocket.OnClose - Connection closed! - message: {e}");
        };

        websocket.OnMessage += (bytes) =>
        {
            // Reading a plain text message
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log($"[KHU] websocket.OnMessage - Received OnMessage! ({bytes.Length} bytes) {message}");
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("[KHU] SendWebSocketMessage", 0.0f, 0.3f);
        
        Debug.Log($"[KHW] asdf");

        await websocket.Connect();
        
        Debug.Log($"[KHW] asdf2");
        
        await websocket.SendText(new UserData().SaveToString());
        
        Debug.Log($"[KHW] asdf3");
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    private async UniTaskVoid SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("[KHU] plain text message");
        }
    }

    private async UniTaskVoid OnApplicationQuit()
    {
        await websocket.Close();
    }
}