using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeforeScore : MonoBehaviour
{
    SaveData Data;//�Z�[�u�f�[�^
    [SerializeField] Sprite GetAchievement;//���W������
    [SerializeField] GameObject[] Achevementobject;//�e�X�e�[�W�̎��W��
    [SerializeField] Text[] textobject;//�X�R�A
    int nowCliar;
    const int MaxStage = 4;
    const int MaxAchievement = 3;
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.Load();
        Data = SaveManager.Instance.GetData;

        //�N���A�����X�e�[�W���`�F�b�N
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

                //���W���`�F�b�N
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
