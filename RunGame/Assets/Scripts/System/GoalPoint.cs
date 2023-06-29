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
    [Header("結果表示時間")] [SerializeField] private float clearWait = 3.0f;
    [Header("セレクトシーン")] [SerializeField] private SceneObject selectScene;
    /// <summary> クリアコルーチン </summary>
    /// <param name="score">スコア</param>
    /// <returns></returns>
    private IEnumerator ClearFade(int score)
    {
        yield return new WaitForSeconds(clearWait); // 一定時間待機
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        data.SetStageClear(stageNum, true); // クリアに変更
        if (data.GetStageScore(stageNum) < score) data.SetScore(stageNum, score); // スコア保存
        SaveManager.Instance.Save(data); // セーブ
        FadeSceneManager.Instance.LoadScene(selectScene); // セレクトシーンに移動
    }
    /// <summary> クリアコルーチン </summary>
    /// <param name="stageNum">ステージ番号</param> <param name="score">スコア</param>
    /// <returns></returns>
    private IEnumerator ClearFade(int stageNum, int score)
    {
        yield return new WaitForSeconds(clearWait); // 一定時間待機
        SaveData data = SaveManager.Instance.GetData; // セーブデータ
        data.SetStageClear(stageNum, true); // クリアに変更
        if (data.GetStageScore(stageNum) < score) data.SetScore(stageNum, score); // スコア保存
        SaveManager.Instance.Save(data); // セーブ
        FadeSceneManager.Instance.LoadScene(selectScene); // セレクトシーンに移動
    }
    /// <summary> ゴール </summary>
    /// <param name="score">スコア</param>
    public void GameClear(int score)
    {
        if (isClear) return; // 一回だけ実行
        StartCoroutine(ClearFade(score)); // クリア
        isClear = true;
    }
    /// <summary> ゴール </summary>
    /// <param name="stageNum">ステージ番号</param> <param name="score">スコア</param>
    public void GameClear(int stageNum, int score)
    {
        if (isClear) return; // 一回だけ実行
        StartCoroutine(ClearFade(stageNum, score)); // クリア
        isClear = true;
    }
}
