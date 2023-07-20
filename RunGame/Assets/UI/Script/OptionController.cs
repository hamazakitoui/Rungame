using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    [SerializeField] RectTransform bgmIconPos;
    [SerializeField] RectTransform seIconPos;
    [SerializeField] RectTransform selectIcon;
    [SerializeField] TitleManager titleManager;
    [SerializeField] UISEData uISE;

    AudioManager audioManager;

    int select = 0;         // �I���Q�ƒl
    bool isProcess = false;

    // ���ʂ̑�����
    float fluctuation = 0.1f;
    // ���ʂ̍ő�l
    float MAXVOLUME = 1.0f;
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
            switch (select)
            {
                case (int)OptionSelect.BGM:
                    // ���ʂ��O���傫���Ȃ�Ώ������s��
                    if(audioManager.bgmVolume > 0)
                    {
                        // ���ʂ�Ⴍ����
                        audioManager.bgmVolume -= fluctuation;
                        bgmIconPos.localPosition -= new Vector3(volumeMove, 0f, 0f);

                        // �ړ�SE��炷
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // ����SE��炷
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
                case (int)OptionSelect.SE:
                    // ���ʂ��O���傫���Ȃ�Ώ������s��
                    if (audioManager.seVolume > 0)
                    {
                        // ���ʂ�Ⴍ����
                        audioManager.seVolume -= fluctuation;
                        seIconPos.localPosition -= new Vector3(volumeMove, 0f, 0f);

                        // �ړ�SE��炷
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // ����SE��炷
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (select)
            {
                case (int)OptionSelect.BGM:
                    // ���ʂ��P���Ⴂ�Ȃ�Ώ������s��
                    if (audioManager.bgmVolume < MAXVOLUME)
                    {
                        // ���ʂ���������
                        audioManager.bgmVolume += fluctuation;
                        bgmIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);

                        // �ړ�SE��炷
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // ����SE��炷
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
                case (int)OptionSelect.SE:
                    // ���ʂ��P���Ⴂ�Ȃ�Ώ������s��
                    if (audioManager.seVolume < MAXVOLUME)
                    {
                        // ���ʂ���������
                        audioManager.seVolume += fluctuation;
                        seIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);

                        // �ړ�SE��炷
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // ����SE��炷
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
            }
        }
        // �Z���N�g�A�C�R���𑀍삷��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �I�𒆂̎Q�Ƃ�ύX
            select--;
            if (select < 0) select = (int)OptionSelect.End;
            // �ړ�SE��炷
            //if (uISE.GetSelectSE != null){}
            // �A�C�R�����ړ�������
            SelectMove();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �I�𒆂̎Q�Ƃ�ύX
            select++;
            if (select > (int)OptionSelect.End) select = 0;
            // �ړ�SE��炷
            //if (uISE.GetSelectSE != null){}
            // �A�C�R�����ړ�������
            SelectMove();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (select != (int)OptionSelect.End) return;
            isProcess = false;
            // �I�v�V���������
            gameObject.SetActive(false);
            // �ړ�SE��炷
            //if (uISE.GetCancelSE != null) { }
            // �^�C�g���̏������ĊJ
            titleManager.processFlag = true;
        }
    }

    public void StartOption()
    {
        // �Z���N�g�A�C�R���������ʒu�ɖ߂�
        select = 0;
        SelectMove();
        // �����X�N���v�g���擾
        audioManager = AudioManager.Instance;
        // �����ʒu�ɒu��
        bgmIconPos.localPosition = new Vector3(volumePos, bgmIconPos.localPosition.y, 0f);
        seIconPos.localPosition = new Vector3(volumePos, seIconPos.localPosition.y, 0f);
        // ���݂̉��ʂɉ����Ĉʒu��ύX����
        for (float i = 0; i < audioManager.bgmVolume; i += fluctuation)
        {
            bgmIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);
        }
        for (float i = 0; i < audioManager.seVolume; i += fluctuation)
        {
            seIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);
        }
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
