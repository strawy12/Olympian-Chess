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
            if(instance == null)
            {
                instance = FindObjectOfType<DeckManager>();
                if(instance == null)
                {
                    instance = new GameObject("DeckManager").AddComponent<DeckManager>();
                }
            }
            return instance;
        }
    }
    public Sprite[] sprites;
    public int card;
    public bool isSelected = false;
    public bool[] isChosen;

    //���⼭ isChosen[ī�� ��ȣ]�� true�� �� ī�带 ���õȰ���

    public void selectedCard(int Card)
    {
        card = Card;
    }
    public void select()
    {
        isSelected = true;
    }
    public void Deselect()
    {
        isSelected = false;
    }
}
