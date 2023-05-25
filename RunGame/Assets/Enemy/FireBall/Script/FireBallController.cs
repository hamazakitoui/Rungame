using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour,IEnemy
{
    [Header("上昇する速度")]
    [SerializeField] float UpPower = 0.4f;
    [Header("最大待機時間")]
    [SerializeField] float MAXWAITTIME = 3.0f;

    Animator anime;         // アニメーション管理
    float waitPos_y;        // 待機座標
    float waitTime;         // 待機時間
    float fallTime;         // 落下時間
    float gravity;          // 現在の重力値
    float UpSpeed = 0.0f;         // ジャンプの速度
    float UpTime = 0.0f;          // ジャンプしている時間
    bool isAscending = false;      // 上昇するかのフラグ
    bool isWait = false;           // 待機するかのフラグ

    const float GRAVITYACCELERATOR = 0.98f;     // 重力加速度

    void Start()
    {
        // 現在の座標を待機座標に設定
        waitPos_y = transform.position.y;

        // アニメーターを取得
        anime = GetComponent<Animator>();
    }
    void Update()
    {
        // 上昇フラグがあるならば処理を行う
        if (isAscending) { Ascending(); }
        // 待機中でなければ落下判定を行う
        else if(!isWait) { Gravity(); }
        // 待機中の処理を行う
        if (isWait)
        {
            // 時間を加算
            waitTime += Time.deltaTime;
            // 待機時間が最大待機時間を超えていれば上昇処理に移行する
            if(waitTime >= MAXWAITTIME)
            {
                // 上昇速度に値を代入する
                UpSpeed = UpPower;
                // 待機時間を初期化する
                waitTime = 0.0f;
                // フラグを切り替える
                isAscending = true;
                isWait = false;
            }
        }
    }

    // 落下の計算
    void Gravity()
    {
        // アニメーターのフラグを変更する
        anime.SetBool("isFall", true);

        // 経過時間を入れる
        fallTime += Time.deltaTime;
        // 落下速度を計算する
        gravity = GRAVITYACCELERATOR * fallTime;
        // 落下値を適用する
        transform.position -= new Vector3(0, gravity, 0);

        // 待機座標に到達した場合、待機状態に移行し、値を初期化する
        if(transform.position.y <= waitPos_y) { 
            isWait = true; 
            gravity = 0.0f;
            fallTime = 0.0f;
        }
    }
    // 上昇の処理
    void Ascending()
    {
        // アニメーターのフラグを変更する
        anime.SetBool("isFall", false);

        // 時間を入れる
        UpTime = Time.deltaTime;

        // ジャンプの計算
        UpSpeed = UpSpeed - GRAVITYACCELERATOR * UpTime;

        // 反映させる
        transform.position += new Vector3(0, UpSpeed, 0);

        // 上昇速度が0以下になると、上昇の処理を終える
        if (UpSpeed <= 0.0f) { isAscending = false; UpTime = 0.0f; }
    }
}
