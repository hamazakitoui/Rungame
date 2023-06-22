using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWallButton : MonoBehaviour
{
    [SerializeField] OpenWallManager wallManager;
    [SerializeField] Sprite ONSwitchImage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // �v���C���[�ƐڐG�����Ȃ�Δ����J��
            wallManager.OpenWall();
            // ���g�̉摜�������ւ���
            GetComponent<SpriteRenderer>().sprite = ONSwitchImage;
        }
    }
}
