using System;
using UnityEngine;

[Serializable]
public class WebSocketBaseData
{
    public string type;
    protected object jsonClass;

    public string ObjectToJson()
    {
        return JsonUtility.ToJson(jsonClass);
    }

    public T JsonToObject<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}

[Serializable]
public class RequestJoinData : WebSocketBaseData
{
    public string userType;

    public RequestJoinData(string userType = "tetris")
    {
        type = "join";
        jsonClass = this;
        this.userType = userType;
    }
}

[Serializable]
public class ResponseJoinData : WebSocketBaseData
{
    public string data;
}

[Serializable]
public class ResponseLeaveData : WebSocketBaseData
{
    public string data;
}

[Serializable]
public class RequestVerifyData : WebSocketBaseData
{
    public string word;

    public RequestVerifyData(string word)
    {
        type = "verify";
        jsonClass = this;
        this.word = word;
    }
}

[Serializable]
public class ResponseVerifyData : WebSocketBaseData
{
    public bool isCorrect;
    public string description;
}

[Serializable]
public class RequestEndData : WebSocketBaseData
{
    public int score;

    public RequestEndData(int score)
    {
        type = "end";
        jsonClass = this;
        this.score = score;
    }
}

[Serializable]
public class LetterData : WebSocketBaseData
{
    public string[] Data = new string[10];

    public LetterData()
    {
        type = "letter";
        jsonClass = this;

        for (var i = 0; i < 7; i++)
        {
            var rng = UnityEngine.Random.Range(0, 19);
            Data[i] = InputLetter.KOR_CHOSUNG_LIST[rng];
        }

        for (var i = 7; i < 10; i++)
        {
            var rng = UnityEngine.Random.Range(0, 21);
            Data[i] = InputLetter.KOR_JUNGSUNG_LIST[rng];
        }
    }
}