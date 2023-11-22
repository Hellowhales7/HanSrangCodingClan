using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InputLetter : MonoBehaviour, IWebSocketReceiver
{
    public static readonly string[] KOR_CHOSUNG_LIST =
        { "ㄱ", "ㄲ", "ㄴ", "ㄷ", "ㄸ", "ㄹ", "ㅁ", "ㅂ", "ㅃ", "ㅅ", "ㅆ", "ㅇ", "ㅈ", "ㅉ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ" };

    public static readonly string[] KOR_JUNGSUNG_LIST =
        { "ㅏ", "ㅐ", "ㅑ", "ㅒ", "ㅓ", "ㅔ", "ㅕ", "ㅖ", "ㅗ", "ㅘ", "ㅙ", "ㅚ", "ㅛ", "ㅜ", "ㅝ", "ㅞ", "ㅟ", "ㅠ", "ㅡ", "ㅢ", "ㅣ" };

    private static readonly string[] KOR_JONGSUNG_LIST =
    {
        "", "ㄱ", "ㄲ", "ㄳ", "ㄴ", "ㄵ", "ㄶ", "ㄷ", "ㄹ", "ㄺ", "ㄻ", "ㄼ", "ㄽ", "ㄾ", "ㄿ", "ㅀ", "ㅁ", "ㅂ", "ㅄ", "ㅅ", "ㅆ", "ㅇ",
        "ㅈ", "ㅊ", "ㅋ", "ㅌ", "ㅍ", "ㅎ"
    };

    public static readonly string[] KOR_All_LIST =
        KOR_CHOSUNG_LIST.Concat(KOR_JUNGSUNG_LIST).Concat(KOR_JONGSUNG_LIST).ToArray();

    [FormerlySerializedAs("WordInput")] public TMP_Text wordInput;

    private string _word;
    private readonly Dictionary<string, int> _myWord = new();

    private void Awake()
    {
        WebSocketManager.Instance.Receivers.Add(this);

        _word = wordInput.text;

        // 한글 자음 추가
        foreach (var ch in KOR_CHOSUNG_LIST)
        {
            _myWord[ch] = 0;
        }

        // 한글 모음 추가
        foreach (var jv in KOR_JUNGSUNG_LIST)
        {
            _myWord[jv] = 0;
        }
    }

    private void Update()
    {
        // 키보드
        if (_word.Length <= 0 || !Input.GetKeyDown(KeyCode.Return))
        {
            return;
        }

        InputName();
        Verifier();
        wordInput.ClearMesh();
    }

    // 마우스
    private void InputName()
    {
        _word = wordInput.text;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Verifier()
    {
        for (var i = 0; i < _word.Length - 1; i++)
        {
            // Debug.Log(_word[i]);
            // Debug.Log(KOR_CHOSUNG_LIST[KoreanCharacterUtils.GetChosung(_word[i])]);
            // Debug.Log(KOR_JUNGSUNG_LIST[KoreanCharacterUtils.GetJungsung(_word[i])]);
            // Debug.Log(KOR_JONGSUNG_LIST[KoreanCharacterUtils.GetJongsung(_word[i])]);
            _myWord[KOR_CHOSUNG_LIST[KoreanCharacterUtils.GetChosung(_word[i])]]++;
            _myWord[KOR_JUNGSUNG_LIST[KoreanCharacterUtils.GetJungsung(_word[i])]]++;

            if (KOR_JONGSUNG_LIST[KoreanCharacterUtils.GetJongsung(_word[i])] != "")
            {
                _myWord[KOR_JONGSUNG_LIST[KoreanCharacterUtils.GetJongsung(_word[i])]]++;
            }
        }

        // foreach (var t in KOR_All_LIST)
        // {
        //     Debug.Log(KOR_All_LIST.Length);
        //     Debug.Log(LetterLogic.koreanDictionary[t]);
        // }

        if (KOR_All_LIST.Any(t => LetterLogic.koreanDictionary[t] < _myWord[t]))
        {
            NoLetter();
            return;
        }

        // 검증 로직
        WebSocketManager.Instance.SendGameLogic(new RequestVerifyData(_word).ObjectToJson());
    }

    public void OnReceivePacket(WebSocketBaseData socketBaseData)
    {
        if (socketBaseData is not ResponseVerifyData data || socketBaseData.type != "verify")
        {
            return;
        }

        if (data is { isCorrect: true })
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Word_Correct);

            // 형태소 분석
            foreach (var t in KOR_All_LIST)
            {
                LetterLogic.koreanDictionary[t] -= _myWord[t];
                LetterLogic.count -= _myWord[t];
                _myWord[t] = 0;
            }

            LetterLogic.score += 100;

            Debug.Log(data.description);
        }
        else
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.word_wrong);
        }
    }

    private void NoLetter()
    {
        // Debug.Log("No Letter");
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Word_No_element);
    }
}