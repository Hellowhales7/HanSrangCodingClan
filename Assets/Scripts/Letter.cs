using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Letters
{
    public static string[] consonants = {
        "ぁ", "あ", "い", "ぇ", "え", "ぉ", "け", "げ", "こ", "さ", "ざ", "し", "じ", "す", "ず", "せ", "ぜ", "そ", "ぞ"
    };
    public static string[] vowels = {
        "た", "だ", "ち", "ぢ", "っ", "つ", "づ", "て", "で", "と", "ど", "な", "に", "ぬ", "ね", "の", "は", "ば", "ぱ", "ひ", "び"
    };
}


public class LetterData
{
    public string Type;
    public string[] Data = new string[10];

    public LetterData()
    {
        Type = "Letter";

        for(int i=0;i<7;i++)
        {
            int rng = Random.Range(0, 19);
            Data[i] = Letters.consonants[rng];
        }
        for(int i=7;i<10;i++)
        {
            int rng = Random.Range(0, 21);
            Data[i] = Letters.vowels[rng];
        }
    }
}