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
    [SerializeField]
    private float m_ActivatorGap = 30.0f;
    public static float ActivatorGap { get { return Inst.m_ActivatorGap; } }
    [SerializeField]
    private float m_DeActivatorGap = 15.0f;
    public static float DeActivatorGap { get { return Inst.m_DeActivatorGap; } }
    [SerializeField]
    private float m_TimeConsume = 1.0f;
    public static float TimeConsume { get { return Inst.m_TimeConsume; } }
    [SerializeField]
    private Sprite m_Cover;
    public static Sprite Cover
    {
        get
        {
            return Inst.m_Cover;
        }
    }


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
    public static void SpeedDown()
    {
        Inst.m_BlockData.BlockSpeed += 0.3f;
    }
    public static void SpawnCover()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject NewCover = new GameObject("Cover");
            SpriteRenderer NewSprite = NewCover.AddComponent<SpriteRenderer>();
            NewSprite.sprite = Cover;
            NewSprite.sortingOrder = 10;
            Vector3 Create = new Vector3();
            Create.x = UnityEngine.Random.Range(-5, 5);
            Create.y = UnityEngine.Random.Range(-10, 10);
            Create.z = 0;
            NewCover.transform.position = Create;
            NewCover.transform.localScale = new Vector3(UnityEngine.Random.Range(2, 8), UnityEngine.Random.Range(2, 8), 1);
            NewCover.AddComponent<Cover>();
        }
    }
}
