using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform PlayerPos;       // �v���C���[���W
    [Header("�v���C���[�𒆐S�ɍ��W���ǂꂾ�����炷��")]
    [SerializeField] Vector3 correctionPos = new Vector3(5.0f, 0.5f, -10.0f);     // ���W�␳
    [Header("�Œፂ�x")]
    [SerializeField] float minAltitude_Y = -5f;       // �Œፂ�x

    void Update()
    {
        // �v���C���[�̍��W���Œፂ�x����ɂ���Ȃ�ΒǏ]����
        if(PlayerPos.position.y > minAltitude_Y)
        {
            // �v���C���[��Ǐ]����
            transform.position = PlayerPos.position + correctionPos;
        }
    }
}
