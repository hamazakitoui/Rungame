using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �S�[���|�C���g </summary>
public class GoalPoint : MonoBehaviour
{
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
}
