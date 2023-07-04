using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStaging : MonoBehaviour
{
    



    public void Staging(int number)
    {
        // ƒAƒjƒÄ¶
        transform.GetChild(number).gameObject.GetComponent<Animator>().SetBool("isGet", true);


    }

}
