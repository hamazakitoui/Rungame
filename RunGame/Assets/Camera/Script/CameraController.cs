using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform PlayerPos;       // プレイヤー座標
    [Header("プレイヤーを中心に座標をどれだけずらすか")]
    [SerializeField] Vector3 correctionPos = new Vector3(5.0f, 0.5f, -10.0f);     // 座標補正
    [Header("最低高度")]
    [SerializeField] float minAltitude_Y = -5f;       // 最低高度

    void Update()
    {
        // プレイヤーの座標が最低高度より上にあるならば追従する
        if(PlayerPos.position.y > minAltitude_Y)
        {
            // プレイヤーを追従する
            transform.position = PlayerPos.position + correctionPos;
        }
    }
}
