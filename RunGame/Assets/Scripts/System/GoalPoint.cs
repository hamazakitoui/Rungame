using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゴールポイント </summary>
public class GoalPoint : MonoBehaviour
{
    private bool isClear = false; // クリアフラグ
    // タグ
    private const string PLAYER_TAG = "Player";
    [Header("ステージ番号")] [SerializeField] private int stageNum;
    [Header("セレクトシーン")] [SerializeField] private SceneObject selectScene;
    /// <summary> ゴール </summary>
    public void GameClear()
    {
        if (isClear) return; // 一回だけ実行
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        data.SetStageClear(stageNum, true); // クリアに変更
        SaveManager.Instance.Save(data); // セーブ
        FadeSceneManager.Instance.LoadScene(selectScene); // セレクトシーンに移動
        isClear = true;
    }
    /// <summary> ゴール </summary>
    /// <param name="stageNum">ステージ番号</param>
    public void GameClear(int stageNum)
    {
        if (isClear) return; // 一回だけ実行
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        data.SetStageClear(stageNum, true); // クリアに変更
        SaveManager.Instance.Save(data); // セーブ
        FadeSceneManager.Instance.LoadScene(selectScene); // セレクトシーンに移動
        isClear = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == PLAYER_TAG) GameClear(); // ゲームクリア
    }
}
