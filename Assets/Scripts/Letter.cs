using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Letters
{
    public static string[] consonants = {
        "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��"
    };
    public static string[] vowels = {
        "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��"
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