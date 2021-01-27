using System.Collections;
using UnityEngine;

public class DeckConstructor : MonoBehaviour
{
    public int deckId;

    public DeckData deckData;

    public PlayerController playerController;

    public Transform decksContainerInDeckMenu;

    public Transform kingsHolder;
    public Transform unitsHolder;

    private Transform KingDeck;
    private Transform UnitsDeck;
    private Transform KingsList;
    private Transform UnitsList;

    void Awake()
    {
        KingDeck = transform.Find("King").Find("Container");
        UnitsDeck = transform.Find("Units").Find("Container");
        KingsList = transform.Find("Kings List").Find("Containers");
        UnitsList = transform.Find("Units List").Find("Containers");
    }

    //private Transform KingDeck()
    //{
    //    return transform.Find("King").Find("Container");
    //}
    //private Transform UnitsDeck()
    //{
    //    return transform.Find("Units").Find("Container");
    //}
    //private Transform KingsList()
    //{
    //    return transform.Find("Kings List").Find("Containers");
    //}
    //private Transform UnitsList()
    //{
    //    return transform.Find("Units List").Find("Containers");
    //}

    public void ConstructorUpdate(int num)
    {
        deckId = num;
        deckData = playerController.playerDeck[deckId - 1];
        KingDeckUpdate();
        UnitsDeckUpdate();
    }

    private void KingDeckUpdate()
    {
        // With Holder
        //if (!deckData.king.isNull)
        //{
        //    foreach (Transform t in kingsHolder)
        //    {
        //        if (deckData.king.unitId == t.GetComponent<King>().unitId)
        //        {
        //            t.SetParent(KingDeck.GetChild(0));
        //            t.localPosition = Vector3.zero;
        //        }
        //    }
        //}


        // Without Holder. Every king in list
        if (!deckData.king.isNull)
        {
            Transform t = KingsList.GetChild(deckData.king.unitId - 1).GetChild(0);
            //Debug.Log(t);
            //Debug.Log(t.GetComponent<King>().unitId);
            if (deckData.king.unitId == t.GetComponent<King>().unitId)
            {
                t.SetParent(KingDeck.GetChild(0));
                t.localPosition = Vector3.zero;
            }
        }
    }

    private void UnitsDeckUpdate()
    {
        // With Holder
        //for (int i = 0; i < deckData.animalUnits.Length; i++)
        //{
        //    foreach (Transform t in unitsHolder)
        //    {
        //        if (deckData.animalUnits[i].unitId == t.GetComponent<Animal>().unitId)
        //        {
        //            t.SetParent(UnitsDeck.GetChild(i));
        //            t.localPosition = Vector3.zero;
        //        }
        //    }
        //}

        // Without Holder. Every king in list
        for (int i = 0; i < deckData.animalUnits.Length; i++)
        {
            if (!deckData.animalUnits[i].isNull)
            {
                Transform t = UnitsList.GetChild(deckData.animalUnits[i].unitId - 1).GetChild(0);
                if (deckData.animalUnits[i].unitId == t.GetComponent<Animal>().unitId)
                {
                    t.SetParent(UnitsDeck.GetChild(i));
                    t.localPosition = Vector3.zero;
                }
            }
        }
    }

    private void RemainsKingsUpdate()
    {
        foreach (Transform t in kingsHolder)
        {
            //Debug.Log(t.GetComponent<King>().unitId - 1);
            //Debug.Log(KingsList.GetChild(t.GetComponent<King>().unitId - 1));
            t.SetParent(KingsList.GetChild(t.GetComponent<King>().unitId - 1));
            t.localPosition = Vector3.zero;
        }
    }

    private void RemainsUnitsUpdate()
    {
        foreach (Transform t in unitsHolder)
        {
            //Debug.Log(t.GetComponent<Animal>().unitId - 1);
            //Debug.Log(UnitsList.GetChild(t.GetComponent<Animal>().unitId - 1));
            t.SetParent(UnitsList.GetChild(t.GetComponent<Animal>().unitId - 1));
            t.localPosition = Vector3.zero;
        }

        //for (int i = 0; i < unitsHolder.childCount; i++)
        //{
        //    Debug.Log(unitsHolder.GetChild(0).GetComponent<Animal>().unitId - 1);
        //    Debug.Log(UnitsList().GetChild(unitsHolder.GetChild(0).GetComponent<Animal>().unitId - 1));
        //    unitsHolder.GetChild(0).SetParent(UnitsList().GetChild(unitsHolder.GetChild(0).GetComponent<Animal>().unitId - 1));
        //    unitsHolder.GetChild(0).localPosition = Vector3.zero;
        //}
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(2);
    }

    public void SetBackAll()
    {
        SetBackKing();
        SetBackUnits();
    }

    private void SetBackKing()
    {
        Transform from = KingDeck.GetChild(0);
        if (from.childCount > 0)
        {
            from = from.GetChild(0);
            Transform to = KingsList.GetChild(from.GetComponent<King>().unitId - 1);
            from.SetParent(to);
            from.localPosition = Vector3.zero;
        }
    }

    private void SetBackUnits()
    {
        for (int i = 0; i < UnitsDeck.childCount; i++)
        {
            Transform from = UnitsDeck.GetChild(i);
            if (from.childCount > 0)
            {
                from = from.GetChild(0);
                Transform to = UnitsList.GetChild(from.GetComponent<Animal>().unitId - 1);
                from.SetParent(to);
                from.localPosition = Vector3.zero;
            }
        }
    }

    public void UpdateDeckMenuById()
    {
        Transform t = decksContainerInDeckMenu.GetChild(deckId - 1);
        t.GetComponent<Deck>().UpdateDeck();
    }
}