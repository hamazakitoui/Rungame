using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゴールポイント </summary>
public class GoalPoint : MonoBehaviour
{
    // タグ
    private readonly string playerTag = "Player";
    private bool isClear = false; // クリアフラグ
    [Header("ステージ番号")] [SerializeField] private int stageNum;
    [Header("結果表示時間")] [SerializeField] private float clearWait = 3.0f;
    [Header("セレクトシーン")] [SerializeField] private SceneObject selectScene;
    /// <summary> ゴール </summary>
    public void GameClear()
    {
        if (isClear) return; // 一回だけ実行
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        data.SetStageClear(stageNum, true); // クリアに変更
        SaveManager.Instance.Save(data); // セーブ
        isClear = true;
    }
    /// <summary> ゴール </summary>
    /// <param name="stageNum">ステージ番号</param> <param name="score">スコア</param>
    public void GameClear(int stageNum, int score)
    {
        if (isClear) return; // 一回だけ実行
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        data.SetStageClear(stageNum, true); // クリアに変更
        SaveManager.Instance.Save(data); // セーブ
        isClear = true;
    }
    /// <summary> 実績保存 </summary>
    /// <param name="element">要素番号</param>
    public void SaveAchievement(int element)
    {
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        data.SetAchievement(stageNum, element, true); // 実績変更
        SaveManager.Instance.Save(data); // セーブ
    }
    /// <summary> 最高スコア保存 </summary>
    /// <param name="score">スコア</param>
    public void SaveScore(int score)
    {
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        // ハイスコアを更新したら
        if (data.GetStageScore(stageNum) < score)
        {
            data.SetScore(stageNum, score);
            SaveManager.Instance.Save(data); // セーブ
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに当たったら
        if (collision.tag == playerTag) ResultUIManager.Instance.ResultProcess(); // 結果を保存
    }
}
