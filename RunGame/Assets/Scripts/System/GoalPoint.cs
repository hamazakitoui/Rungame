using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �S�[���|�C���g </summary>
public class GoalPoint : MonoBehaviour
{
    // �I�u�W�F�N�g�^�O
    readonly string playerTag = "Player";
    private bool isClear = false; // �N���A�t���O
    [Header("�X�e�[�W�ԍ�")] [SerializeField] private int stageNum;
    [Header("�Z���N�g�V�[��")] [SerializeField] private SceneObject selectScene;
    /// <summary> �S�[�� </summary>
    public void GameClear()
    {
        if (isClear) return; // ��񂾂����s
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        data.SetStageClear(stageNum, true); // �N���A�ɕύX
        SaveManager.Instance.Save(data); // �Z�[�u
        FadeSceneManager.Instance.LoadScene(selectScene); // �Z���N�g�V�[���Ɉړ�
        isClear = true;
    }
    /// <summary> �S�[�� </summary>
    /// <param name="stageNum">�X�e�[�W�ԍ�</param>
    public void GameClear(int stageNum)
    {
        if (isClear) return; // ��񂾂����s
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        data.SetStageClear(stageNum, true); // �N���A�ɕύX
        SaveManager.Instance.Save(data); // �Z�[�u
        FadeSceneManager.Instance.LoadScene(selectScene); // �Z���N�g�V�[���Ɉړ�
        isClear = true;
    }
    /// <summary> �ō��X�R�A�ۑ� </summary>
    /// <param name="score">�X�R�A</param>
    public void SaveScore(int score)
    {
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        // �n�C�X�R�A���X�V������
        if (data.Score < score)
        {
            data.Score = score;
            SaveManager.Instance.Save(data); // �Z�[�u
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag) GameClear(); // �v���C���[�ɓ���������N���A
    }
}
