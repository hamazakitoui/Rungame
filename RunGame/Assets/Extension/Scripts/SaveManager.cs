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
        data = new SaveData(0); // セーブデータ初期化
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
public struct SaveData
{
    // ステージ数、実績数
    private const int STAGE_LENGTH = 5, ACHIEVEMENT_LENGTH = 3;
    // ステージクリア配列
    public bool stageClear1, stageClear2, stageClear3, stageClear4, stageClear5;
    // 実績配列
    public bool sa1_1, sa1_2, sa1_3, sa2_1, sa2_2, sa2_3, sa3_1, sa3_2, sa3_3, sa4_1, sa4_2, sa4_3;
    public bool sa5_1, sa5_2, sa5_3;
    // スコア
    public int Score1, Score2, Score3, Score4, Score5;
    /// <summary> コンストラクタ </summary>
    /// <param name="num">ステージ数</param>
    public SaveData(int num)
    {
        // スコア初期化
        Score1 = num;
        Score2 = num;
        Score3 = num;
        Score4 = num;
        Score5 = num;
        // クリア状況初期化
        stageClear1 = false;
        stageClear2 = false;
        stageClear3 = false;
        stageClear4 = false;
        stageClear5 = false;
        // 実績初期化
        sa1_1 = false;
        sa1_2 = false;
        sa1_3 = false;
        sa2_1 = false;
        sa2_2 = false;
        sa2_3 = false;
        sa3_1 = false;
        sa3_2 = false;
        sa3_3 = false;
        sa4_1 = false;
        sa4_2 = false;
        sa4_3 = false;
        sa5_1 = false;
        sa5_2 = false;
        sa5_3 = false;
    }
    /// <summary> クリア状況設定 </summary>
    /// <param name="element">ステージ番号</param> <param name="value">設定する要素</param>
    public void SetStageClear(int element, bool value)
    {
        if (element >= STAGE_LENGTH) return; // 要素数超えていたら無視
        switch (element)
        {
            case 0:
                stageClear1 = value;
                break;
            case 1:
                stageClear2 = value;
                break;
            case 2:
                stageClear3 = value;
                break;
            case 3:
                stageClear4 = value;
                break;
            case 4:
                stageClear5 = value;
                break;
            default:
                break;
        }
    }
    /// <summary> 実績設定 </summary>
    /// <param name="stage">ステージ番号</param> <param name="element">要素番号</param>
    /// <param name="value">設定する要素</param>
    public void SetAchievement(int stage, int element, bool value)
    {
        if (stage >= STAGE_LENGTH && element >= ACHIEVEMENT_LENGTH) return; // 要素数超えていたら無視
        switch (stage)
        {
            case 0:
                switch (element)
                {
                    case 0:
                        sa1_1 = value;
                        break;
                    case 1:
                        sa1_2 = value;
                        break;
                    case 2:
                        sa1_3 = value;
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                switch (element)
                {
                    case 0:
                        sa2_1 = value;
                        break;
                    case 1:
                        sa2_2 = value;
                        break;
                    case 2:
                        sa2_3 = value;
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (element)
                {
                    case 0:
                        sa3_1 = value;
                        break;
                    case 1:
                        sa3_2 = value;
                        break;
                    case 2:
                        sa3_3 = value;
                        break;
                    default:
                        break;
                }
                break;
            case 3:
                switch (element)
                {
                    case 0:
                        sa4_1 = value;
                        break;
                    case 1:
                        sa4_2 = value;
                        break;
                    case 2:
                        sa4_3 = value;
                        break;
                    default:
                        break;
                }
                break;
            case 4:
                switch (element)
                {
                    case 0:
                        sa5_1 = value;
                        break;
                    case 1:
                        sa5_2 = value;
                        break;
                    case 2:
                        sa5_3 = value;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
    /// <summary> スコア保存 </summary>
    /// <param name="element">ステージ番号</param>
    /// <param name="value">保存する値</param>
    public void SetScore(int element,int value)
    {
        if (element >= STAGE_LENGTH) return; // 要素数超えていたら無視
        switch (element)
        {
            case 0:
                Score1 = value;
                break;
            case 1:
                Score2 = value;
                break;
            case 2:
                Score3 = value;
                break;
            case 3:
                Score4 = value;
                break;
            case 4:
                Score5 = value;
                break;
            default:
                break;
        }
    }
    /// <summary> ステージごとのスコア </summary>
    /// <param name="element">ステージ番号</param>
    /// <returns>保存されているスコア</returns>
    public int GetStageScore(int element)
    {
        if (element >= STAGE_LENGTH) return 0; // 要素数を超えたらFalseを必ず返す
        int result = 0;
        switch (element)
        {
            case 0:
                result = Score1;
                break;
            case 1:
                result = Score2;
                break;
            case 2:
                result = Score3;
                break;
            case 3:
                result = Score4;
                break;
            case 4:
                result = Score5;
                break;
            default:
                break;
        }
        return result;
    }
    /// <summary> クリア状況取得 </summary>
    /// <param name="element">ステージ番号</param>
    /// <returns>ステージのクリア状況</returns>
    public bool GetStageClear(int element)
    {
        if (element >= STAGE_LENGTH) return false; // 要素数を超えたらFalseを必ず返す
        bool result = false;
        switch (element)
        {
            case 0:
                result = stageClear1;
                break;
            case 1:
                result = stageClear2;
                break;
            case 2:
                result = stageClear3;
                break;
            case 3:
                result = stageClear4;
                break;
            case 4:
                result = stageClear5;
                break;
            default:
                break;
        }
        return result;
    }
    /// <summary> 実績取得 </summary>
    /// <param name="stage">ステージ番号</param> <param name="element">要素番号</param>
    /// <returns>ステージの実績</returns>
    public bool GetAchievement(int stage, int element)
    {
        if (element >= STAGE_LENGTH) return false; // 要素数を超えたらFalseを必ず返す
        bool result = false;
        switch (stage)
        {
            case 0:
                switch (element)
                {
                    case 0:
                        result = sa1_1;
                        break;
                    case 1:
                        result = sa1_2;
                        break;
                    case 2:
                        result = sa1_3;
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                switch (element)
                {
                    case 0:
                        result = sa2_1;
                        break;
                    case 1:
                        result = sa2_2;
                        break;
                    case 2:
                        result = sa2_3;
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (element)
                {
                    case 0:
                        result = sa3_1;
                        break;
                    case 1:
                        result = sa3_2;
                        break;
                    case 2:
                        result = sa3_3;
                        break;
                    default:
                        break;
                }
                break;
            case 3:
                switch (element)
                {
                    case 0:
                        result = sa4_1;
                        break;
                    case 1:
                        result = sa4_2;
                        break;
                    case 2:
                        result = sa4_3;
                        break;
                    default:
                        break;
                }
                break;
            case 4:
                switch (element)
                {
                    case 0:
                        result = sa5_1;
                        break;
                    case 1:
                        result = sa5_2;
                        break;
                    case 2:
                        result = sa5_3;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        return result;
    }
    /// <summary> ステージ数 </summary>
    public int StageLength { get { return STAGE_LENGTH; } }
}
