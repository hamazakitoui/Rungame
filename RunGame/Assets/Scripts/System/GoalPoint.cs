using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �S�[���|�C���g </summary>
public class GoalPoint : MonoBehaviour
{
    private bool isClear = false; // �N���A�t���O
    [Header("�X�e�[�W�ԍ�")] [SerializeField] private int stageNum;
    [Header("���ʕ\������")] [SerializeField] private float clearWait = 3.0f;
    [Header("�Z���N�g�V�[��")] [SerializeField] private SceneObject selectScene;
    /// <summary> �N���A�R���[�`�� </summary>
    /// <returns></returns>
    private IEnumerator ClearFade()
    {
        yield return new WaitForSeconds(clearWait); // ��莞�ԑҋ@
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        data.SetStageClear(stageNum, true); // �N���A�ɕύX
        SaveManager.Instance.Save(data); // �Z�[�u
        FadeSceneManager.Instance.LoadScene(selectScene); // �Z���N�g�V�[���Ɉړ�
    }
    /// <summary> �N���A�R���[�`�� </summary>
    /// <param name="stageNum">�X�e�[�W�ԍ�</param> <param name="score">�X�R�A</param>
    /// <returns></returns>
    private IEnumerator ClearFade(int stageNum, int score)
    {
        yield return new WaitForSeconds(clearWait); // ��莞�ԑҋ@
        SaveData data = SaveManager.Instance.GetData; // �Z�[�u�f�[�^
        data.SetStageClear(stageNum, true); // �N���A�ɕύX
        SaveManager.Instance.Save(data); // �Z�[�u
        FadeSceneManager.Instance.LoadScene(selectScene); // �Z���N�g�V�[���Ɉړ�
    }
    /// <summary> �S�[�� </summary>
    public void GameClear()
    {
        if (isClear) return; // ��񂾂����s
        StartCoroutine(ClearFade()); // �N���A
        isClear = true;
    }
    /// <summary> �S�[�� </summary>
    /// <param name="stageNum">�X�e�[�W�ԍ�</param> <param name="score">�X�R�A</param>
    public void GameClear(int stageNum, int score)
    {
        if (isClear) return; // ��񂾂����s
        StartCoroutine(ClearFade(stageNum, score)); // �N���A
        isClear = true;
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
}
