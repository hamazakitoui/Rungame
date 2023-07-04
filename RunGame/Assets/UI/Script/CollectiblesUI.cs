using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesUI : MonoBehaviour
{
    [Header("収集物UIのプレファブ")]
    [SerializeField] GameObject collectiblesImageUI;
    [Header("収集物の画像")]
    [SerializeField] Sprite collectiblesSprite;
    [Header("収集物の数")]
    [SerializeField]int collectiblesValue = 3;      // 収集物の数

    bool[] collectibles;                            // 収集物の獲得状況を入れる
    Image[] collectiblesImage;                      // 収集物の表示UI         

    public bool[] GetCollectibles { get { return collectibles; } }
    void Start()
    {
        // 初期値を入れる
        collectibles = new bool[collectiblesValue];
        collectiblesImage = new Image[collectiblesValue];

        for(int i = 0; i < collectiblesValue; i++)
        {
            // 収集物の数分画像を生成して配列に入れる
            collectiblesImage[i] = Instantiate(collectiblesImageUI, transform.position, Quaternion.identity, transform).gameObject.GetComponent<Image>();
        }
    }

    public void SetCollectibles(int number)
    {
        // 収集物を入手
        collectibles[number] = true;

        // UIに反映
        GetComponent<CollectStaging>().Staging(number);
    }
}
