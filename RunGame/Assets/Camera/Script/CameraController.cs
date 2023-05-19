using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerPos;           // �v���C���[���W
    [SerializeField] PlayerController player;       // �v���C���[���
    [Header("�ړI�n��Y���W")]
    [SerializeField] Vector2[] routeVec;            // �ړI�n���W
    [Header("�J�����̑��x")]
    [SerializeField] float speed = 0.1f;                   // �J�������x

    [Header("�v���C���[�𒆐S�ɍ��W���ǂꂾ�����炷��")]
    [SerializeField] Vector3 correctionPos = new Vector3(5.0f, 0.5f, -10.0f);     // ���W�␳
    //[Header("�Œፂ�x")]
    //[SerializeField] float minAltitude_Y = -5f;       // �Œፂ�x
    CameraMove cameraMove;                              // �J�����ړ���
    int routeNumber = 0;                              �@// �ړI�n���W�̌��ݔԍ�

    public bool isProcess { get; set; }                 // �������s���� 

    enum CameraMove
    {
        Up,
        Down,
    }
    private void Start()
    {
        // �J�����̍��W�����炷
        transform.position = correctionPos;
       
        // �ړI�n�Ɍ����ďォ���ɍs���������肷��
        if (transform.position.y < routeVec[routeNumber].y) cameraMove = CameraMove.Up;
        else cameraMove = CameraMove.Down;

        isProcess = true;
    }

    void Update()
    {
        // �������s��Ȃ��ꍇ�Areturn��Ԃ�
        if (!isProcess) return;

        // �v���C���[�����S�������A�X�e�[�W�N���A�����ꍇ�A�J�������~�߂�
        if (player.isDead || player.isDead) return;

        // �v���C���[��Ǐ]���ړI���W�֐i��
        if(transform.position.y < routeVec[routeNumber].y && cameraMove == CameraMove.Up)
        {
            // ��Ɉړ�
            transform.position = new Vector3(playerPos.position.x + correctionPos.x, transform.position.y + speed, correctionPos.z);
        }
        if(transform.position.y > routeVec[routeNumber].y && cameraMove == CameraMove.Down)
        {
            // ���ֈړ�
            transform.position = new Vector3(playerPos.position.x + correctionPos.x, transform.position.y - speed, correctionPos.z);
        }
        if(transform.position.x >= routeVec[routeNumber].x)
        {
            // �ړI�n�_�ɓ��B�����ꍇ�A���̖ړI�n�ɐi��
            if (routeNumber + 1 < routeVec.Length) routeNumber++;
            else return;
            // �ړI�n�Ɍ����ďォ���ɍs���������肷��
            if (transform.position.y < routeVec[routeNumber].y) cameraMove = CameraMove.Up;
            else cameraMove = CameraMove.Down;
        }
        // X���W�̓v���C���[��Ǐ]
        transform.position = new Vector3(playerPos.position.x + correctionPos.x, transform.position.y, transform.position.z);
    }
}
