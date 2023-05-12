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
        // �R���|�[�l���g���擾
        scoreText = GetComponent<Text>();
        // �X�R�A�ɏ����l������
        score = 0;
        // �\�����Ă���X�R�A���X�V
        scoreText.text = score.ToString("00000000");
    }
    public void SetScore(int value)
    {
        // �X�R�A�ɉ��Z
        score += value;
        // �\�����Ă���X�R�A���X�V
        scoreText.text = score.ToString("00000000");
    }
}
