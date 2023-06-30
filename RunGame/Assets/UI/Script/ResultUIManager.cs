using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] GameObject collectibles;
    [SerializeField] GameObject scoreMane;
    [SerializeField] RectTransform selectImage;        // 選択アイコン

    int number = 0;             // 処理番号

    float retryPos_x = -204f;
    float nextPos_x = 78f;

    const int MAXNUMBER = 1;    // 最大処理番号
    enum ProcessNumber
    {
        RETRY,
        NEXT,
    }

    public void ResultProcess()
    {
        StartCoroutine(Result());
    }

    private IEnumerator Result()
    {
        // スコアなどを取得
        int score = scoreMane.GetComponent<ScoreManager>().GetScore;
        bool[] collect = collectibles.GetComponent<CollectiblesUI>().GetCollectibles;

        // 各UIの非表示
        scoreMane.SetActive(false);
        collectibles.SetActive(false);

        // UIを表示
        GetComponent<Animator>().SetBool("isClear", true);

        yield return new WaitForSeconds(3.0f);


        // 入手したコレクトアイテムの表示
        for(int i = 0; i < collect.Length; i++)
        {
            // コレクトアイテムを取得しているなら処理を行う
            if (collect[i])
            {

            }
            yield return null;
        }

        // スコアの表示


        // セーブを行う
        // セーブマネージャーを探す
        SaveManager save = GameObject.Find("SavaManager").GetComponent<SaveManager>();

        save.Save();

        // 入力処理
        while (true)
        {
            yield return null;
            // 左右キーで選択を変更する
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // 処理番号を減算する0未満になった場合、0に直す
                number--;
                if (number < 0) number = 0;
                // 選択アイコンを移動させる
                yield return SelectImageMove();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // 処理番号を加算する最大選択肢値を超えたら最大値に戻す
                number++;
                if (number > MAXNUMBER) number = MAXNUMBER;
                // 選択アイコンを移動させる
                yield return SelectImageMove();
            }
            // 決定キーが押された時、シーン推移を行う
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                // フェードマネージャーを探す
                FadeSceneManager fade = GameObject.Find("FadeManager").GetComponent<FadeSceneManager>();
                // 番号ごとに処理を行う
                switch (number)
                {
                    // 現在のシーンを読み込みなおす
                    case (int)ProcessNumber.RETRY:
                        fade.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    // セレクトシーンを読み込む
                    case (int)ProcessNumber.NEXT:
                        fade.LoadScene("SelectScene");
                        break;
                    // 当てはまらない数値が検出された場合、エラー文を出してセレクトシーンに戻す
                    default:
                        Debug.LogError("当てはまらない数値が検出されました！");
                        fade.LoadScene("SelectScene");
                        break;
                }
            }
        }
    }
    IEnumerator SelectImageMove()
    {
        // 現在の処理番号の位置に移動させる
        switch (number)
        {
            case (int)ProcessNumber.RETRY:
                selectImage.localPosition = new Vector3(retryPos_x, selectImage.localPosition.y, 0.0f);
                break;
            case (int)ProcessNumber.NEXT:
                selectImage.localPosition = new Vector3(nextPos_x, selectImage.localPosition.y, 0.0f);
                break;
            // 当てはまらない数値が検出された場合、エラー文を出す
            default:
                Debug.LogError("当てはまらない数値が検出されました！");
                break;
        }
        yield return null;
    }
}
