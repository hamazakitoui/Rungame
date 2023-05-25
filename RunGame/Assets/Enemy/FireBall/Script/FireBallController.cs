using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour,IEnemy
{
    [Header("�㏸���鑬�x")]
    [SerializeField] float UpPower = 0.4f;
    [Header("�ő�ҋ@����")]
    [SerializeField] float MAXWAITTIME = 3.0f;

    Animator anime;         // �A�j���[�V�����Ǘ�
    float waitPos_y;        // �ҋ@���W
    float waitTime;         // �ҋ@����
    float fallTime;         // ��������
    float gravity;          // ���݂̏d�͒l
    float UpSpeed = 0.0f;         // �W�����v�̑��x
    float UpTime = 0.0f;          // �W�����v���Ă��鎞��
    bool isAscending = false;      // �㏸���邩�̃t���O
    bool isWait = false;           // �ҋ@���邩�̃t���O

    const float GRAVITYACCELERATOR = 0.98f;     // �d�͉����x

    void Start()
    {
        // ���݂̍��W��ҋ@���W�ɐݒ�
        waitPos_y = transform.position.y;

        // �A�j���[�^�[���擾
        anime = GetComponent<Animator>();
    }
    void Update()
    {
        // �㏸�t���O������Ȃ�Ώ������s��
        if (isAscending) { Ascending(); }
        // �ҋ@���łȂ���Η���������s��
        else if(!isWait) { Gravity(); }
        // �ҋ@���̏������s��
        if (isWait)
        {
            // ���Ԃ����Z
            waitTime += Time.deltaTime;
            // �ҋ@���Ԃ��ő�ҋ@���Ԃ𒴂��Ă���Ώ㏸�����Ɉڍs����
            if(waitTime >= MAXWAITTIME)
            {
                // �㏸���x�ɒl��������
                UpSpeed = UpPower;
                // �ҋ@���Ԃ�����������
                waitTime = 0.0f;
                // �t���O��؂�ւ���
                isAscending = true;
                isWait = false;
            }
        }
    }

    // �����̌v�Z
    void Gravity()
    {
        // �A�j���[�^�[�̃t���O��ύX����
        anime.SetBool("isFall", true);

        // �o�ߎ��Ԃ�����
        fallTime += Time.deltaTime;
        // �������x���v�Z����
        gravity = GRAVITYACCELERATOR * fallTime;
        // �����l��K�p����
        transform.position -= new Vector3(0, gravity, 0);

        // �ҋ@���W�ɓ��B�����ꍇ�A�ҋ@��ԂɈڍs���A�l������������
        if(transform.position.y <= waitPos_y) { 
            isWait = true; 
            gravity = 0.0f;
            fallTime = 0.0f;
        }
    }
    // �㏸�̏���
    void Ascending()
    {
        // �A�j���[�^�[�̃t���O��ύX����
        anime.SetBool("isFall", false);

        // ���Ԃ�����
        UpTime = Time.deltaTime;

        // �W�����v�̌v�Z
        UpSpeed = UpSpeed - GRAVITYACCELERATOR * UpTime;

        // ���f������
        transform.position += new Vector3(0, UpSpeed, 0);

        // �㏸���x��0�ȉ��ɂȂ�ƁA�㏸�̏������I����
        if (UpSpeed <= 0.0f) { isAscending = false; UpTime = 0.0f; }
    }
}
