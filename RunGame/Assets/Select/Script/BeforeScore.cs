using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeforeScore : MonoBehaviour
{
    SaveData Data;//セーブデータ
    [SerializeField] Sprite GetAchievement;//収集物入手
    [SerializeField] GameObject[] Achevementobject;//各ステージの収集物
    [SerializeField] Text[] textobject;//スコア
    int nowCliar;
    const int MaxStage = 4;
    const int MaxAchievement = 3;
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.Load();
        Data = SaveManager.Instance.GetData;

        //クリアしたステージ数チェック
        for (int Stage = 0; MaxStage > Stage; Stage++)
        {
            textobject[Stage].text = Data.GetStageScore(Stage).ToString();


            if (Data.GetStageClear(Stage))
            {
                nowCliar++;


                for (int nowcliarstage = 0; nowcliarstage <= nowCliar; nowcliarstage++)
                {
                    Achevementobject[nowcliarstage].SetActive(true);
                }

                //収集物チェック
                for (int Ahevement = 0; Ahevement <= MaxAchievement; Ahevement++)
                {
                    bool checkAchevement = Data.GetAchievement(Stage, Ahevement);
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
