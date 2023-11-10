using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputLetter : MonoBehaviour
{

    public static string[] KOR_CHOSUNG_LIST = new string[] { "ぁ", "あ", "い", "ぇ", "え", "ぉ", "け", "げ", "こ", "さ", "ざ", "し", "じ", "す", "ず", "せ", "ぜ", "そ", "ぞ" };

    public static string[] KOR_JUNGSUNG_LIST = new string[] { "た", "だ", "ち", "ぢ", "っ", "つ", "づ", "て", "で", "と", "ど", "な", "に", "ぬ", "ね", "の", "は", "ば", "ぱ", "ひ", "び" };

    public static string[] KOR_JONGSUNG_LIST = new string[] { "", "ぁ", "あ", "ぃ", "い", "ぅ", "う", "ぇ", "ぉ", "お", "か", "が", "き", "ぎ", "く", "ぐ", "け", "げ", "ご", "さ", "ざ", "し", "じ", "ず", "せ", "ぜ", "そ", "ぞ" };


    public TMP_Text WordInput;
    private string Word = null;
    Dictionary<string, int> MyWord = new Dictionary<string, int>();
    private void Awake()
    {
        Word = WordInput.text;
        string[] chosung = { "ぁ", "あ", "い", "ぇ", "え", "ぉ", "け", "げ", "こ", "さ", "ざ", "し", "じ", "す", "ず", "せ", "ぜ", "そ", "ぞ" };
        foreach (string ch in chosung)
        {
            MyWord[ch] = 0;
        }

        // 廃越 乞製 蓄亜
        string[] jungsung = { "た", "だ", "ち", "ぢ", "っ", "つ", "づ", "て", "で", "と", "ど", "な", "に", "ぬ", "ね", "の", "は", "ば", "ぱ", "ひ", "び" };
        foreach (string jv in jungsung)
        {
            MyWord[jv] = 0;
        }
    }

    private void Update()
    {
        //徹左球
        if (Word.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            InputName();
            Verifier();
            WordInput.ClearMesh();
        }
    }

    //原酔什
    public void InputName()
    {
        Word = WordInput.text;
       // Debug.Log(KOR_CHOSUNG_LIST[KoreanCharacterUtils.GetChosung(Word[0])]);
    }
    bool Verifier()
    {
        string[] All = { "ぁ", "あ", "い", "ぇ", "え", "ぉ", "け", "げ", "こ", "さ", "ざ", "し", "じ", "す", "ず", "せ", "ぜ", "そ", "ぞ", "た", "だ", "ち", "ぢ", "っ", "つ", "づ", "て", "で", "と", "ど", "な", "に", "ぬ", "ね", "の", "は", "ば", "ぱ", "ひ", "び" };
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
        for (int i = 0; i < All.Length; i++)
        {
            LetterLogic.koreanDictionary[All[i]] = LetterLogic.koreanDictionary[All[i]] - MyWord[All[i]];
            LetterLogic.count = LetterLogic.count - MyWord[All[i]];
            MyWord[All[i]] = 0;
        }
        LetterLogic.score += 100;
        return true;
    }
    void NoLetter()
    {
        Debug.Log("No Letter");
    }
}
