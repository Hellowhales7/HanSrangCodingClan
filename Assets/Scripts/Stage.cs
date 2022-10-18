using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public Stage(bool ZeroStage, bool Speed, bool Cover)
    {
        SpeedUp = Speed;
        ZeroClear = ZeroStage;
        if (ZeroClear)
            LeverList.Add(0);
        if (SpeedUp)
            LeverList.Add(1);
    }
    private bool SpeedUp;
    private bool ZeroClear;
    private bool Cover;
    private List<int> LeverList = new List<int>();

    public int SelectLever() // 들어간 레버별 숫자에서 랜덤으로 하나를 고른다.
    {
        int rand = Random.Range(0, LeverList.Count);
        return LeverList[rand];
    }
}