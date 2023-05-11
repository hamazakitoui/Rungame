using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �Z���N�g�f�B���N�^�[ </summary>
public class SelectDirector : MonoBehaviour
{
    private bool isSelect = false; // �I�𔻒�
    [SerializeField] GameObject[] scenes; // �I���V�[���z��
    [SerializeField] SceneObject[] selectScenes; // �I���\�V�[���z��
    // Start is called before the first frame update
    void Start()
    {
        SaveData data = SaveManager.Instance.GetData; // �X�e�[�W�f�[�^
        // �I���ł���V�[����������
        for(int s = 0; s < scenes.Length; s++)
        {
            // �X�e�[�W���𒴂�����
            if (s >= data.StageLength)
            {
                scenes[s].SetActive(false); // ��\��
                continue;
            }
            if (s == 0) scenes[s].SetActive(true); // �����V�[���͏�ɑI���ł���
            else scenes[s].SetActive(data.GetStageClear(s - 1)); // �O�̃V�[�����N���A����Ă�����\��
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary> �V�[���I�� </summary>
    /// <param name="scene">�I���V�[���ԍ�</param>
    public void SceneSelect(int scene)
    {
        if (scene >= selectScenes.Length || scene < 0) return; // �ԍ����z��O�Ȃ疳��
        if (isSelect) return; // ���ɑI���ς݂Ȃ疳��
        FadeSceneManager.Instance.LoadScene(selectScenes[scene]); // �V�[���ǂݍ���
        isSelect = true;
    }
}
