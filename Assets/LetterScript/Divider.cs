using UnityEngine;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class KoreanCharacterUtils
{


    private const int BASE_CODE = 44032;
    private const int CHOSUNG = 588;
    private const int JUNGSUNG = 28;

    public static int GetCharCode(char word)
    {
        
        return (int)word - BASE_CODE;
    }

    public static int GetChosung(char word)
    {
        return GetCharCode(word) / CHOSUNG;
    }

    public static int GetJungsung(char word)
    {
        return (GetCharCode(word) - (GetChosung(word) * CHOSUNG)) / JUNGSUNG;
    }

    public static int GetJongsung(char word)
    {
        return GetCharCode(word) - (GetChosung(word) * CHOSUNG) - (GetJungsung(word) * JUNGSUNG);
    }

    public static int GetWithoutChosung(char word)
    {
        return (GetCharCode(word) % CHOSUNG) + BASE_CODE;
    }

    public static int GetWithoutJongsung(char word)
    {
        return (int)word - GetJongsung(word);
    }

    public static bool IsKorean(string word)
    {
        return Regex.IsMatch(word, @".*[°¡-ÆR]+.*");
    }

    public static bool IsKor(string word)
    {
        return Regex.IsMatch(word, @".*[¤¡-¤¾¤¿-¤Ó°¡-ÆR]+.*");
    }
}