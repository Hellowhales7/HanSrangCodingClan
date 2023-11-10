using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LetterLogic : MonoBehaviour
{
    private static LetterLogic Inst;

    public TMP_Text[] LetterUI;
    public TMP_Text ScoreUI;
    public static int count=0;
    // �ѱ� ������ ������ Ű�� �ϴ� Dictionary ����
    public static Dictionary<string, int> koreanDictionary = new Dictionary<string, int>();
    public static int score = 0;
    private void Awake()
    {
        Inst = this;
        // �ѱ� ���� �߰�
        string[] chosung = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
        foreach (string ch in chosung)
        {
            koreanDictionary[ch] = 0;
        }

        // �ѱ� ���� �߰�
        string[] jungsung = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
        foreach (string jv in jungsung)
        {
            koreanDictionary[jv] = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        string jsonString = "{\"Type\":\"Letter\",\"Data\":[\"��\",\"��\",\"��\",\"��\",\"��\",\"��\",\"��\",\"��\",\"��\",\"��\"]}"; // JSON ���ڿ��� �����ɴϴ�

        // JSON ���ڿ��� LetterData ��ü�� ������ȭ
        LetterData Data = JsonUtility.FromJson<LetterData>(jsonString);

        for (int i = 0; i < Data.Data.Length; i++)
        {
            if (count < 25)
            {
                koreanDictionary[Data.Data[i]]++;
                count++;
            }
        }

        Debug.Log(koreanDictionary["��"]);
    }

    // Update is called once per frame
    void Update()
    {
        string[] All = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };
        int temp = 0;

        for(int i =0; i<25;i++)
        {
            LetterUI[i].text = null;
        }
        for (int j = 0; j < All.Length; j++)
        {
            if (koreanDictionary[All[j]] >0)
            {
                for (int k = koreanDictionary[All[j]]; k > 0; k--)
                {
                    LetterUI[temp].text = All[j];
                    temp++;
                }
            }
        }
        temp = 0;
        ScoreUI.text = score.ToString();
    }
}
