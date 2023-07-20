using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [Header("プレイヤー")] [SerializeField] private PlayerController pl;

    [Header("カウントダウンスピード")] [SerializeField] private int countDownSpeed = 120;
    [Header("カウント")] [SerializeField] private float startCount = 3;
    [Header("テキストサイズ加算数")] [SerializeField] private int size = 1;
    [Header("カウントダウンSE")] [SerializeField] private AudioClip countDownSE;
    [Header("開始SE")] [SerializeField] private AudioClip GoSE;
    [Header("BGM")] [SerializeField] private AudioClip BGM;
    AudioSource audio;
    private int startSize;//初期フォントサイズ
    private Text countDownText;//カウントダウン用テキスト

    [Header("スタートキャンバス")] [SerializeField] private Canvas startCanvas;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        //スタートのカウントダウン
        StartCoroutine(StartGame());

    }

    public IEnumerator StartGame()
    {
        //プレイヤーを動かないようにする
        //pl.isMove = false;

        //アクティブ状態にする
        startCanvas.gameObject.SetActive(true);

        //開始時のテキストサイズを取得
        countDownText = startCanvas.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        startSize = countDownText.fontSize;

        int oneBefore = 0;  //カウントダウン判定用変数
        bool isFast = false;//初回処理フラグ

        //CanvasをONにする
        startCanvas.enabled = true;

        //端数切捨てのため加算
        startCount++;

        //カウント待機
        while (true)
        {
            //カウントダウン
            startCount -= 1f / countDownSpeed;

            //秒数判定
            if (startCount <= 1)
            {
                //テキスト変更
                countDownText.text = "GO!";

                if (!isFast)
                {
                    isFast = true;

                    //テキストを初期状態に戻す
                    countDownText.fontSize = startSize;

                    //SE判定
                    if (audio.clip != null && GoSE != null)
                    {
                        audio.clip = GoSE;
                        audio.Play();
                    }

                }
            }
            else if ((int)startCount != oneBefore)
            {
                //テキスト変更
                countDownText.text = ((int)startCount).ToString();

                //数を揃える
                oneBefore = (int)startCount;

                //テキストを初期状態に戻す
                countDownText.fontSize = startSize;
                audio.clip = countDownSE;
                audio.Play();
            }

            //テキストを大きくする
            countDownText.fontSize += size;

            //終了判定
            if (startCount <= 0)
            {
                break;
            }
            yield return null;
        }

        //CanvasをOFFにする
        startCanvas.enabled = false;

        //プレイヤーを動かす
        //pl.isMove = true;
    }
}
