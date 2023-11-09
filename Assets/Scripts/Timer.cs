using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private static Timer Inst;
    static bool b_Timer = false;
    public TextMeshProUGUI Minute, Seconds, Score;
    [SerializeField]
    private float m_time = 180.0f;
    public static float time { get { return Inst.m_time; } set { Inst.m_time = value; } }
    private float realtime;

    private void Start()
    {
        Inst = this;
        realtime = m_time - 0.1f;
        m_time = realtime;
    }

    public static void TimerON()
    {
        b_Timer = true;
    }
    public static void TimerOFF()
    {
        b_Timer = false;
        Inst.m_time = Inst.realtime;
    }
    public static void TimeComsume(float consume)
    {
        time -= consume;
    }
    public void SetTimer(int time)
    {
        TimerON();
    }
    public static bool TimeOver()
    {
        if (Inst.m_time <= 0f)
            return true;
        return false;
    }
    public void Update()
    {
        if (b_Timer)
        {
            m_time -= Time.deltaTime;
            Minute.text = ((int)time / 60).ToString();
            Seconds.text = ((int)time % 60).ToString();
        }
        Score.text = ((int)LogicValue.Score*100).ToString();
    }
    public static bool ActivatorCounter()
    {
        if (time % LogicValue.ActivatorGap < 0.5f)
            return true;
        return false;
    }
}
