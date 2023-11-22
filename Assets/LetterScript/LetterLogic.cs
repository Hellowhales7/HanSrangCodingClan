using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterLogic : MonoBehaviour, IWebSocketReceiver
{
    public TMP_Text[] LetterUI;
    public TMP_Text ScoreUI;
    public static int count;
    
    // 한글 자음과 모음을 키로 하는 Dictionary 생성
    public static Dictionary<string, int> koreanDictionary = new ();
    public static int score;
    private string jsonString;
    
    private void Awake()
    {
        // 한글 자음 추가
        string[] chosung = { "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ" };
        foreach (string ch in chosung)
        {
            koreanDictionary[ch] = 0;
        }

        // 한글 모음 추가
        string[] jungsung = { "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ" };
        foreach (string jv in jungsung)
        {
            koreanDictionary[jv] = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //string jsonString = "{\"Type\":\"Letter\",\"Data\":[\"ㅅ\",\"ㅎ\",\"ㅋ\",\"ㅍ\",\"ㅌ\",\"ㅎ\",\"ㄲ\",\"ㅏ\",\"ㅗ\",\"ㅣ\"]}"; // JSON 문자열을 가져옵니다

        //// JSON 문자열을 LetterData 객체로 역직렬화
        //LetterData Data = JsonUtility.FromJson<LetterData>(jsonString);

        //for (int i = 0; i < Data.Data.Length; i++)
        //{
        //    if (count < 25)
        //    {
        //        koreanDictionary[Data.Data[i]]++;
        //        count++;
        //    }
        //}

        //Debug.Log(koreanDictionary["ㅅ"]);
    }

    // Update is called once per frame
    public void OnReceivePacket(WebSocketBaseData socketBaseData)
    {
        if (socketBaseData.type != "letter")
        {
            return;
        }
        
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.word_refill);

        if (socketBaseData is LetterData data)
        {
            foreach (var t in data.Data)
            {
                if (count >= 25)
                {
                    continue;
                }

                koreanDictionary[t]++;
                count++;
            }
        }

        var temp = 0;

        for (var i = 0; i < 25; i++)
        {
            LetterUI[i].text = null;
        }
        
        foreach (var t in InputLetter.KOR_All_LIST)
        {
            if (koreanDictionary[t] <= 0)
            {
                continue;
            }
            
            for (var k = koreanDictionary[t]; k > 0; k--)
            {
                LetterUI[temp].text = t;
                temp++;
            }
        }
        
        ScoreUI.text = score.ToString();
    }
}
