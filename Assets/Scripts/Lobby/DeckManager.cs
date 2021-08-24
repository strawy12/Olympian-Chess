using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private static DeckManager instance = null;
    public static DeckManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DeckManager>();
                if (instance == null)
                {
                    instance = new GameObject("DeckManager").AddComponent<DeckManager>();
                }
            }
            return instance;
        }
    }
    public Sprite[] sprites = new Sprite[23];
    public int card;
    public bool isSelected = false;
    public bool[] isChosen;
    [SerializeField]
    private CardItemSO cardItemSO;

    //���⼭ isChosen[ī�� ��ȣ]�� true�� �� ī�带 ���õȰ���

    private void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = cardItemSO.cardItems[i].sprite;
        }
    }
    public void SelectedCard(int Card)
    {
        card = Card;
    }
    public void Select()
    {
        isSelected = true;
    }
    public void Deselect()
    {
        isSelected = false;
    }
    public Sprite GetSprite(int index)
    {
        return sprites[index];
    }
}
