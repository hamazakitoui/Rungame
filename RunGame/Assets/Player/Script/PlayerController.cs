using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicGirl
{
    public class PlayerController : MonoBehaviour
    {
        [Header("加速アイテムのタグの名前")]
        [SerializeField] string accelerateNameTag;      // 加速アイテムのタグ
        [Header("収集物のタグの名前")]
        [SerializeField] string coinNameTag;            // エキストラアイテムのタグ
        [Header("スコアアイテムのタグの名前")]
        [SerializeField] string scoreNameTag;           // スコアアイテムのタグ
        [Header("接地判定のレイヤー")]
        [SerializeField] LayerMask groundLayer;         // 地面チェック用のレイヤー
        [Header("ジャンプの初速度")]
        [SerializeField] float vec0 = 0.25f;            // ジャンプの初速度
        [Header("大ジャンプの倍率")]
        [SerializeField] float bigJump = 1.35f;         // 大ジャンプのジャンプの倍率
        [Header("走る速度")]
        [SerializeField] float runSpeed = 0.2f;         // 走る速度

        Animator anime;             // アニメーターコンポーネント
        Rigidbody2D rigidbody;      // 物理挙動コンポーネント

        bool isJump = false;        // ジャンプフラグ
        bool isGround = false;      // 接地フラグ
        bool isSpeedUp = false;     // 加速するフラグ
        bool isOverHead = false;    // 頭上に天井があるかのフラグ
        bool isCollision = false;   // 壁に衝突したかのフラグ

        int jumpCount = 0;              // ジャンプ回数
        int speedUpTime = 0;            // 加速継続時間
        float jumpKeyTime = 0.0f;       // ジャンプボタンを押している時間
        float jumpSpeed = 0.0f;         // ジャンプの速度
        float jumpTime = 0.0f;          // ジャンプしている時間
        float gravity = 0.0f;           // 重力値
        float fallTime = 0.0f;          // 落下時間

        const int MAXJUMPCOUNT = 2;     // 最大ジャンプ回数
        const float GRAVITYACCELERATOR = 0.98f;     // 重力加速度
        const float BIGJUMPTIME = 0.2f; // 大ジャンプに必要な判定時間
        const float HEAD = 2.0f;        // 足元から頭までの座標距離

        void Start()
        {
            // コンポーネントを取得
            anime = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();

        }
        void Update()
        {
            // スペースキーを押したらジャンプを行うフラグを建てる
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < MAXJUMPCOUNT)
            {
                // ジャンプのフラグを入れる
                isJump = true;
                // ジャンプのカウントを行う
                jumpCount++;
                // ジャンプに初速度を入れる
                jumpSpeed = vec0;
            }
        }
        void FixedUpdate()
        {
            // 接地しているかの判定を行う
            GroundCheck();
            // 頭上の判定
            OverheadCheck();
            // 壁に衝突判定
            IsWallCheck();
            // 衝突した場合、処理を行う
            if (isCollision) { Dead(); return; }

            // 接地しており、ジャンプ中でなければ、ジャンプの回数をリセットする
            if (isGround && !isJump) { jumpCount = 0; }

            // 条件が一致するならばジャンプをする
            if (isJump) { Jump(); }
            
            // 走る処理
            if (!isCollision) { Run(); }
            

            // 空中におり、ジャンプ中ではないなら落下させる
            //if (!isGround && !isJump)
            //{
            //    Gravity();
            //}
            //else
            //{
            //    // 落下の値の0にする
            //    gravity = 0.0f;
            //    fallTime = 0.0f;
            //}
        }
        // 前に走る処理
        void Run()
        {
            // 速度の倍率
            float mag = 1.0f;
            // 加速中なら速度を2倍にする
            if (isSpeedUp) { mag *= 2.0f; speedUpTime--; }
            // 座標に速度を入れる
            transform.position += new Vector3(runSpeed * mag, 0f, 0f);
            // 加速経過時間が最大継続時間を超えたら加速を終える
            if(speedUpTime <= 0) { isSpeedUp = false; }
        }
        // ジャンプの処理
        void Jump()
        {
            // アニメーターのフラグを変更する
            anime.SetBool("isJump", true);

            // 重力を0にする
            rigidbody.velocity = Vector3.zero;

            // 時間を入れる
            jumpTime = Time.deltaTime;

            // ジャンプの計算
            jumpSpeed = jumpSpeed - GRAVITYACCELERATOR * jumpTime;

            // ジャンプ速度が0以下になるか、頭上にブロックがある場合、ジャンプの処理を終える
            if (jumpSpeed <= 0.0f || isOverHead)
            {
                isJump = false;
                anime.SetBool("isJump", false);
            }
            else
            {
                // 反映させる
                transform.position += new Vector3(0, jumpSpeed, 0);
            }
        }
        // 死亡処理
        void Dead()
        {
            // 死亡アニメーションを再生する
            anime.SetBool("isDead", false);

            // 重力を0にする
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector3.zero;
            
            // ゲームオーバー処理


        }
        // 重力の計算
        void Gravity()
        {
            // 経過時間を入れる
            fallTime += Time.deltaTime;
            // 落下速度を計算する
            gravity = GRAVITYACCELERATOR * fallTime;
            // 落下値を適用する
            transform.position -= new Vector3(0, gravity, 0);
        }
        void GroundCheck()
        {
            // 接地判定
            isGround = Physics2D.Linecast(
            transform.position + transform.up * 0.05f,
            transform.position - transform.up * 0.05f,
            groundLayer
            );
        }
        void OverheadCheck()
        {
            // 頭上判定
            isOverHead = Physics2D.Linecast(
            transform.position + new Vector3(0,HEAD,0) - transform.up * 0.15f,
            transform.position + new Vector3(0, HEAD, 0) + transform.up * 0.15f,
            groundLayer
            );
        }
        void IsWallCheck()
        {
            // 進行方向に壁があるか判定
            isCollision = Physics2D.Linecast(
            transform.position + new Vector3(0.25f * transform.localScale.x, 1f, 0f),
            transform.position + new Vector3(0.45f * transform.localScale.x, 1f, 0f),
            groundLayer
            );
            // レイを表示してみる
            Debug.DrawLine(
            transform.position + new Vector3(0.25f * transform.localScale.x, 1f, 0f),
            transform.position + new Vector3(0.45f * transform.localScale.x, 1f, 0f),
            Color.red);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            // アイテムに接触したかの判定
            if (collision.gameObject.GetComponent<ItemData>() != null)
            {
                // どのアイテムに接触したか識別する
                switch (collision.gameObject.GetComponent<ItemData>().GetItemKinds)
                {
                    case _ItemKinds.ScoreItem:
                        // UIに反映する

                        break;
                    case _ItemKinds.Coin:
                        // UIに反映する

                        break;
                    case _ItemKinds.Accelerator:
                        // 一定時間加速する
                        isSpeedUp = true;
                        speedUpTime = collision.gameObject.GetComponent<ItemData>().GetValue;
                        break;
                }
            }
        }
    }
}