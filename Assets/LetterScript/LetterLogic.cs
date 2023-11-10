using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LetterLogic : MonoBehaviour
{
    private static LetterLogic Inst;

    public TMP_Text[] LetterUI;
    public TMP_Text ScoreUI;
    public static int count=0;
    // 한글 자음과 모음을 키로 하는 Dictionary 생성
    public static Dictionary<string, int> koreanDictionary = new Dictionary<string, int>();
    public static int score = 0;
    string jsonString;
    private void Awake()
    {
        Inst = this;
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
    void Update()
    {
        if(WebSocketDemo.Instance.wsQueue.Count >0)
        {
            jsonString =WebSocketDemo.Instance.wsQueue.Dequeue();

            LetterData Data = JsonUtility.FromJson<LetterData>(jsonString);

            for (int i = 0; i < Data.Data.Length; i++)
            {
                if (count < 25)
                {
                    koreanDictionary[Data.Data[i]]++;
                    count++;
                }
            }
        }
        string[] All = { "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ", "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ" };
        int temp = 0;

        for(int i =0; i<25;i++)
        {
            LetterUI[i].text = null;
        }
        for (int j = 0; j < All.Length; j++)
        {
            if (koreanDictionary[All[j]] >0)
            {
                for (int k = koreanDictionary[All[j]]; k > 0; k--)
                {
                    LetterUI[temp].text = All[j];
                    temp++;
                }
            }
        }
        temp = 0;
        ScoreUI.text = score.ToString();
    }
}
