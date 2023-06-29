using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCheck : MonoBehaviour
{
    public bool CollectAllCheck()
    {
        // スコアアイテムがあるか確認する
        int value = transform.childCount;

        // スコアアイテムを全て取ったのならtrueを返す
        if (value <= 0) return true;
        else return false;
    }
}
