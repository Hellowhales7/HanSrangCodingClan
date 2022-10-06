using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BlockData
{
    public float BlockSpeed = 1.0f;
    public float BlockLockDelay = 0.5f;
}
public class LogicValue : MonoBehaviour
{
    private static LogicValue Inst;
   
    [SerializeField]
    BlockData m_BlockData = new BlockData();

    [SerializeField]
    private static float m_Score;
    public static float Score { get { return m_Score; } set { m_Score = value; } }
    [SerializeField]
    private float m_GetScore = 0.025f;

    public static float GetScore { get { return Inst.m_GetScore; } }
    private void Awake()
    {
        Inst = this;
    }

    public static void ScoreReset()
    {
        Score = 0;
    }

    public static float BlockSpeed
    {
        get { return Inst.m_BlockData.BlockSpeed - (m_Score*1.1f); }
    }
    public static float BlockLockDelay
    {
        get { return Inst.m_BlockData.BlockLockDelay; }
    }

    public static void SpeedUp()
    {
        Inst.m_BlockData.BlockSpeed -= 0.3f;
    }
}
