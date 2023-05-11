using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("アイテムの種類")]
    [SerializeField] _ItemKinds itemKinds;
    [Header("アイテムの値")]
    [SerializeField] int value;

    public _ItemKinds GetItemKinds { get { return itemKinds; } }

    public int GetValue { get { return value; } }
}
