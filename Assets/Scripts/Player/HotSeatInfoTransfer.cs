using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSeatInfoTransfer : MonoBehaviour
{
    public DeckData player1;
    public DeckData player2;

    public List<GameObject> kingsListInOrder;
    public List<GameObject> unitsListInOrder;

    public List<GameObject> GetGameObjectsList(bool isFirstPlayer)
    {
        DeckData deckToGO = isFirstPlayer == true ? player1 : player2;
        List<GameObject> goList = new List<GameObject>();

        for (int i = 0; i < deckToGO.animalUnits.Length; i++)
        {
            if (unitsListInOrder.Count < deckToGO.animalUnits[i].unitId)
            {
                goList.Add(unitsListInOrder[0]);
            }
            else
            {
                goList.Add(unitsListInOrder[deckToGO.animalUnits[i].unitId - 1]);
            }
        }
        return goList;
    }

    public GameObject GetKing(bool isFirstPlayer)
    {
        DeckData deckToGO = isFirstPlayer == true ? player1 : player2;
        GameObject king;
        if (kingsListInOrder.Count < deckToGO.king.unitId)
        {
            king = kingsListInOrder[0];
        }
        else
        {
            king = kingsListInOrder[deckToGO.king.unitId - 1];
        }
        return king;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
