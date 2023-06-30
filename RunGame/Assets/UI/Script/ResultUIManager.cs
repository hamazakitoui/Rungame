using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] SaveManager save;


    void Start()
    {
        
    }

    public void ResultProcess()
    {
        StartCoroutine(Result());
    }

    private IEnumerator Result()
    {



        yield return null;
    }
}
