using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] GameObject collectibles;
    [SerializeField] GameObject scoreMane;
    [SerializeField] RectTransform selectImage;        // �I���A�C�R��
    [SerializeField] int stageNumber = 0;              // �X�e�[�W�ԍ�

    private static ResultUIManager instance; // �C���X�^���X�ۑ��p�ϐ�

    int number = 0;             // �����ԍ�

    float retryPos_x = -204f;
    float nextPos_x = 78f;

    const int MAXNUMBER = 1;    // �ő又���ԍ�
    enum ProcessNumber
    {
        RETRY,
        NEXT,
    }

    public static ResultUIManager Instance
    {
        get
        {
            // �C���X�^���X���ݒ肳��Ă��Ȃ����
            if (instance == null)
            {
                instance = FindObjectOfType<ResultUIManager>(); // �C���X�^���X����
                // �G���[����
                if (instance == null) Debug.LogError($"{typeof(ResultUIManager)}��������܂���!");
            }
            return instance;
        }
    }

    public void ResultProcess()
    {
        StartCoroutine(Result());
    }

    private IEnumerator Result()
    {
        // �X�R�A�Ȃǂ��擾
        int score = scoreMane.GetComponent<ScoreManager>().GetScore;
        bool[] collect = collectibles.GetComponent<CollectiblesUI>().GetCollectibles;

        // �eUI�̔�\��
        scoreMane.SetActive(false);
        collectibles.SetActive(false);

        // UI��\��
        GetComponent<Animator>().SetBool("isClear", true);

        yield return new WaitForSeconds(3.0f);


        // ���肵���R���N�g�A�C�e���̕\��
        for(int i = 0; i < collect.Length; i++)
        {
            // �R���N�g�A�C�e�����擾���Ă���Ȃ珈�����s��
            if (collect[i])
            {

            }
            yield return null;
        }

        // �X�R�A�̕\��


        // �Z�[�u���s��
        // �Z�[�u�f�[�^�̍쐬
        SaveData saveData = SaveManager.Instance.GetData;
        // �X�R�A�̏�������
        saveData.SetScore(stageNumber, score);
        // �R���N�g�A�C�e���̏�������
        for (int i = 0; i < collect.Length; i++)
        {
            saveData.SetAchievement(stageNumber, i, collect[i]);
        }
        // �Z�[�u�t�@�C���ɏ�������
        SaveManager.Instance.Save();

        // ���͏���
        while (true)
        {
            yield return null;
            // ���E�L�[�őI����ύX����
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // �����ԍ������Z����0�����ɂȂ����ꍇ�A0�ɒ���
                number--;
                if (number < 0) number = 0;
                // �I���A�C�R�����ړ�������
                yield return SelectImageMove();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // �����ԍ������Z����ő�I�����l�𒴂�����ő�l�ɖ߂�
                number++;
                if (number > MAXNUMBER) number = MAXNUMBER;
                // �I���A�C�R�����ړ�������
                yield return SelectImageMove();
            }
            // ����L�[�������ꂽ���A�V�[�����ڂ��s��
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // �ԍ����Ƃɏ������s��
                switch (number)
                {
                    // ���݂̃V�[����ǂݍ��݂Ȃ���
                    case (int)ProcessNumber.RETRY:
                        FadeSceneManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    // �Z���N�g�V�[����ǂݍ���
                    case (int)ProcessNumber.NEXT:
                        FadeSceneManager.Instance.LoadScene("SelectScene");
                        break;
                    // ���Ă͂܂�Ȃ����l�����o���ꂽ�ꍇ�A�G���[�����o���ăZ���N�g�V�[���ɖ߂�
                    default:
                        Debug.LogError("���Ă͂܂�Ȃ����l�����o����܂����I");
                        FadeSceneManager.Instance.LoadScene("SelectScene");
                        break;
                }
            }
        }
    }
    IEnumerator SelectImageMove()
    {
        // ���݂̏����ԍ��̈ʒu�Ɉړ�������
        switch (number)
        {
            case (int)ProcessNumber.RETRY:
                selectImage.localPosition = new Vector3(retryPos_x, selectImage.localPosition.y, 0.0f);
                break;
            case (int)ProcessNumber.NEXT:
                selectImage.localPosition = new Vector3(nextPos_x, selectImage.localPosition.y, 0.0f);
                break;
            // ���Ă͂܂�Ȃ����l�����o���ꂽ�ꍇ�A�G���[�����o��
            default:
                Debug.LogError("���Ă͂܂�Ȃ����l�����o����܂����I");
                break;
        }
        yield return null;
    }
}
