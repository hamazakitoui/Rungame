using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerPos;           // プレイヤー座標
    [SerializeField] PlayerController player;       // プレイヤー情報
    [Header("目的地のY座標")]
    [SerializeField] Vector2[] routeVec;            // 目的地座標
    [Header("カメラの速度")]
    [SerializeField] float speed = 0.1f;                   // カメラ速度

    [Header("プレイヤーを中心に座標をどれだけずらすか")]
    [SerializeField] Vector3 correctionPos = new Vector3(5.0f, 0.5f, -10.0f);     // 座標補正
    //[Header("最低高度")]
    //[SerializeField] float minAltitude_Y = -5f;       // 最低高度
    CameraMove cameraMove;                              // カメラ移動先
    int routeNumber = 0;                              　// 目的地座標の現在番号

    public bool isProcess { get; set; }                 // 処理を行うか 

    enum CameraMove
    {
        Up,
        Down,
    }
    private void Start()
    {
        // カメラの座標をずらす
        transform.position = correctionPos;
       
        // 目的地に向けて上か下に行くかを決定する
        if (transform.position.y < routeVec[routeNumber].y) cameraMove = CameraMove.Up;
        else cameraMove = CameraMove.Down;

        isProcess = true;
    }

    void Update()
    {
        // 処理を行わない場合、returnを返す
        if (!isProcess) return;

        // プレイヤーが死亡したか、ステージクリアした場合、カメラを止める
        if (player.isDead || player.isDead) return;

        // プレイヤーを追従しつつ目的座標へ進む
        if(transform.position.y < routeVec[routeNumber].y && cameraMove == CameraMove.Up)
        {
            // 上に移動
            transform.position = new Vector3(playerPos.position.x + correctionPos.x, transform.position.y + speed, correctionPos.z);
        }
        if(transform.position.y > routeVec[routeNumber].y && cameraMove == CameraMove.Down)
        {
            // 下へ移動
            transform.position = new Vector3(playerPos.position.x + correctionPos.x, transform.position.y - speed, correctionPos.z);
        }
        if(transform.position.x >= routeVec[routeNumber].x)
        {
            // 目的地点に到達した場合、次の目的地に進む
            if (routeNumber + 1 < routeVec.Length) routeNumber++;
            else return;
            // 目的地に向けて上か下に行くかを決定する
            if (transform.position.y < routeVec[routeNumber].y) cameraMove = CameraMove.Up;
            else cameraMove = CameraMove.Down;
        }
        // X座標はプレイヤーを追従
        transform.position = new Vector3(playerPos.position.x + correctionPos.x, transform.position.y, transform.position.z);
    }
}
