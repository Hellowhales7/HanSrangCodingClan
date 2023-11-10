using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputLetter : MonoBehaviour
{

    public static string[] KOR_CHOSUNG_LIST = new string[] { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

    public static string[] KOR_JUNGSUNG_LIST = new string[] { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

    public static string[] KOR_JONGSUNG_LIST = new string[] { "", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };


    public TMP_Text WordInput;
    private string Word = null;
    Dictionary<string, int> MyWord = new Dictionary<string, int>();
    private void Awake()
    {
        Word = WordInput.text;
        string[] chosung = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
        foreach (string ch in chosung)
        {
            MyWord[ch] = 0;
        }

        // �ѱ� ���� �߰�
        string[] jungsung = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
        foreach (string jv in jungsung)
        {
            MyWord[jv] = 0;
        }
    }

    private void Update()
    {
        //Ű����
        if (Word.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            InputName();
            Verifier();
            WordInput.ClearMesh();
        }
    }

    //���콺
    public void InputName()
    {
        Word = WordInput.text;
       // Debug.Log(KOR_CHOSUNG_LIST[KoreanCharacterUtils.GetChosung(Word[0])]);
    }
    bool Verifier()
    {
        string[] All = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
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
