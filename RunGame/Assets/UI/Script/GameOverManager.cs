using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
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

            }
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
