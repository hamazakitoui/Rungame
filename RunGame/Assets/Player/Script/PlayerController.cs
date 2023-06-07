using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�ڒn����̃��C���[")]
    [SerializeField] LayerMask groundLayer;         // �n�ʃ`�F�b�N�p�̃��C���[
    [Header("�W�����v�̏����x")]
    [SerializeField] float vec0 = 0.25f;            // �W�����v�̏����x
    [Header("���鑬�x")]
    [SerializeField] float runSpeed = 0.2f;         // ���鑬�x
    [Header("�������S������W")]
    [SerializeField] float deadLine_y = -5f;        // �������S����̍��W

    [SerializeField] GameOverManager gameOverManager;// �Q�[���I�[�o�[UI�Ǘ�
    [SerializeField] ScoreManager scoreManager;     // �X�R�A�\���Ǘ�
    [SerializeField] CollectiblesUI collectiblesUI; // ���W������Ǘ�
    Animator anime;             // �A�j���[�^�[�R���|�[�l���g
    Rigidbody2D rigidbody;      // ���������R���|�[�l���g

    bool isJump = false;        // �W�����v�t���O
    bool isGround = false;      // �ڒn�t���O
    bool isSpeedUp = false;     // ��������t���O
    bool isOverHead = false;    // ����ɓV�䂪���邩�̃t���O
    bool isCollision = false;   // �ǂɏՓ˂������̃t���O

    int jumpCount = 0;              // �W�����v��
    int speedUpTime = 0;            // �����p������
    float jumpSpeed = 0.0f;         // �W�����v�̑��x
    float jumpTime = 0.0f;          // �W�����v���Ă��鎞��

    const int MAXJUMPCOUNT = 2;     // �ő�W�����v��
    const float GRAVITYACCELERATOR = 0.98f;     // �d�͉����x
    const float HEAD = 2.0f;        // �������瓪�܂ł̍��W����

    public bool isMove { get; set; }    // �s���\���̃t���O
    public bool isDead { get; set; }    // ���S�������̃t���O
    public bool isStageClear { get; set; }   // �N���A�������̃t���O

    void Start()
    {
        // �R���|�[�l���g���擾
        anime = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        isMove = true;
    }
    void Update()
    {
        // �s���֎~�̏ꍇ�A�������Ȃ�
        if (!isMove) return;

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
        // �s���֎~�̏ꍇ�A�������Ȃ�
        if (!isMove) return;

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

        // ���g�̌��݂�Y���W�����S���C���̍��W�ȉ��Ȃ�Ύ��S�������s��
        if (transform.position.y < deadLine_y) { Dead(); }

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
        if (speedUpTime <= 0) { isSpeedUp = false; }
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
        anime.SetBool("isDead", true);

        // �d�͂�0�ɂ���
        rigidbody.gravityScale = 0;
        rigidbody.velocity = Vector3.zero;

        // �Q�[���I�[�o�[��UI�̏������J�n����
        gameOverManager.GameOver();
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
        transform.position + new Vector3(0, HEAD, 0) - transform.up * 0.15f,
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
                    scoreManager.SetScore(collision.gameObject.GetComponent<ItemData>().GetValue);
                    break;
                case _ItemKinds.Collectibles:
                    // UI�ɔ��f����
                    collectiblesUI.SetCollectibles(collision.gameObject.GetComponent<ItemData>().GetValue);
                    break;
                case _ItemKinds.Accelerator:
                    // ��莞�ԉ�������
                    isSpeedUp = true;
                    speedUpTime = collision.gameObject.GetComponent<ItemData>().GetValue;
                    break;
            }
        }

        // �G�ƐڐG�������̔���
        if(collision.gameObject.GetComponent<IEnemy>() != null)
        {
            // �ڐG���Ă���Ȃ�Ύ��S�����Ɉڂ�
            Dead();
        }
    }
}