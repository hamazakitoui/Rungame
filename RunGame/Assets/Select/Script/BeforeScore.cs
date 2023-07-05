using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeforeScore : MonoBehaviour
{
    SaveData Data;
    [SerializeField] Sprite NotAchievement;
    [SerializeField] Sprite GetAchievement;
    [SerializeField] GameObject[] Achevementobject;
    [SerializeField] Text[] textobject;
    int nowCliar;
    const int MaxStage = 5;
    const int MaxAchievement = 3;
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.Load();
        Data = SaveManager.Instance.GetData;

        for (int Stage = 0; MaxStage > Stage; Stage++)
        {
            Achevementobject[Stage].SetActive(true);
            textobject[Stage].gameObject.SetActive(true);
            textobject[Stage].text = Data.GetStageScore(Stage).ToString();
            if (Data.GetStageClear(Stage))
            {
                
                nowCliar++;
                

                for (int Ahevement = 0; Ahevement <= MaxAchievement; Ahevement++)
                {
                    bool checkAchevement = Data.GetAchievement(Stage, Ahevement);
                    Debug.Log(Data.GetAchievement(Stage,Ahevement));
                    if (checkAchevement)
                    {
                        Debug.Log("abc");
                        Achevementobject[Stage].transform.GetChild(Ahevement).GetComponent<Image>().sprite = GetAchievement;

                    }
                }
            }

        }

    }

}
