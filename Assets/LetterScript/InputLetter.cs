using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class WordRequestData
{
    public string word;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}

[Serializable]
public class WordResponseData
{
    public string description;
    public bool isCorrect;
}

public class InputLetter : MonoBehaviour
{
    // private bool isFinished;
    // private bool isCorrect;

    public static string[] KOR_CHOSUNG_LIST = new string[] { "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ" };

    public static string[] KOR_JUNGSUNG_LIST = new string[] { "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ" };

    public static string[] KOR_JONGSUNG_LIST = new string[] { "", "ㄱ", "ㄲ", "ㄳ", "ㄴ", "ㄵ", "ㄶ", "ㄷ", "ㄹ", "ㄺ", "ㄻ", "ㄼ", "ㄽ", "ㄾ", "ㄿ", "ㅀ", "ㅁ", "ㅂ", "ㅄ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ" };


    public TMP_Text WordInput;
    private string Word = null;
    Dictionary<string, int> MyWord = new Dictionary<string, int>();
    private void Awake()
    {
        Word = WordInput.text;
        string[] chosung = { "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ" };
        foreach (string ch in chosung)
        {
            MyWord[ch] = 0;
        }

        // 한글 모음 추가
        string[] jungsung = { "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ" };
        foreach (string jv in jungsung)
        {
            MyWord[jv] = 0;
        }
    }

    private void Update()
    {
        //키보드
        if (Word.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            InputName();
            Verifier();
            WordInput.ClearMesh();
        }
    }

    //마우스
    public void InputName()
    {
        Word = WordInput.text;
       // Debug.Log(KOR_CHOSUNG_LIST[KoreanCharacterUtils.GetChosung(Word[0])]);
    }
    
    private bool Verifier()
    {
        // isFinished = false;
        // isCorrect = false;
        
        string[] All = { "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ", "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ" };

        for (int i=0;i<Word.Length-1;i++)
        {
            //Debug.Log(Word[i]);
            //Debug.Log(KOR_CHOSUNG_LIST[KoreanCharacterUtils.GetChosung(Word[i])]);
            //Debug.Log(KOR_JUNGSUNG_LIST[KoreanCharacterUtils.GetJungsung(Word[i])]);
            //Debug.Log(KOR_JONGSUNG_LIST[KoreanCharacterUtils.GetJongsung(Word[i])]);
            MyWord[KOR_CHOSUNG_LIST[KoreanCharacterUtils.GetChosung(Word[i])]]++;
            MyWord[KOR_JUNGSUNG_LIST[KoreanCharacterUtils.GetJungsung(Word[i])]]++;
            if(KOR_JONGSUNG_LIST[KoreanCharacterUtils.GetJongsung(Word[i])] != "")
                MyWord[KOR_JONGSUNG_LIST[KoreanCharacterUtils.GetJongsung(Word[i])]]++;
        }
        //for(int i=0;i<All.Length;i++)
        //{
        //    Debug.Log(All.Length);
        //    Debug.Log(LetterLogic.koreanDictionary[All[i]]);
        //}
        for (int i = 0; i < All.Length; i++)
        {
            if (LetterLogic.koreanDictionary[All[i]] - MyWord[All[i]] < 0)
            {
                NoLetter();
                return false;
            }
        }
        
        // 검증 로직
        // StartCoroutine(SendPost("http://13.209.164.126:8000/api/game/verify", JsonUtility.ToJson(new WordRequestData())));

        // StartCoroutine(SleepTime());

        // if (isCorrect == false)
        //     return false;
        
        // 형태소 분석
        for (int i = 0; i < All.Length; i++)
        {
            LetterLogic.koreanDictionary[All[i]] = LetterLogic.koreanDictionary[All[i]] - MyWord[All[i]];
            LetterLogic.count = LetterLogic.count - MyWord[All[i]];
            MyWord[All[i]] = 0;
        }

        LetterLogic.score += 100;
        return true;
    }
    
    private IEnumerator SendPost(string url, string json)
    {
        var request = UnityWebRequest.PostWwwForm(url, json);

        yield return request.SendWebRequest();
        
        // 에러 발생 시
        if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
            yield break;
        }
        
        var data = JsonUtility.FromJson<WordResponseData>(request.downloadHandler.text);

        // isCorrect = data.isCorrect;
        // isFinished = true;
    }

    private IEnumerator SleepTime()
    {
        yield return new WaitForSeconds(1000);
    }

    void NoLetter()
    {
        Debug.Log("No Letter");
    }
}
