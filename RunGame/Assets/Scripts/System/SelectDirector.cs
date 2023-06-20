using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �Z���N�g�f�B���N�^�[ </summary>
public class SelectDirector : MonoBehaviour
{
    // �ҋ@����
    private const float APP_WAIT = 0.2f;
    private bool isSelect = false; // �I�𔻒�
    [SerializeField] AudioObject bgm; // BGM
    [SerializeField] GameObject[] scenes; // �I���V�[���z��
    [SerializeField] SceneObject[] selectScenes; // �I���\�V�[���z��
    // Start is called before the first frame update
    void Start()
    {
        if (bgm != null && bgm != "") AudioManager.Instance.PlayBGM(bgm); // BGM�Đ�
        SaveManager.Instance.Load(); // �Z�[�u�f�[�^�ǂݍ���
        SaveData data = SaveManager.Instance.GetData; // �X�e�[�W�f�[�^
        Debug.Log(data.StageLength);
        // �I���ł���V�[����������
        for(int s = 0; s < scenes.Length; s++)
        {
            // �X�e�[�W���𒴂�����
            if (s >= data.StageLength)
            {
                scenes[s].SetActive(false); // ��\��
                continue;
            }
            if (s == 0) StartCoroutine(SceneActive(scenes[s], s)); // �����V�[���͏�ɑI���ł���
            else
            {
                // �O�̃V�[�����N���A����Ă�����\��
                if (data.GetStageClear(s - 1)) StartCoroutine(SceneActive(scenes[s], s));
                else scenes[s].SetActive(false); // ��\��
            }
        }
    }
    /// <summary> �Z���N�g�{�^���o���C�x���g </summary>
    /// <param name="scene">�Z���N�g�{�^��</param> <param name="num">�ԍ�</param>
    /// <returns></returns>
    private IEnumerator SceneActive(GameObject scene, int num)
    {
        yield return new WaitForSeconds((num + 1) * APP_WAIT); // ��莞�ԑҋ@
        scene.SetActive(true); // �{�^���\��
        Vector3 scale = scene.transform.localScale; // �{�^���̑傫��
        float rad = scene.transform.localEulerAngles.z; // �{�^���̊p�x
        scene.transform.localScale = new Vector3(0, 0, 1); // �傫�����[����
        // �i�X�傫��
        while (scene.transform.localScale.x < scale.x)
        {
            scene.transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime;
            scene.transform.Rotate(0, 0, (num % 2 == 0 ? 360 : -360) * Time.deltaTime); // ��]
            yield return 0;
        }
        scene.transform.localScale = scale; // �傫�������ɖ߂�
        scene.transform.rotation = Quaternion.Euler(0, 0, rad); // �p�x�����ɖ߂�
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
