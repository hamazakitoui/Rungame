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
            // プレイヤーと接触したならば扉を開く
            wallManager.OpenWall();
            // 自身の画像を差し替える
            GetComponent<SpriteRenderer>().sprite = ONSwitchImage;
        }
    }
}
