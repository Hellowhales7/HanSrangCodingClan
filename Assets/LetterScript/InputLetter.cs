using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputLetter : MonoBehaviour
{
    public TMP_Text WordInput;
    private string Word = null;

    private void Awake()
    {
        Word = WordInput.text;
    }

    private void Update()
    {
        //Ű����
        if (Word.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            InputName();
        }
    }

    //���콺
    public void InputName()
    {
        Word = WordInput.text;
        Debug.Log((char)KoreanCharacterUtils.GetChosung(Word[0]));
        Debug.Log((int)'��');
    }
}
