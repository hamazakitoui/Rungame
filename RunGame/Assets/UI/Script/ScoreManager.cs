using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    Text scoreText;

    int score;

    int limit = 999999;

    public int GetScore { get { return score; } }
    void Start()
    {
        // コンポーネントを取得
        scoreText = GetComponent<Text>();
        // スコアに初期値を入れる
        score = 0;
        // 表示しているスコアを更新
        scoreText.text = score.ToString("000000");
    }
    public void SetScore(int value)
    {
        // スコアに加算
        score += value;

        // スコアの表示限界を超えているか
        if (score > limit)
        {
            // 最大値を表示
            scoreText.text = limit.ToString();
        }
        else
        {
            // 表示しているスコアを更新
            scoreText.text = score.ToString("000000");
        }    
    }
}
