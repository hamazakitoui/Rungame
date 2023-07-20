using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    [SerializeField] RectTransform selectIcon;
    [SerializeField] TitleManager titleManager;

    AudioManager audioManager;

    int select = 0;         // �I���Q�ƒl
    int volumeValue = 1;    // ���ʒl
    bool isProcess = false;

    // ���ʃA�C�R���̏����ʒu
    float volumePos = -114f;
    float volumeMove = 34.3f;
    // ���ڕʂ̍��W
    Vector3 selectPos_bgm = new Vector3(0f, 34f, 0f);
    Vector3 selectPos_se = new Vector3(0f, -38f, 0f);
    Vector3 selectPos_end = new Vector3(0f, -111f, 0f);

    // �I�v�V�����̑I�𒆂̍���
    enum OptionSelect
    {
        BGM,
        SE,
        End,
    }

    void Update()
    {
        if (!isProcess) return;

        // ���ʂ̑傫���̕ύX���s��
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
        // �Z���N�g�A�C�R���𑀍삷��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �I�𒆂̎Q�Ƃ�ύX
            select--;
            if (select < 0) select = (int)OptionSelect.End;
            // �A�C�R�����ړ�������
            SelectMove();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �I�𒆂̎Q�Ƃ�ύX
            select++;
            if (select > (int)OptionSelect.End) select = 0;
            // �A�C�R�����ړ�������
            SelectMove();
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (select != (int)OptionSelect.End) return;
            isProcess = false;
            // �I�v�V���������
            gameObject.SetActive(false);
            // �^�C�g���̏������ĊJ
            titleManager.processFlag = true;
        }
    }

    public void StartOption()
    {
        // �Z���N�g�A�C�R���������ʒu�ɖ߂�
        select = 0;
        SelectMove();
        // ���݂̉��ʂɉ����Ĉʒu��ύX����
        audioManager = AudioManager.Instance;
        

        // �I�u�W�F�N�g����������
        gameObject.SetActive(true);
        isProcess = true;
    }
    /// <summary>
    ///  �Z���N�g�A�C�R���̍��W�ړ�
    /// </summary>
    void SelectMove()
    {
        switch (select)
        {
            case (int)OptionSelect.BGM:
                selectIcon.localPosition = selectPos_bgm;
                break;
            case (int)OptionSelect.SE:
                selectIcon.localPosition = selectPos_se;
                break;
            case (int)OptionSelect.End:
                selectIcon.localPosition = selectPos_end;
                break;
        }
    }
}
