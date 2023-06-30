using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �S�[���|�C���g </summary>
public class GoalPoint : MonoBehaviour
{
    // �^�O
    private readonly string playerTag = "Player";
    private bool isClear = false; // �N���A�t���O
    [Header("�X�e�[�W�ԍ�")] [SerializeField] private int stageNum;
    [Header("���ʕ\������")] [SerializeField] private float clearWait = 3.0f;
    [Header("�Z���N�g�V�[��")] [SerializeField] private SceneObject selectScene;
    /// <summary> �S�[�� </summary>
    public void GameClear()
    {
        if (isClear) return; // ��񂾂����s
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        data.SetStageClear(stageNum, true); // �N���A�ɕύX
        SaveManager.Instance.Save(data); // �Z�[�u
        isClear = true;
    }
    /// <summary> �S�[�� </summary>
    /// <param name="stageNum">�X�e�[�W�ԍ�</param> <param name="score">�X�R�A</param>
    public void GameClear(int stageNum, int score)
    {
        if (isClear) return; // ��񂾂����s
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        data.SetStageClear(stageNum, true); // �N���A�ɕύX
        SaveManager.Instance.Save(data); // �Z�[�u
        isClear = true;
    }
    /// <summary> ���ѕۑ� </summary>
    /// <param name="element">�v�f�ԍ�</param>
    public void SaveAchievement(int element)
    {
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        data.SetAchievement(stageNum, element, true); // ���ѕύX
        SaveManager.Instance.Save(data); // �Z�[�u
    }
    /// <summary> �ō��X�R�A�ۑ� </summary>
    /// <param name="score">�X�R�A</param>
    public void SaveScore(int score)
    {
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        // �n�C�X�R�A���X�V������
        if (data.GetStageScore(stageNum) < score)
        {
            data.SetScore(stageNum, score);
            SaveManager.Instance.Save(data); // �Z�[�u
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ɓ���������
        if (collision.tag == playerTag) ResultUIManager.Instance.ResultProcess(); // ���ʂ�ۑ�
    }
}
