using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    Text scoreText;

    int score = 0;

    private void Start()
    {
        // �R���|�[�l���g���擾
        scoreText = GetComponent<Text>();


    }


}
