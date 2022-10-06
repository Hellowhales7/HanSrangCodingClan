using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage
{
    public Stage(bool Speed)
    {
        SpeedUp = Speed;
        if (SpeedUp)
            LeverList.Add(1);
    }
    public bool SpeedUp;
    private List<int> LeverList;

    public int SelectLever()
    {
        int rand = Random.Range(0, LeverList.Count);
        return LeverList[rand];
    }
}

public class StageMgr : MonoBehaviour
{
    private static StageMgr Inst;
    [SerializeField]
    private int m_Stage = 0;
    public static int Stage { get { return Inst.m_Stage; } set { Inst.m_Stage = value; } }
   // public static Stage[] StageArr = { new Stage(false), new Stage(true) };
    void Awake()
    {
        Inst = this;
    }
    private void Update()
    {
        if (Stage == 0)
        {
            if (LogicValue.Score >= 0.1)
            {
                ZeroClear();
            }
        }
    }
    public static void StageClear()
    {
        Inst.m_Stage += 1;
        LogicValue.ScoreReset();
        Timer.TimerOFF();
        SceneManager.LoadScene("GameOver");
    }
    public void ZeroClear()
    {
        Inst.m_Stage += 1;
        LogicValue.ScoreReset();
        Timer.TimerOFF();
        SceneManager.LoadScene("0Clear");
    }
    public void Activator(Stage CurrentStage)
    {
        int lever = CurrentStage.SelectLever();
        switch (lever)
        {
            case 1:
                LogicValue.SpeedUp();
                break;
            default:
                Debug.LogWarning("No Lever");
                break;
        }
    }
}
