using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> データセーブ </summary>
public class SaveManager : MonoBehaviour
{
    // 保存先指定末端文字
    private const char SAVE_FILE_END = '\\';
    // セーブデータファイル名、エンコード用文字コード
    private const string SAVE_FILE_PATH = "SaveData.json", ENCODE_CODE = "UTF-8";
    private SaveData data; // セーブデータ
    private static SaveManager instance; // インスタンス保存用変数
    /// <summary> シングルトン </summary>
    public static SaveManager Instance
    {
        get
        {
            // インスタンスが設定されていなければ
            if (instance == null)
            {
                instance = FindObjectOfType<SaveManager>(); // インスタンス検索
                // エラー処理
                if (instance == null) Debug.LogError($"{typeof(SaveManager)}が見つかりません!");
            }
            return instance;
        }
    }
    void Awake()
    {
        // 既にインスタンスがあるなら破棄
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        // セーブデータ初期化
        data = new SaveData();
        for(int d = 0; d < data.StageLength; d++)
        {
            data.SetStageClear(d, d == 0);
            data.SetAchievement(d, false);
        }
    }
    /// <summary> セーブ </summary>
    public void Save()
    {
        string json = JsonUtility.ToJson(data); // jsonファイル用に変換
        string path = null; // 保存先
        // エディタなら
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            path = Directory.GetCurrentDirectory(); // 保存先を現在のディレクトリの設定
#endif
        }
        // アプリなら
        // 実行ファイルの中に保存先指定
        else path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(SAVE_FILE_END);
        path += ("/" + SAVE_FILE_PATH); // パスの末端にセーブファイルを設定
        Encoding encoding = Encoding.GetEncoding(ENCODE_CODE); // 暗号化用オブジェクト生成
        // 上書き
        StreamWriter writer = new StreamWriter(path, false); // 書き込み用オブジェクト生成
        writer.WriteLine(Convert.ToBase64String(encoding.GetBytes(json))); // データを暗号化して上書き
        writer.Flush();
        writer.Close();
    }
    /// <summary> セーブ </summary>
    /// <param name="data">保存するデータ</param>
    public void Save(SaveData data)
    {
        this.data = data;
        Save(); // セーブ
    }
    /// <summary> 読み込み </summary>
    public void Load()
    {
        try
        {
            string path = null; // 読み込み先
            // エディタなら
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                path = Directory.GetCurrentDirectory(); // 現在のディレクトリから読み込み先指定
#endif
            }
            // アプリなら
            // 実行データの中から読み込み先指定
            else AppDomain.CurrentDomain.BaseDirectory.TrimEnd(SAVE_FILE_END);
            // 読込先の末端にセーブファイル設定してファイル情報取得
            FileInfo file = new FileInfo(path + "/" + SAVE_FILE_PATH);
            StreamReader reader = new StreamReader(file.OpenRead()); // ファイルを開いて読み込み
            string json = reader.ReadToEnd(); // 読み込んだデータをjsonファイルに変換
            Encoding encoding = Encoding.GetEncoding(ENCODE_CODE); // 暗号化用オブジェクト生成
            // jsonファイルをデータに変換
            // 暗号化されたデータを復元して読み込む
            data = JsonUtility.FromJson<SaveData>(encoding.GetString(Convert.FromBase64String(json)));
        }
        catch { } // エラー処理だが特に意味は無い
    }
    /// <summary> 保存されているセーブデータ </summary>
    public SaveData GetData { get { return data; } }
}
/// <summary> セーブデータ </summary>
[Serializable]
public class SaveData
{
    private const int DATA_LENGTH = 5; // 要素数
    // ステージクリア配列、実績配列
    private bool[] stageClear = new bool[DATA_LENGTH], stageAchievement = new bool[DATA_LENGTH];
    public int Score; // スコア
    /// <summary> クリア状況設定 </summary>
    /// <param name="element">ステージ番号</param> <param name="value">設定する要素</param>
    public void SetStageClear(int element, bool value)
    {
        stageClear[element] = value;
    }
    /// <summary> 実績設定 </summary>
    /// <param name="element">ステージ番号</param> <param name="value">設定する要素</param>
    public void SetAchievement(int element, bool value)
    {
        stageAchievement[element] = value;
    }
    /// <summary> クリア状況取得 </summary>
    /// <param name="element">ステージ番号</param>
    /// <returns>ステージのクリア状況</returns>
    public bool GetStageClear(int element)
    {
        return stageClear[element];
    }
    /// <summary> 実績取得 </summary>
    /// <param name="element">ステージ番号</param>
    /// <returns>ステージの実績</returns>
    public bool GetAchievement(int element)
    {
        return stageAchievement[element];
    }
    /// <summary> ステージ数 </summary>
    public int StageLength { get { return stageClear.Length; } }
}
