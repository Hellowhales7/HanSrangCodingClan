using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private static Timer Inst;
    static bool b_Timer = false;
    public Text Minute, Seconds;
    [SerializeField]
    private float m_time = 10 ;
    public static float time { get { return Inst.m_time; }  }
    private void Start()
    {
        Inst = this;
    }

    public static void TimerON()
    {
        b_Timer = true;
    }
    public static void TimerOFF()
    {
        b_Timer = false;
        Inst.m_time = 180;
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
    }

}
