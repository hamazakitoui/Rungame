using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicGirl
{
    public class PlayerController : MonoBehaviour
    {
        [Header("�����A�C�e���̃^�O�̖��O")]
        [SerializeField] string accelerateNameTag;      // �����A�C�e���̃^�O
        [Header("���W���̃^�O�̖��O")]
        [SerializeField] string coinNameTag;            // �G�L�X�g���A�C�e���̃^�O
        [Header("�X�R�A�A�C�e���̃^�O�̖��O")]
        [SerializeField] string scoreNameTag;           // �X�R�A�A�C�e���̃^�O
        [Header("�ڒn����̃��C���[")]
        [SerializeField] LayerMask groundLayer;         // �n�ʃ`�F�b�N�p�̃��C���[
        [Header("�W�����v�̏����x")]
        [SerializeField] float vec0 = 0.25f;            // �W�����v�̏����x
        [Header("��W�����v�̔{��")]
        [SerializeField] float bigJump = 1.35f;         // ��W�����v�̃W�����v�̔{��
        [Header("���鑬�x")]
        [SerializeField] float runSpeed = 0.2f;         // ���鑬�x

        Animator anime;             // �A�j���[�^�[�R���|�[�l���g
        Rigidbody2D rigidbody;      // ���������R���|�[�l���g

        bool isJump = false;        // �W�����v�t���O
        bool isGround = false;      // �ڒn�t���O
        bool isSpeedUp = false;     // ��������t���O
        bool isOverHead = false;    // ����ɓV�䂪���邩�̃t���O
        bool isCollision = false;   // �ǂɏՓ˂������̃t���O

        int jumpCount = 0;              // �W�����v��
        int speedUpTime = 0;            // �����p������
        float jumpKeyTime = 0.0f;       // �W�����v�{�^���������Ă��鎞��
        float jumpSpeed = 0.0f;         // �W�����v�̑��x
        float jumpTime = 0.0f;          // �W�����v���Ă��鎞��
        float gravity = 0.0f;           // �d�͒l
        float fallTime = 0.0f;          // ��������

        const int MAXJUMPCOUNT = 2;     // �ő�W�����v��
        const float GRAVITYACCELERATOR = 0.98f;     // �d�͉����x
        const float BIGJUMPTIME = 0.2f; // ��W�����v�ɕK�v�Ȕ��莞��
        const float HEAD = 2.0f;        // �������瓪�܂ł̍��W����

        void Start()
        {
            // �R���|�[�l���g���擾
            anime = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();

        }
        void Update()
        {
            // �X�y�[�X�L�[����������W�����v���s���t���O�����Ă�
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < MAXJUMPCOUNT)
            {
                // �W�����v�̃t���O������
                isJump = true;
                // �W�����v�̃J�E���g���s��
                jumpCount++;
                // �W�����v�ɏ����x������
                jumpSpeed = vec0;
            }
        }
        void FixedUpdate()
        {
            // �ڒn���Ă��邩�̔�����s��
            GroundCheck();
            // ����̔���
            OverheadCheck();
            // �ǂɏՓ˔���
            IsWallCheck();
            // �Փ˂����ꍇ�A�������s��
            if (isCollision) { Dead(); return; }

            // �ڒn���Ă���A�W�����v���łȂ���΁A�W�����v�̉񐔂����Z�b�g����
            if (isGround && !isJump) { jumpCount = 0; }

            // ��������v����Ȃ�΃W�����v������
            if (isJump) { Jump(); }
            
            // ���鏈��
            if (!isCollision) { Run(); }
            

            // �󒆂ɂ���A�W�����v���ł͂Ȃ��Ȃ痎��������
            //if (!isGround && !isJump)
            //{
            //    Gravity();
            //}
            //else
            //{
            //    // �����̒l��0�ɂ���
            //    gravity = 0.0f;
            //    fallTime = 0.0f;
            //}
        }
        // �O�ɑ��鏈��
        void Run()
        {
            // ���x�̔{��
            float mag = 1.0f;
            // �������Ȃ瑬�x��2�{�ɂ���
            if (isSpeedUp) { mag *= 2.0f; speedUpTime--; }
            // ���W�ɑ��x������
            transform.position += new Vector3(runSpeed * mag, 0f, 0f);
            // �����o�ߎ��Ԃ��ő�p�����Ԃ𒴂�����������I����
            if(speedUpTime <= 0) { isSpeedUp = false; }
        }
        // �W�����v�̏���
        void Jump()
        {
            // �A�j���[�^�[�̃t���O��ύX����
            anime.SetBool("isJump", true);

            // �d�͂�0�ɂ���
            rigidbody.velocity = Vector3.zero;

            // ���Ԃ�����
            jumpTime = Time.deltaTime;

            // �W�����v�̌v�Z
            jumpSpeed = jumpSpeed - GRAVITYACCELERATOR * jumpTime;

            // �W�����v���x��0�ȉ��ɂȂ邩�A����Ƀu���b�N������ꍇ�A�W�����v�̏������I����
            if (jumpSpeed <= 0.0f || isOverHead)
            {
                isJump = false;
                anime.SetBool("isJump", false);
            }
            else
            {
                // ���f������
                transform.position += new Vector3(0, jumpSpeed, 0);
            }
        }
        // ���S����
        void Dead()
        {
            // ���S�A�j���[�V�������Đ�����
            anime.SetBool("isDead", false);

            // �d�͂�0�ɂ���
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
            
            // �Q�[���I�[�o�[����


        }
        // �d�͂̌v�Z
        void Gravity()
        {
            // �o�ߎ��Ԃ�����
            fallTime += Time.deltaTime;
            // �������x���v�Z����
            gravity = GRAVITYACCELERATOR * fallTime;
            // �����l��K�p����
            transform.position -= new Vector3(0, gravity, 0);
        }
        void GroundCheck()
        {
            // �ڒn����
            isGround = Physics2D.Linecast(
            transform.position + transform.up * 0.05f,
            transform.position - transform.up * 0.05f,
            groundLayer
            );
        }
        void OverheadCheck()
        {
            // ���㔻��
            isOverHead = Physics2D.Linecast(
            transform.position + new Vector3(0,HEAD,0) - transform.up * 0.15f,
            transform.position + new Vector3(0, HEAD, 0) + transform.up * 0.15f,
            groundLayer
            );
        }
        void IsWallCheck()
        {
            // �i�s�����ɕǂ����邩����
            isCollision = Physics2D.Linecast(
            transform.position + new Vector3(0.25f * transform.localScale.x, 1f, 0f),
            transform.position + new Vector3(0.45f * transform.localScale.x, 1f, 0f),
            groundLayer
            );
            // ���C��\�����Ă݂�
            Debug.DrawLine(
            transform.position + new Vector3(0.25f * transform.localScale.x, 1f, 0f),
            transform.position + new Vector3(0.45f * transform.localScale.x, 1f, 0f),
            Color.red);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            // �A�C�e���ɐڐG�������̔���
            if (collision.gameObject.GetComponent<ItemData>() != null)
            {
                // �ǂ̃A�C�e���ɐڐG���������ʂ���
                switch (collision.gameObject.GetComponent<ItemData>().GetItemKinds)
                {
                    case _ItemKinds.ScoreItem:
                        // UI�ɔ��f����

                        break;
                    case _ItemKinds.Coin:
                        // UI�ɔ��f����

                        break;
                    case _ItemKinds.Accelerator:
                        // ��莞�ԉ�������
                        isSpeedUp = true;
                        speedUpTime = collision.gameObject.GetComponent<ItemData>().GetValue;
                        break;
                }
            }
        }
    }
}