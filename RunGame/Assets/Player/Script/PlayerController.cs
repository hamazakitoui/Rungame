using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("接地判定のレイヤー")]
    [SerializeField] LayerMask groundLayer;         // 地面チェック用のレイヤー
    [Header("ジャンプの初速度")]
    [SerializeField] float vec0 = 0.25f;            // ジャンプの初速度
    [Header("走る速度")]
    [SerializeField] float runSpeed = 0.2f;         // 走る速度
    [Header("落下死亡判定座標")]
    [SerializeField] float deadLine_y = -5f;        // 落下死亡判定の座標

    [SerializeField] GameOverManager gameOverManager;// ゲームオーバーUI管理
    [SerializeField] ScoreManager scoreManager;     // スコア表示管理
    [SerializeField] CollectiblesUI collectiblesUI; // 収集物入手管理
    Animator anime;             // アニメーターコンポーネント
    Rigidbody2D rigidbody;      // 物理挙動コンポーネント

    bool isJump = false;        // ジャンプフラグ
    bool isGround = false;      // 接地フラグ
    bool isSpeedUp = false;     // 加速するフラグ
    bool isOverHead = false;    // 頭上に天井があるかのフラグ
    bool isCollision = false;   // 壁に衝突したかのフラグ

    int jumpCount = 0;              // ジャンプ回数
    int speedUpTime = 0;            // 加速継続時間
    float jumpSpeed = 0.0f;         // ジャンプの速度
    float jumpTime = 0.0f;          // ジャンプしている時間

    const int MAXJUMPCOUNT = 2;     // 最大ジャンプ回数
    const float GRAVITYACCELERATOR = 0.98f;     // 重力加速度
    const float HEAD = 2.0f;        // 足元から頭までの座標距離

    public bool isMove { get; set; }    // 行動可能かのフラグ
    public bool isDead { get; set; }    // 死亡したかのフラグ
    public bool isStageClear { get; set; }   // クリアしたかのフラグ

    void Start()
    {
        // コンポーネントを取得
        anime = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        isMove = true;
    }
    void Update()
    {
        // 行動禁止の場合、処理しない
        if (!isMove) return;

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
        // 行動禁止の場合、処理しない
        if (!isMove) return;

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

        // 自身の現在のY座標が死亡ラインの座標以下ならば死亡処理を行う
        if (transform.position.y < deadLine_y) { Dead(); }

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
        if (speedUpTime <= 0) { isSpeedUp = false; }
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
        anime.SetBool("isDead", true);

        // 重力を0にする
        rigidbody.gravityScale = 0;
        rigidbody.velocity = Vector3.zero;

        // ゲームオーバーのUIの処理を開始する
        gameOverManager.GameOver();
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
        transform.position + new Vector3(0, HEAD, 0) - transform.up * 0.15f,
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
                    scoreManager.SetScore(collision.gameObject.GetComponent<ItemData>().GetValue);
                    break;
                case _ItemKinds.Collectibles:
                    // UIに反映する
                    collectiblesUI.SetCollectibles(collision.gameObject.GetComponent<ItemData>().GetValue);
                    break;
                case _ItemKinds.Accelerator:
                    // 一定時間加速する
                    isSpeedUp = true;
                    speedUpTime = collision.gameObject.GetComponent<ItemData>().GetValue;
                    break;
            }
        }

        // 敵と接触したかの判定
        if(collision.gameObject.GetComponent<IEnemy>() != null)
        {
            // 接触しているならば死亡処理に移る
            Dead();
        }
    }
}