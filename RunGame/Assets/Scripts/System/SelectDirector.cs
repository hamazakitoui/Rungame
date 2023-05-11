using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> セレクトディレクター </summary>
public class SelectDirector : MonoBehaviour
{
    private bool isSelect = false; // 選択判定
    [SerializeField] GameObject[] scenes; // 選択シーン配列
    [SerializeField] SceneObject[] selectScenes; // 選択可能シーン配列
    // Start is called before the first frame update
    void Start()
    {
        SaveData data = SaveManager.Instance.GetData; // ステージデータ
        // 選択できるシーンを初期化
        for(int s = 0; s < scenes.Length; s++)
        {
            // ステージ数を超えたら
            if (s >= data.StageLength)
            {
                scenes[s].SetActive(false); // 非表示
                continue;
            }
            if (s == 0) scenes[s].SetActive(true); // 初期シーンは常に選択できる
            else scenes[s].SetActive(data.GetStageClear(s - 1)); // 前のシーンがクリアされていたら表示
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary> シーン選択 </summary>
    /// <param name="scene">選択シーン番号</param>
    public void SceneSelect(int scene)
    {
        if (scene >= selectScenes.Length || scene < 0) return; // 番号が配列外なら無視
        if (isSelect) return; // 既に選択済みなら無視
        FadeSceneManager.Instance.LoadScene(selectScenes[scene]); // シーン読み込み
        isSelect = true;
    }
}
