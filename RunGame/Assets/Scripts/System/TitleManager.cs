using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �^�C�g���}�l�[�W���[ </summary>
public class TitleManager : MonoBehaviour
{
    bool startFlag = false; // �J�n�t���O
    [SerializeField] SceneObject selectScene; // ���[�h�V�[��
    [SerializeField] AudioObject bgm; // BGM
    // Start is called before the first frame update
    void Start()
    {
        if (bgm != null && bgm != "") AudioManager.Instance.PlayBGM(bgm); // BGM�Đ�
    }

    // Update is called once per frame
    void Update()
    {
        GameStart(); // �Q�[���J�n
    }
    /// <summary> �Q�[���J�n </summary>
    void GameStart()
    {
        if (startFlag) return;
        // �X�y�[�X�L�[�ŊJ�n
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FadeSceneManager.Instance.LoadScene(selectScene);
            startFlag = true;
        }
    }
}
