using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject SelectImage;        // �I���A�C�R��

    int number = 0;             // �����ԍ�
    bool isControl = false;     // �������s�����̃t���O

    enum ProcessNumber
    {
        RETRY,      // �X�e�[�W�����Ȃ���
        EXIT,       // �X�e�[�W����o��
    }

    void Update()
    {
        // �������s��Ȃ��ꍇ�Areturn��Ԃ�
        if (!isControl) { return; }

        // ����{�^���������ꂽ�ꍇ�A����ɉ������������s��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (number)
            {
                // ���݂̃V�[����ǂݍ��݂Ȃ���
                case (int)ProcessNumber.RETRY:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                // �Z���N�g�V�[����ǂݍ���
                case (int)ProcessNumber.EXIT:
                    SceneManager.LoadScene("SelectScene");
                    break;
                // ���Ă͂܂�Ȃ����l�����o���ꂽ�ꍇ�A�G���[�����o���ăZ���N�g�V�[���ɖ߂�
                default:
                    Debug.LogError("���Ă͂܂�Ȃ����l�����o����܂����I");
                    SceneManager.LoadScene("SelectScene");
                    break;
            }
        }

        // �㉺�L�[�őI����ύX����
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �����ԍ������Z����0�����ɂȂ����ꍇ�A0�ɒ���
            number--;
            if (number < 0) number = 0;

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �����ԍ������Z����ő�I�����l�𒴂�����ő�l�ɖ߂�
            number++;
            
        }
    }

    public void GameOver()
    {
        // ���g��\������
        this.gameObject.SetActive(true);
        // �������s���悤�Ƀt���O������
        isControl = true;
    }
}
