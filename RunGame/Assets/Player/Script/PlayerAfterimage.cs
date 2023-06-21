using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterimage : MonoBehaviour
{
    [SerializeField] GameObject afterImageObj;
    List<GameObject> afterImageObjs;
    bool isGene = false;
    float maxTimeCount = 0.5f;
    float maxWaitTime = 0.15f;

    public void StartGenerator(Transform playerPos, SpriteRenderer playerSprite)
    {
        isGene = true;
        StartCoroutine(AfterImage(playerPos, playerSprite));
    }

    IEnumerator AfterImage(Transform playerPos, SpriteRenderer playerSprite)
    {
        float waitTimeCount = 0;
        float timeCount = 0;
        afterImageObjs = new List<GameObject>();

        while (true)
        {
            if (isGene && waitTimeCount >= maxWaitTime)
            {
                GameObject obj = Instantiate(afterImageObj, playerPos.position, Quaternion.identity);
                obj.GetComponent<SpriteRenderer>().sprite = playerSprite.sprite;

                afterImageObjs.Add(obj);

                waitTimeCount = 0f;
            }
            else waitTimeCount += Time.deltaTime;

            if (timeCount > maxTimeCount) {
                GameObject desobj = afterImageObjs[0];
                afterImageObjs.RemoveAt(0);
                Destroy(desobj);
                timeCount = 0f;
            }
            else { timeCount += Time.deltaTime; }

            if (!isGene && afterImageObjs.Count == 0) { break; }

            yield return null;
        }
    }

    public void EndGenerator()
    {
        isGene = false;

    }
}
