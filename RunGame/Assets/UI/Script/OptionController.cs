using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    [SerializeField] RectTransform selectIcon;
    [SerializeField] TitleManager titleManager;

    AudioManager audioManager;

    int select = 0;         // 選択参照値
    int volumeValue = 1;    // 音量値
    bool isProcess = false;

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
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
        // セレクトアイコンを操作する
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 選択中の参照を変更
            select--;
            if (select < 0) select = (int)OptionSelect.End;
            // アイコンを移動させる
            SelectMove();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 選択中の参照を変更
            select++;
            if (select > (int)OptionSelect.End) select = 0;
            // アイコンを移動させる
            SelectMove();
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (select != (int)OptionSelect.End) return;
            isProcess = false;
            // オプションを閉じる
            gameObject.SetActive(false);
            // タイトルの処理を再開
            titleManager.processFlag = true;
        }
    }

    public void StartOption()
    {
        // セレクトアイコンを初期位置に戻す
        select = 0;
        SelectMove();
        // 現在の音量に応じて位置を変更する
        audioManager = AudioManager.Instance;
        

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
