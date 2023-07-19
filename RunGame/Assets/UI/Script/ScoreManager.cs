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
        // �R���|�[�l���g���擾
        scoreText = GetComponent<Text>();
        // �X�R�A�ɏ����l������
        score = 0;
        // �\�����Ă���X�R�A���X�V
        scoreText.text = score.ToString("000000");
    }
    public void SetScore(int value)
    {
        // �X�R�A�ɉ��Z
        score += value;

        // �X�R�A�̕\�����E�𒴂��Ă��邩
        if (score > limit)
        {
            // �ő�l��\��
            scoreText.text = limit.ToString();
        }
        else
        {
            // �\�����Ă���X�R�A���X�V
            scoreText.text = score.ToString("000000");
        }    
    }
}
