using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISEData", menuName = "CreateUISEData")]
public class UISEData : ScriptableObject
{
    [SerializeField] AudioClip selectSE = null;
    [SerializeField] AudioClip decisionSE = null;
    [SerializeField] AudioClip cancelSE = null;
    [SerializeField] AudioClip inputRejectionSE = null;

    // “Ç‚ÝŽæ‚è—p
    public AudioClip GetSelectSE { get { return selectSE; } }
    public AudioClip GetDecisionSE { get { return decisionSE; } }
    public AudioClip GetCancelSE { get { return cancelSE; } }
    public AudioClip GetInputRejectionSE { get { return inputRejectionSE; } }
}
