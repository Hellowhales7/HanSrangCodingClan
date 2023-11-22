using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private bool SpeedUp;
    private bool ZeroClear;
    private bool Cover;
    private List<int> LeverList;

    public Stage(bool ZeroStage, bool Speed, bool bCover)
    {
        LeverList = new List<int>();

        SpeedUp = Speed;
        ZeroClear = ZeroStage;
        Cover = bCover;

        if (ZeroClear)
        {
            LeverList.Add(0);
        }

        if (SpeedUp)
        {
            LeverList.Add(1);
        }

        if (Cover)
        {
            LeverList.Add(2);
        }
    }

    public int SelectLever() // 들어간 레버별 숫자에서 랜덤으로 하나를 고른다.
    {
        int rand = Random.Range(0, LeverList.Count);
        return LeverList[rand];
    }
}