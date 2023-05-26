using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> エンディング </summary>
public class EndingDirector : MonoBehaviour
{
    float waitDelta = 0.0f; // 待機経過時間
    bool isLoading = false; // 読み込み中フラグ
    // 次のシーンまでの待機時間
    const float NEXT_WAIT = 3f;
    // スタッフロール移動速度、スタッフロール初期Y座標、スタッフロール終了Y座標
    [SerializeField] float staffRollSpeed = 2.5f, srStartY = 0, srEndY = 10;
    // スタッフロールRectTransformプロパティ
    [Header("スタッフロール")] [SerializeField] RectTransform staffRollRect;
    [Header("クリアシーン")] [SerializeField] SceneObject clearScene; // クリアシーン
    // Start is called before the first frame update
    void Start()
    {
        Vector3 srPos = Vector3.zero; // スタッフロールの初期位置
        srPos.y = srStartY; // Y座標だけ初期化
        staffRollRect.anchoredPosition = srPos; // 位置初期化
    }

    // Update is called once per frame
    void Update()
    {
        StaffRollMove(); // スタッフロール
    }
    /// <summary> スタッフロール </summary>
    void StaffRollMove()
    {
        // 終了位置まで移動するまで移動
        if (staffRollRect.anchoredPosition.y < srEndY)
        {
            Vector2 vel = new Vector2(0, staffRollSpeed * Time.deltaTime); // 移動速度
            staffRollRect.anchoredPosition += vel; // 移動
        }
        // 終了位置まで移動したら一定時間待機後次のシーンへ
        else
        {
            waitDelta += Time.deltaTime;
            // 一定時間経つまで処理しない
            if (waitDelta < NEXT_WAIT) return;
            if (!isLoading) FadeSceneManager.Instance.LoadScene(clearScene); // シーン移動
            isLoading = true; // 読み込み開始
        }
    }
}
