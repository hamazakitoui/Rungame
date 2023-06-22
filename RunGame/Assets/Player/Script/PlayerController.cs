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
    [Header("SEのデータ")]
    [SerializeField] PlayerSEData seData;           // 効果音データ
    [Header("二段目のジャンプのエフェクト")]
    [SerializeField] ParticleSystem JumpSmoke;      // ジャンプ時の土煙
    [Header("各UIスクリプト")]
    [SerializeField] GameOverManager gameOverManager;// ゲームオーバーUI管理
    [SerializeField] ScoreManager scoreManager;     // スコア表示管理
    [SerializeField] CollectiblesUI collectiblesUI; // 収集物入手管理
    Animator anime;             // アニメーターコンポーネント
    new Rigidbody2D rigidbody;  // 物理挙動コンポーネント
    new AudioSource audio;      // 音声コンポーネント

    ParticleSystem dustCloudEffect; // 土煙エフェクト
    PlayerAfterimage afterimage;    // 残像エフェクト

    bool isJump = false;        // ジャンプフラグ
    bool isGround = false;      // 接地フラグ
    bool isSpeedUp = false;     // 加速するフラグ
    bool isOverHead = false;    // 頭上に天井があるかのフラグ
    bool isCollision = false;   // 壁に衝突したかのフラグ
    bool isJumpRamp = false;    // ジャンプ台を踏んだかのフラグ
    bool isDeathCollision = false;  // ぶつかって倒れたかのフラグ

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
        audio = GetComponent<AudioSource>();
        // エフェクトを取得
        dustCloudEffect = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        // スクリプトを取得
        afterimage = transform.GetChild(1).gameObject.GetComponent<PlayerAfterimage>();

        isMove = true;
    }
    void Update()
    {
        // 行動禁止の場合、処理しない
        if (!isMove || isDead) return;

        // スペースキーを押したらジャンプを行うフラグを建てる
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < MAXJUMPCOUNT)
        {
            // ジャンプのフラグを入れる
            isJump = true;
            // ジャンプのカウントを行う
            jumpCount++;
            // ジャンプに初速度を入れる
            jumpSpeed = vec0;
            // 二段ジャンプ目に土煙を出す
            if(jumpCount >= MAXJUMPCOUNT) { Instantiate(JumpSmoke, transform.position, Quaternion.identity); }
        }
    }
    void FixedUpdate()
    {
        // 行動禁止の場合、処理しない
        if (!isMove || isDead) return;

        // 接地しているかの判定を行う
        GroundCheck();
        // 頭上の判定
        OverheadCheck();
        // 壁に衝突判定
        IsWallCheck();
        // 衝突した場合、処理を行う
        if (isCollision && !isDead) { isDeathCollision = true; isDead = true; Dead(); return; }

        // 接地しており、ジャンプ中でなければ、ジャンプの回数をリセットする
        if (isGround && !isJump) { jumpCount = 0; }

        // 地面に接地していないならばエフェクトを非表示にする、
        if (!isGround) { dustCloudEffect.Stop(false); }

        // 接地してなくジャンプ中でないならば落下モーションに変更、接地しているならば走るモーションに変更にする
        if (!isGround && !isJump) { anime.SetBool("isJumpDown", true); }
        else anime.SetBool("isJumpDown", false);

        // 条件が一致するならばジャンプをする
        if (isJump) { Jump(); }

        // 走る処理
        if (!isCollision) { Run(); }

        // 自身の現在のY座標が死亡ラインの座標以下ならば死亡処理を行う
        if (transform.position.y < deadLine_y) { isDead = true; Dead(); }

        // ステージクリアした場合、クリアモーションへ移行し停止する
        if (isStageClear) { 
            isMove = false; 
            // アニメーターのフラグを変更する
            anime.SetBool("isClear", true);
        }
    }
    // 前に走る処理
    void Run()
    {
        // アニメーターのフラグを変更する
        anime.SetBool("isRun", true);
        // エフェクトを表示
        if (isGround) { dustCloudEffect.Play(); }
        // 速度の倍率
        float mag = 1.0f;
        // ジャンプ台によるジャンプ中なら速度を半分にする
        if (isJumpRamp) { mag /= 2.0f; }
        // 加速中なら速度を2倍にする
        if (isSpeedUp) { mag *= 2.0f; speedUpTime--; }
        // 座標に速度を入れる
        transform.position += new Vector3(runSpeed * mag, 0f, 0f);
        // 加速経過時間が最大継続時間を超えたら加速を終える
        if (speedUpTime <= 0) { isSpeedUp = false; afterimage.EndGenerator(); }
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
            isJumpRamp = false;
            anime.SetBool("isJump", false);
            // スピードアップ中でなければジャンプ台によるジャンプの残像の生成を切る
            if(!isSpeedUp) afterimage.EndGenerator();
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
        // 音データがあるかをチェック
        if(seData.GetDeadSE != null)
        {
            // 死亡効果音を鳴らす
            audio.clip = seData.GetDeadSE;
            audio.Play();
        }
        // 死亡アニメーションを再生する
        anime.SetBool("isDead", true);
        // エフェクトを非表示
        dustCloudEffect.Stop(false);
        // コライダーをOFFにする
        this.GetComponent<CapsuleCollider2D>().enabled = false;

        // 衝突して倒れた場合、後方へ吹っ飛ばす
        if (isDeathCollision) { StartCoroutine("DeadFlyAway"); }

        // ゲームオーバーのUIの処理を開始する
        gameOverManager.GameOver();
    }

    IEnumerator DeadFlyAway()
    {
        // 回転値
        Vector3 rotation = new Vector3(0, 0, 0);

        // 回転禁止を解く
        rigidbody.freezeRotation = false;

        while (true)
        {
            // 回転値に加算する
            rotation += new Vector3(0, 0, runSpeed);
            // 回転させる
            transform.Rotate(rotation);
            // 重力を0にする
            rigidbody.velocity = Vector3.zero;
            // 速度を入れる
            jumpSpeed = vec0;
            // 後方へ飛ぶ
            transform.position += new Vector3(-jumpSpeed, jumpSpeed, 0);
            // 一瞬停止
            yield return null;
        }
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
        // 進行方向に壁があるか判定（頭）
        isCollision = Physics2D.Linecast(
        transform.position + new Vector3(0.3f * transform.localScale.x, 1.15f, 0f),
        transform.position + new Vector3(0.5f * transform.localScale.x, 1.15f, 0f),
        groundLayer
        );
        // レイを表示してみる
        Debug.DrawLine(
        transform.position + new Vector3(0.3f * transform.localScale.x, 1.15f, 0f),
        transform.position + new Vector3(0.5f * transform.localScale.x, 1.15f, 0f),
        Color.red);

        // 進行方向に壁があるか判定(足元）
        isCollision = Physics2D.Linecast(
        transform.position + new Vector3(0.3f * transform.localScale.x, 0.25f, 0f),
        transform.position + new Vector3(0.5f * transform.localScale.x, 0.25f, 0f),
        groundLayer
        );
        // レイを表示してみる
        Debug.DrawLine(
        transform.position + new Vector3(0.3f * transform.localScale.x, 0.25f, 0f),
        transform.position + new Vector3(0.5f * transform.localScale.x, 0.25f, 0f),
        Color.green);
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
                    scoreManager.SetScore((int)collision.gameObject.GetComponent<ItemData>().GetValue);
                    // 音データがあるかをチェック
                    if (seData.GetScoreSE != null)
                    {
                        // スコア入手効果音を鳴らす
                        audio.clip = seData.GetScoreSE;
                        audio.Play();
                    }
                    // 接触したアイテムを削除
                    Destroy(collision.gameObject);
                    break;
                case _ItemKinds.Collectibles:
                    // UIに反映する
                    collectiblesUI.SetCollectibles((int)collision.gameObject.GetComponent<ItemData>().GetValue);
                    // 音データがあるかをチェック
                    if (seData.GetCollectiblesSE != null)
                    {
                        // 死亡効果音を鳴らす
                        audio.clip = seData.GetCollectiblesSE;
                        audio.Play();
                    }
                    // 接触したアイテムを削除
                    Destroy(collision.gameObject);
                    break;
                case _ItemKinds.Accelerator:
                    // 一定時間加速する
                    isSpeedUp = true;
                    speedUpTime = (int)collision.gameObject.GetComponent<ItemData>().GetValue;
                    // 残像を生成する
                    afterimage.StartGenerator(transform, GetComponent<SpriteRenderer>());
                    // 音データがあるかをチェック
                    if (seData.GetAcceleratorSE != null)
                    {
                        // 加速効果音を鳴らす
                        audio.clip = seData.GetAcceleratorSE;
                        audio.Play();
                    }
                    // 接触したアイテムを削除
                    Destroy(collision.gameObject);
                    break;
                case _ItemKinds.JumpRamp:
                    // 即時ジャンプする
                    jumpSpeed = collision.gameObject.GetComponent<ItemData>().GetValue;
                    isJumpRamp = true;
                    isJump = true;
                    // 残像を生成する
                    afterimage.StartGenerator(transform, GetComponent<SpriteRenderer>());
                    // 音データがあるかをチェック
                    if (seData.GetJumpRampSE != null)
                    {
                        // ジャンプ台効果音を鳴らす
                        audio.clip = seData.GetJumpRampSE;
                        audio.Play();
                    }
                    break;
            }
        }

        // 敵と接触したかの判定
        if (collision.gameObject.GetComponent<IEnemy>() != null && !isDead)
        {
            // 接触しているならば死亡処理に移る
            isDead = true;
            Dead();
        }

        // ゴールに接触したかの判定
        if(collision.gameObject.GetComponent<GoalPoint>() != null && !isDead)
        {
            // 接触しているならばクリアフラグを立てる
            collision.gameObject.GetComponent<GoalPoint>().GameClear();
            isMove = false;
            anime.SetBool("isClear", true);

            // エフェクトを非表示
            dustCloudEffect.Stop(false);
        }
    }
}