using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    Text scoreText;

    int score;

    public int GetScore { get { return score; } }
    void Start()
    {
        // コンポーネントを取得
        scoreText = GetComponent<Text>();
        // スコアに初期値を入れる
        score = 0;
        // 表示しているスコアを更新
        scoreText.text = score.ToString("00000000");
    }
    public void SetScore(int value)
    {
        // スコアに加算
        score += value;
        // 表示しているスコアを更新
        scoreText.text = score.ToString("00000000");
    }
}
