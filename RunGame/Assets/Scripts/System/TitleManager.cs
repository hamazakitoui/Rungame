using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトルマネージャー </summary>
public class TitleManager : MonoBehaviour
{
    int select = 0;         // 選択参照値
    public bool processFlag { get; set; }     // 処理フラグ
    [SerializeField] SceneObject selectScene; // ロードシーン
    [SerializeField] AudioObject bgm; // BGM
    [SerializeField] RectTransform SelectIcon;
    [SerializeField] OptionController option;
    [SerializeField] UISEData uISE;

    Vector3 selectPos_start = new Vector3(-106f, -28f, 0f);
    Vector3 selectPos_option = new Vector3(-106f, -90f, 0f);
    Vector3 selectPos_end = new Vector3(-106f, -146f, 0f);

    // タイトル画面の選択中の項目
    enum TitleSelect
    {
        GameStart,
        Option,
        End,
    }

    void Start()
    {
        if (bgm != null && bgm != "") AudioManager.Instance.PlayBGM(bgm); // BGM再生
        SaveManager.Instance.Load(); // 読み込み

        processFlag = true; // 操作可能にする
    }
    void Update()
    {
        if (!processFlag) return;

        // スペースキーで処理を行う
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            // 移動SEを鳴らす
            //if (uISE.GetDecisionSE != null) { }
            switch (select)
            {
                case (int)TitleSelect.GameStart:
                    processFlag = false;
                    FadeSceneManager.Instance.LoadScene(selectScene);
                    break;
                case (int)TitleSelect.Option:
                    processFlag = false;
                    option.StartOption();
                    break;
                case (int)TitleSelect.End:
                    processFlag = false;
                    Quit();
                    break;
            }
        }
        // セレクトアイコンを操作する
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 選択中の参照を変更
            select--;
            if (select < 0) select = (int)TitleSelect.End;
            // 移動SEを鳴らす
            //if (uISE.GetSelectSE != null){}
            // アイコンを移動させる
            SelectMove();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 選択中の参照を変更
            select++;
            if (select > (int)TitleSelect.End) select = 0;
            // 移動SEを鳴らす
            //if (uISE.GetSelectSE != null){}
            // アイコンを移動させる
            SelectMove();
        }
    }
    /// <summary>
    ///  セレクトアイコンの座標移動
    /// </summary>
    void SelectMove()
    {
        switch (select)
        {
            case (int)TitleSelect.GameStart:
                SelectIcon.localPosition = selectPos_start;
                break;
            case (int)TitleSelect.Option:
                SelectIcon.localPosition = selectPos_option;
                break;
            case (int)TitleSelect.End:
                SelectIcon.localPosition = selectPos_end;
                break;
        }
    }
    /// <summary> アプリの終了 </summary>
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
    }
}
