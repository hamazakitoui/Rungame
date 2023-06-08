using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWallManager : MonoBehaviour
{
    [Header("扉が開く速度")]
    [SerializeField] float openSpeed = 0.02f;
    
    // 扉オブジェクト
    Transform door = null;

    // 開く際の扉の目的座標
    float open_y = 0.51f;

    private void Start()
    {
        // 扉を取得
        door = transform.GetChild(0).gameObject.transform;
    }

    public void OpenWall()
    {
        // 扉開き処理を行う
        StartCoroutine("OpenProcess");
    }
    IEnumerator OpenProcess()
    {
        while (true)
        {
            // 扉を上に徐々に上げる
            door.localPosition += new Vector3(0.0f, openSpeed, 0.0f);

            // 扉の座標が目的座標に到達したならば、停止する
            if (door.localPosition.y >= open_y) { break; }

            // 1フレーム止める
            yield return null;
        }
    }
}
