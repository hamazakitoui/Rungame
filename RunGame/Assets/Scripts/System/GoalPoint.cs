using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゴールポイント </summary>
public class GoalPoint : MonoBehaviour
{
    // オブジェクトタグ
    readonly string playerTag = "Player";
    private bool isClear = false; // クリアフラグ
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
    /// <summary> 最高スコア保存 </summary>
    /// <param name="score">スコア</param>
    public void SaveScore(int score)
    {
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        // ハイスコアを更新したら
        if (data.Score < score)
        {
            data.Score = score;
            SaveManager.Instance.Save(data); // セーブ
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag) GameClear(); // プレイヤーに当たったらクリア
    }
}
