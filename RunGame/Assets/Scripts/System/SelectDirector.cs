using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> セレクトディレクター </summary>
public class SelectDirector : MonoBehaviour
{
    // 待機時間
    private const float APP_WAIT = 0.2f;
    private bool isSelect = false; // 選択判定
    [SerializeField] AudioObject bgm; // BGM
    [SerializeField] GameObject[] scenes; // 選択シーン配列
    [SerializeField] SceneObject[] selectScenes; // 選択可能シーン配列
    // Start is called before the first frame update
    void Start()
    {
        if (bgm != null && bgm != "") AudioManager.Instance.PlayBGM(bgm); // BGM再生
        SaveManager.Instance.Load(); // セーブデータ読み込み
        SaveData data = SaveManager.Instance.GetData; // ステージデータ
        Debug.Log(data.StageLength);
        // 選択できるシーンを初期化
        for(int s = 0; s < scenes.Length; s++)
        {
            // ステージ数を超えたら
            if (s >= data.StageLength)
            {
                scenes[s].SetActive(false); // 非表示
                continue;
            }
            if (s == 0) StartCoroutine(SceneActive(scenes[s], s)); // 初期シーンは常に選択できる
            else
            {
                // 前のシーンがクリアされていたら表示
                if (data.GetStageClear(s - 1)) StartCoroutine(SceneActive(scenes[s], s));
                else scenes[s].SetActive(false); // 非表示
            }
        }
    }
    /// <summary> セレクトボタン出現イベント </summary>
    /// <param name="scene">セレクトボタン</param> <param name="num">番号</param>
    /// <returns></returns>
    private IEnumerator SceneActive(GameObject scene, int num)
    {
        yield return new WaitForSeconds((num + 1) * APP_WAIT); // 一定時間待機
        scene.SetActive(true); // ボタン表示
        Vector3 scale = scene.transform.localScale; // ボタンの大きさ
        float rad = scene.transform.localEulerAngles.z; // ボタンの角度
        scene.transform.localScale = new Vector3(0, 0, 1); // 大きさをゼロに
        // 段々大きく
        while (scene.transform.localScale.x < scale.x)
        {
            scene.transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime;
            scene.transform.Rotate(0, 0, (num % 2 == 0 ? 360 : -360) * Time.deltaTime); // 回転
            yield return 0;
        }
        scene.transform.localScale = scale; // 大きさを元に戻す
        scene.transform.rotation = Quaternion.Euler(0, 0, rad); // 角度を元に戻す
    }
    /// <summary> シーン選択 </summary>
    /// <param name="scene">選択シーン番号</param>
    public void SceneSelect(int scene)
    {
        if (scene >= selectScenes.Length || scene < 0) return; // 番号が配列外なら無視
        if (isSelect) return; // 既に選択済みなら無視
        FadeSceneManager.Instance.LoadScene(selectScenes[scene]); // シーン読み込み
        isSelect = true;
    }
}
