using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSEData", menuName = "CreatePlayerSEData")]
public class PlayerSEData : ScriptableObject
{
    [SerializeField] AudioClip deadSE = null;
    [SerializeField] AudioClip scoreGetSE = null;
    [SerializeField] AudioClip collectiblesGetSE = null;
    [SerializeField] AudioClip acceleratorSE = null;
    [SerializeField] AudioClip jumpRampSE = null;

    // �ǂݎ��p
    public AudioClip GetDeadSE { get { return deadSE; } }
    public AudioClip GetScoreSE { get { return scoreGetSE; } }
    public AudioClip GetCollectiblesSE { get { return collectiblesGetSE; } }
    public AudioClip GetAcceleratorSE { get { return acceleratorSE; } }
    public AudioClip GetJumpRampSE { get { return jumpRampSE; } }
}
