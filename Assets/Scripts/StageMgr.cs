using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMgr : MonoBehaviour
{
    private static StageMgr Inst;
    private int ActivatorCount = 1;
    private int CurrentAcivator;
    [SerializeField]
    private int m_Stage = 0;
    public static int Stage { get { return Inst.m_Stage; } set { Inst.m_Stage = value; } }
    public List<Stage> StageList = new List<Stage>();
    private List<int> LeverList = new List<int>();

    private float ActivatorOnTime = 0;
    private bool ActivatorOnOff = true;
    private void Init()
    {
         ActivatorCount = 1;
         ActivatorOnTime = 0;
         ActivatorOnOff = true;
         LeverList.Clear();
    }
    void Awake()
    {
        Inst = this;
       // Stage = PlayerPrefs.GetInt("Stage"); //���������� �ҷ��´�
        Inst.StageList.Add(new Stage(true, false, false));
        Inst.StageList.Add(new Stage(false, true, false));
        Inst.StageList.Add(new Stage(false, false, true));
        DeActivator();
    }
    private void Update()
    {
        if (!ActivatorOnOff&&Timer.ActivatorCounter() && CurrentAcivator > 0)
        {
            ActivatorOnTime = Timer.time;
            Activator(StageList[Stage]);
            CurrentAcivator--;
            ActivatorOnOff = true;
        }
        if (ActivatorOnOff && ActivatorOnTime - Timer.time > LogicValue.ActivatorGap - LogicValue.DeActivatorGap)
        {
            DeActivator();
        }
    }
    public static void StageClear()
    {
        Inst.m_Stage += 1;
        LogicValue.ScoreReset();
        Timer.TimerOFF();
        SceneManager.LoadScene("GameOver");
        PlayerPrefs.SetInt("Stage", Stage); //���������� �����Ѵ�
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
            case 0:
                if (LogicValue.Score >= 0.1)
                {
                    ZeroClear();
                    LeverList.Add(0);
                }
                Debug.Log("ZeroClearOn");
                break;
            case 1:
                LogicValue.SpeedUp();
                LeverList.Add(1);
                Debug.Log("SpeedUpOn");
                break;
            case 2:
                LogicValue.SpawnCover();
                LeverList.Add(2);
                Debug.Log("SpawnCover");
                break;
            default:
               // Debug.Log(lever);
                Debug.LogWarning("No Lever");
                break;
        }
    }
    public void DeActivator()
    {
        Debug.Log("DcActivator");
        CurrentAcivator = ActivatorCount;
        for (int i = 0; i < LeverList.Count; i++)
        {
            switch (LeverList[i])
            {
                case 0:
                    break;
                case 1:
                    LogicValue.SpeedDown();
                    break;
            }
        }
        LeverList.Clear();
        ActivatorOnOff = false;
    }
}
