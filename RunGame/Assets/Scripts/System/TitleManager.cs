using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �^�C�g���}�l�[�W���[ </summary>
public class TitleManager : MonoBehaviour
{
    int select = 0;         // �I���Q�ƒl
    public bool processFlag { get; set; }     // �����t���O
    [SerializeField] SceneObject selectScene; // ���[�h�V�[��
    [SerializeField] AudioObject bgm; // BGM
    [SerializeField] RectTransform SelectIcon;
    [SerializeField] OptionController option;
    [SerializeField] UISEData uISE;

    Vector3 selectPos_start = new Vector3(-106f, -28f, 0f);
    Vector3 selectPos_option = new Vector3(-106f, -90f, 0f);
    Vector3 selectPos_end = new Vector3(-106f, -146f, 0f);

    // �^�C�g����ʂ̑I�𒆂̍���
    enum TitleSelect
    {
        GameStart,
        Option,
        End,
    }

    void Start()
    {
        if (bgm != null && bgm != "") AudioManager.Instance.PlayBGM(bgm); // BGM�Đ�
        SaveManager.Instance.Load(); // �ǂݍ���

        processFlag = true; // ����\�ɂ���
    }
    void Update()
    {
        if (!processFlag) return;

        // �X�y�[�X�L�[�ŏ������s��
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            // �ړ�SE��炷
            //if (uISE.GetDecisionSE != null) { }
            switch (select)
            {
                case (int)TitleSelect.GameStart:
                    processFlag = false;
                    FadeSceneManager.Instance.LoadScene(selectScene);
                    break;
                case (int)TitleSelect.Option:
                    processFlag = false;
                    option.StartOption();
                    break;
                case (int)TitleSelect.End:
                    processFlag = false;
                    Quit();
                    break;
            }
        }
        // �Z���N�g�A�C�R���𑀍삷��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �I�𒆂̎Q�Ƃ�ύX
            select--;
            if (select < 0) select = (int)TitleSelect.End;
            // �ړ�SE��炷
            //if (uISE.GetSelectSE != null){}
            // �A�C�R�����ړ�������
            SelectMove();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �I�𒆂̎Q�Ƃ�ύX
            select++;
            if (select > (int)TitleSelect.End) select = 0;
            // �ړ�SE��炷
            //if (uISE.GetSelectSE != null){}
            // �A�C�R�����ړ�������
            SelectMove();
        }
    }
    /// <summary>
    ///  �Z���N�g�A�C�R���̍��W�ړ�
    /// </summary>
    void SelectMove()
    {
        switch (select)
        {
            case (int)TitleSelect.GameStart:
                SelectIcon.localPosition = selectPos_start;
                break;
            case (int)TitleSelect.Option:
                SelectIcon.localPosition = selectPos_option;
                break;
            case (int)TitleSelect.End:
                SelectIcon.localPosition = selectPos_end;
                break;
        }
    }
    /// <summary> �A�v���̏I�� </summary>
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
    }
}
