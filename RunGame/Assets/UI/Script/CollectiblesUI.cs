using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesUI : MonoBehaviour
{
    [Header("���W��UI�̃v���t�@�u")]
    [SerializeField] GameObject collectiblesImageUI;
    [Header("���W���̉摜")]
    [SerializeField] Sprite collectiblesSprite;
    [Header("���W���̐�")]
    [SerializeField]int collectiblesValue = 3;      // ���W���̐�

    bool[] collectibles;                            // ���W���̊l���󋵂�����
    Image[] collectiblesImage;                      // ���W���̕\��UI         

    public bool[] GetCollectibles { get { return collectibles; } }
    void Start()
    {
        // �����l������
        collectibles = new bool[collectiblesValue];
        collectiblesImage = new Image[collectiblesValue];

        for(int i = 0; i < collectiblesValue; i++)
        {
            // ���W���̐����摜�𐶐����Ĕz��ɓ����
            collectiblesImage[i] = Instantiate(collectiblesImageUI, transform.position, Quaternion.identity, transform).gameObject.GetComponent<Image>();
        }
    }

    public void SetCollectibles(int number)
    {
        // ���W�������
        collectibles[number] = true;

        // UI�ɔ��f
        GetComponent<CollectStaging>().Staging(number);
    }
}
