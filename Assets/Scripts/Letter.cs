// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public static class Letters
// {
//     public static string[] consonants = {
//         "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ"
//     };
//     public static string[] vowels = {
//         "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ"
//     };
// }
//
//
// public class LetterData
// {
//     public string Type;
//     public string[] Data = new string[10];
//
//     public LetterData()
//     {
//         Type = "Letter";
//
//         for(int i=0;i<7;i++)
//         {
//             int rng = Random.Range(0, 19);
//             Data[i] = Letters.consonants[rng];
//         }
//         for(int i=7;i<10;i++)
//         {
//             int rng = Random.Range(0, 21);
//             Data[i] = Letters.vowels[rng];
//         }
//     }
// }