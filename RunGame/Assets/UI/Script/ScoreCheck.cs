using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCheck : MonoBehaviour
{
    public bool CollectAllCheck()
    {
        // �X�R�A�A�C�e�������邩�m�F����
        int value = transform.childCount;

        // �X�R�A�A�C�e����S�Ď�����̂Ȃ�true��Ԃ�
        if (value <= 0) return true;
        else return false;
    }
}
