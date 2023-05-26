using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �G���f�B���O </summary>
public class EndingDirector : MonoBehaviour
{
    float waitDelta = 0.0f; // �ҋ@�o�ߎ���
    bool isLoading = false; // �ǂݍ��ݒ��t���O
    // ���̃V�[���܂ł̑ҋ@����
    const float NEXT_WAIT = 3f;
    // �X�^�b�t���[���ړ����x�A�X�^�b�t���[������Y���W�A�X�^�b�t���[���I��Y���W
    [SerializeField] float staffRollSpeed = 2.5f, srStartY = 0, srEndY = 10;
    // �X�^�b�t���[��RectTransform�v���p�e�B
    [Header("�X�^�b�t���[��")] [SerializeField] RectTransform staffRollRect;
    [Header("�N���A�V�[��")] [SerializeField] SceneObject clearScene; // �N���A�V�[��
    // Start is called before the first frame update
    void Start()
    {
        Vector3 srPos = Vector3.zero; // �X�^�b�t���[���̏����ʒu
        srPos.y = srStartY; // Y���W����������
        staffRollRect.anchoredPosition = srPos; // �ʒu������
    }

    // Update is called once per frame
    void Update()
    {
        StaffRollMove(); // �X�^�b�t���[��
    }
    /// <summary> �X�^�b�t���[�� </summary>
    void StaffRollMove()
    {
        // �I���ʒu�܂ňړ�����܂ňړ�
        if (staffRollRect.anchoredPosition.y < srEndY)
        {
            Vector2 vel = new Vector2(0, staffRollSpeed * Time.deltaTime); // �ړ����x
            staffRollRect.anchoredPosition += vel; // �ړ�
        }
        // �I���ʒu�܂ňړ��������莞�ԑҋ@�㎟�̃V�[����
        else
        {
            waitDelta += Time.deltaTime;
            // ��莞�Ԍo�܂ŏ������Ȃ�
            if (waitDelta < NEXT_WAIT) return;
            if (!isLoading) FadeSceneManager.Instance.LoadScene(clearScene); // �V�[���ړ�
            isLoading = true; // �ǂݍ��݊J�n
        }
    }
}
