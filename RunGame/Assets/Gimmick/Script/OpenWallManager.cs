using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWallManager : MonoBehaviour
{
    [Header("�����J�����x")]
    [SerializeField] float openSpeed = 0.02f;
    
    // ���I�u�W�F�N�g
    Transform door = null;

    // �J���ۂ̔��̖ړI���W
    float open_y = 0.51f;

    private void Start()
    {
        // �����擾
        door = transform.GetChild(0).gameObject.transform;
    }

    public void OpenWall()
    {
        // ���J���������s��
        StartCoroutine("OpenProcess");
    }
    IEnumerator OpenProcess()
    {
        while (true)
        {
            // ������ɏ��X�ɏグ��
            door.localPosition += new Vector3(0.0f, openSpeed, 0.0f);

            // ���̍��W���ړI���W�ɓ��B�����Ȃ�΁A��~����
            if (door.localPosition.y >= open_y) { break; }

            // 1�t���[���~�߂�
            yield return null;
        }
    }
}
