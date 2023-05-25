using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    int number = 0;             // 処理番号
    bool isControl = false;     // 処理を行うかのフラグ

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

            }
        }
    }

    public void GameOver()
    {
        // 自身を表示する
        this.gameObject.SetActive(true);
        // 処理を行うようにフラグを入れる
        isControl = true;
    }
}
