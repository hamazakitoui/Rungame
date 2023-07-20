using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    [SerializeField] RectTransform bgmIconPos;
    [SerializeField] RectTransform seIconPos;
    [SerializeField] RectTransform selectIcon;
    [SerializeField] TitleManager titleManager;
    [SerializeField] UISEData uISE;

    AudioManager audioManager;

    int select = 0;         // 選択参照値
    bool isProcess = false;

    // 音量の増減量
    float fluctuation = 0.1f;
    // 音量の最大値
    float MAXVOLUME = 1.0f;
    // 音量アイコンの初期位置
    float volumePos = -114f;
    float volumeMove = 34.3f;
    // 項目別の座標
    Vector3 selectPos_bgm = new Vector3(0f, 34f, 0f);
    Vector3 selectPos_se = new Vector3(0f, -38f, 0f);
    Vector3 selectPos_end = new Vector3(0f, -111f, 0f);

    // オプションの選択中の項目
    enum OptionSelect
    {
        BGM,
        SE,
        End,
    }

    void Update()
    {
        if (!isProcess) return;

        // 音量の大きさの変更を行う
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (select)
            {
                case (int)OptionSelect.BGM:
                    // 音量が０より大きいならば処理を行う
                    if(audioManager.bgmVolume > 0)
                    {
                        // 音量を低くする
                        audioManager.bgmVolume -= fluctuation;
                        bgmIconPos.localPosition -= new Vector3(volumeMove, 0f, 0f);

                        // 移動SEを鳴らす
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // 無効SEを鳴らす
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
                case (int)OptionSelect.SE:
                    // 音量が０より大きいならば処理を行う
                    if (audioManager.seVolume > 0)
                    {
                        // 音量を低くする
                        audioManager.seVolume -= fluctuation;
                        seIconPos.localPosition -= new Vector3(volumeMove, 0f, 0f);

                        // 移動SEを鳴らす
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // 無効SEを鳴らす
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (select)
            {
                case (int)OptionSelect.BGM:
                    // 音量が１より低いならば処理を行う
                    if (audioManager.bgmVolume < MAXVOLUME)
                    {
                        // 音量を高くする
                        audioManager.bgmVolume += fluctuation;
                        bgmIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);

                        // 移動SEを鳴らす
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // 無効SEを鳴らす
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
                case (int)OptionSelect.SE:
                    // 音量が１より低いならば処理を行う
                    if (audioManager.seVolume < MAXVOLUME)
                    {
                        // 音量を高くする
                        audioManager.seVolume += fluctuation;
                        seIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);

                        // 移動SEを鳴らす
                        //if (uISE.GetSelectSE != null){}
                    }
                    else
                    {
                        // 無効SEを鳴らす
                        //if(uISE.GetInputRejectionSE != null) {}
                    }
                    break;
            }
        }
        // セレクトアイコンを操作する
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 選択中の参照を変更
            select--;
            if (select < 0) select = (int)OptionSelect.End;
            // 移動SEを鳴らす
            //if (uISE.GetSelectSE != null){}
            // アイコンを移動させる
            SelectMove();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 選択中の参照を変更
            select++;
            if (select > (int)OptionSelect.End) select = 0;
            // 移動SEを鳴らす
            //if (uISE.GetSelectSE != null){}
            // アイコンを移動させる
            SelectMove();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (select != (int)OptionSelect.End) return;
            isProcess = false;
            // オプションを閉じる
            gameObject.SetActive(false);
            // 移動SEを鳴らす
            //if (uISE.GetCancelSE != null) { }
            // タイトルの処理を再開
            titleManager.processFlag = true;
        }
    }

    public void StartOption()
    {
        // セレクトアイコンを初期位置に戻す
        select = 0;
        SelectMove();
        // 音響スクリプトを取得
        audioManager = AudioManager.Instance;
        // 初期位置に置く
        bgmIconPos.localPosition = new Vector3(volumePos, bgmIconPos.localPosition.y, 0f);
        seIconPos.localPosition = new Vector3(volumePos, seIconPos.localPosition.y, 0f);
        // 現在の音量に応じて位置を変更する
        for (float i = 0; i < audioManager.bgmVolume; i += fluctuation)
        {
            bgmIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);
        }
        for (float i = 0; i < audioManager.seVolume; i += fluctuation)
        {
            seIconPos.localPosition += new Vector3(volumeMove, 0f, 0f);
        }
        // オブジェクトを可視化する
        gameObject.SetActive(true);
        isProcess = true;
    }
    /// <summary>
    ///  セレクトアイコンの座標移動
    /// </summary>
    void SelectMove()
    {
        switch (select)
        {
            case (int)OptionSelect.BGM:
                selectIcon.localPosition = selectPos_bgm;
                break;
            case (int)OptionSelect.SE:
                selectIcon.localPosition = selectPos_se;
                break;
            case (int)OptionSelect.End:
                selectIcon.localPosition = selectPos_end;
                break;
        }
    }
}
