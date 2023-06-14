using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトルマネージャー </summary>
public class TitleManager : MonoBehaviour
{
    bool startFlag = false; // 開始フラグ
    [SerializeField] SceneObject selectScene; // ロードシーン
    [SerializeField] AudioObject bgm; // BGM
    // Start is called before the first frame update
    void Start()
    {
        if (bgm != null && bgm != "") AudioManager.Instance.PlayBGM(bgm); // BGM再生
    }

    // Update is called once per frame
    void Update()
    {
        GameStart(); // ゲーム開始
    }
    /// <summary> ゲーム開始 </summary>
    void GameStart()
    {
        if (startFlag) return;
        // スペースキーで開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FadeSceneManager.Instance.LoadScene(selectScene);
            startFlag = true;
        }
    }
}
