using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHandler : MonoBehaviour, IDropHandler
{
    enum DragVariants
    {
        DeckToList,
        ListToDeck,
        DeckToDeck,
        ListToList
    }

    enum SlotsVariants
    {
        King_Deck,
        Units_Deck,
        Kings_List,
        Units_List
    }

    public bool unitItem; //Is it unit?
    public bool deckSlot; //Is it deck?
    public int slotID;

    public GameObject unit
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Same Slots (King or Unit)
        if (unitItem == DragHandler.unitDragged.GetComponent<DragHandler>().unitSlot)
        {
            DragVariants? dragCheck = CheckDrag();
            //Debug.Log(dragCheck);

            if (dragCheck == DragVariants.ListToDeck)
            {
                if (unit)
                {
                    var temp = unit;
                    temp.transform.SetParent(FindItemInDeck(unit.transform));
                    temp.transform.localPosition = Vector3.zero;
                }
                DragHandler.unitDragged.transform.SetParent(transform);
                DragHandler.unitDragged.transform.localPosition = Vector3.zero;
            }
            else if (dragCheck == DragVariants.DeckToList)
            {
                DragHandler.unitDragged.transform.SetParent(FindItemInDeck(DragHandler.unitDragged.transform));
                DragHandler.unitDragged.transform.localPosition = Vector3.zero;
            }
            else if (dragCheck == DragVariants.DeckToDeck)
            {
                if (unit)
                {
                    var temp = unit;
                    temp.transform.SetParent(DragHandler.unitDragged.transform.parent);
                    temp.transform.localPosition = Vector3.zero;
                }
                DragHandler.unitDragged.transform.SetParent(transform);
                DragHandler.unitDragged.transform.localPosition = Vector3.zero;
            }
        }
    }

    private Transform FindItemInDeck(Transform toFind)
    {
        var tempTransform = toFind.parent.parent.parent.parent;
        //Debug.Log(tempTransform);

        if (unitItem)
        {
            tempTransform = tempTransform.Find("Units List").GetChild(0);
        }
        else
        {
            tempTransform = tempTransform.Find("Kings List").GetChild(0);
        }

        tempTransform = tempTransform.GetChild(toFind.GetComponent<DragHandler>().unitID - 1);

        return tempTransform;
    }

    private DragVariants? CheckDrag()
    {
        var from = DragHandler.unitDragged.transform.parent.parent.parent;
        var to = transform.parent.parent;
        var enumFrom = CheckSlot(from);
        var enumTo = CheckSlot(to);

        //Debug.Log(enumFrom);
        //Debug.Log(enumTo);

        if (enumFrom == enumTo)
        {
            if (enumFrom == SlotsVariants.Kings_List || enumFrom == SlotsVariants.Units_List)
                return DragVariants.ListToList;
            if (enumFrom == SlotsVariants.King_Deck || enumFrom == SlotsVariants.Units_Deck)
                return DragVariants.DeckToDeck;
        }
        else
        {
            if (enumFrom == SlotsVariants.Kings_List || enumFrom == SlotsVariants.Units_List)
                return DragVariants.ListToDeck;
            if (enumFrom == SlotsVariants.King_Deck || enumFrom == SlotsVariants.Units_Deck)
                return DragVariants.DeckToList;
        }

        return null;
    }

    private SlotsVariants? CheckSlot(Transform transform)
    {
        switch (transform.name)
        {
            case "King":
                return SlotsVariants.King_Deck;
            case "Units":
                return SlotsVariants.Units_Deck;
            case "Kings List":
                return SlotsVariants.Kings_List;
            case "Units List":
                return SlotsVariants.Units_List;
        }
        return null;
    }
}
