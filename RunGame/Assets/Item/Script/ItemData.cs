using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("�A�C�e���̎��")]
    [SerializeField] _ItemKinds itemKinds;
    [Header("�A�C�e���̒l")]
    [SerializeField] float value;

    public _ItemKinds GetItemKinds { get { return itemKinds; } }

    public float GetValue { get { return value; } }
}
