using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] RectTransform SelectImage;        // 選択アイコン

    int number = 0;             // 処理番号
    bool isControl = false;     // 処理を行うかのフラグ

    const int MAXNUMBER = 1;    // 最大処理番号
    const float RETRY_Y = 9.0f;  // リトライのｙ座標
    const float EXIT_Y = -69.0f; // ステージを出る選択のｙ座標

    enum ProcessNumber
    {
        RETRY,      // ステージをやりなおす
        EXIT,       // ステージから出る
    }

    void Update()
    {
        // 処理を行わない場合、returnを返す
        if (!isControl) { return; }

        // 決定ボタンが押された場合、それに応じた処理を行う
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (number)
            {
                // 現在のシーンを読み込みなおす
                case (int)ProcessNumber.RETRY:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                // セレクトシーンを読み込む
                case (int)ProcessNumber.EXIT:
                    SceneManager.LoadScene("SelectScene");
                    break;
                // 当てはまらない数値が検出された場合、エラー文を出してセレクトシーンに戻す
                default:
                    Debug.LogError("当てはまらない数値が検出されました！");
                    SceneManager.LoadScene("SelectScene");
                    break;
            }
        }

        // 上下キーで選択を変更する
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 処理番号を減算する0未満になった場合、0に直す
            number--;
            if (number < 0) number = 0;
            // 選択アイコンを移動させる
            SelectImageMove();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 処理番号を加算する最大選択肢値を超えたら最大値に戻す
            number++;
            if (number > MAXNUMBER) number = MAXNUMBER;
            // 選択アイコンを移動させる
            SelectImageMove();
        }
    }

    void SelectImageMove()
    {
        // 現在の処理番号の位置に移動させる
        switch (number)
        {
            case (int)ProcessNumber.RETRY:
                SelectImage.localPosition = new Vector3(SelectImage.localPosition.x, RETRY_Y, 0.0f);
                break;
            case (int)ProcessNumber.EXIT:
                SelectImage.localPosition = new Vector3(SelectImage.localPosition.x, EXIT_Y, 0.0f);
                break;
            // 当てはまらない数値が検出された場合、エラー文を出す
            default:
                Debug.LogError("当てはまらない数値が検出されました！");
                break;
        }
    }

    public void GameOver()
    {
        // 自身を表示する
        this.gameObject.SetActive(true);
        // アニメーションさせる
        GetComponent<Animator>().SetBool("isMove", true);
    }

    public void isProcess()
    {
        // 処理を行うようにフラグを入れる
        isControl = true;
    }
}
