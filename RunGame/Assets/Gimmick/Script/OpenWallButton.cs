using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWallButton : MonoBehaviour
{
    [SerializeField] OpenWallManager wallManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // プレイヤーと接触したならば扉を開く
            wallManager.OpenWall();
        }
    }
}
