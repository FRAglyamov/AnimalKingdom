using UnityEngine;

public class Deck : MonoBehaviour
{
    public int deckId;

    public DeckData deckData;

    public PlayerController playerController;

    void Start()
    {
        deckData = playerController.playerDeck[deckId - 1];
        SpawnKing();
        SpawnUnits();
    }

    private void SpawnKing()
    {
        if (!deckData.king.isNull)
        {
            string str = "King_id" + deckData.king.unitId;
            GameObject go = Instantiate(Resources.Load("Kings_Images/" + str) as GameObject, transform.Find("King"));
            go.transform.localPosition = Vector3.zero;
        }
    }

    private void SpawnUnits()
    {
        for (int i = 0; i < deckData.animalUnits.Length; i++)
        {
            if (!deckData.animalUnits[i].isNull)
            {
                string str = "Unit_id" + deckData.animalUnits[i].unitId;
                GameObject go = Instantiate(Resources.Load("Units_Images/" + str) as GameObject, transform.Find("Units").GetChild(i));
                go.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void SpawnSingleUnit(int i)
    {
        if (!deckData.animalUnits[i].isNull)
        {
            string str = "Unit_id" + deckData.animalUnits[i].unitId;
            GameObject go = Instantiate(Resources.Load("Units_Images/" + str) as GameObject, transform.Find("Units").GetChild(i));
            go.transform.localPosition = Vector3.zero;
        }
    }

    public void UpdateDeck()
    {
        deckData = playerController.playerDeck[deckId - 1];
        UpdateKing();
        UpdateUnits();
    }

    public void UpdateKing()
    {
        //Debug.Log(deckData.deckId);
        if (transform.Find("King").childCount > 0)
        {
            Transform t = transform.Find("King").GetChild(0);
            if (deckData.king.isNull)
            {
                Destroy(t.gameObject);
            }
            else if (t.GetComponent<King>().unitId != deckData.king.unitId)
            {
                Destroy(t.gameObject);
                SpawnKing();
            }
        }
        else
        {
            if (!deckData.king.isNull)
            {
                Debug.Log("Reached");
                SpawnKing();
            }
        }
    }

    private void UpdateUnits()
    {
        for (int i = 0; i < deckData.animalUnits.Length; i++)
        {
            if (transform.Find("Units").GetChild(i).childCount > 0)
            {
                Transform t = transform.Find("Units").GetChild(i).GetChild(0);
                //Debug.Log(t.GetComponent<Animal>().unitId);
                //Debug.Log(deckData.animalUnits[i].unitId);
                if (deckData.animalUnits[i].isNull)
                {
                    Destroy(t.gameObject);
                }
                else if (t.GetComponent<Animal>().unitId != deckData.animalUnits[i].unitId)
                {
                    Destroy(t.gameObject);
                    SpawnSingleUnit(i);
                }
            }
            else
            {
                if (!deckData.animalUnits[i].isNull)
                {
                    SpawnSingleUnit(i);
                }
            }
        }
    }

    public bool isFull
    {
        get
        {
            if (deckData.king.isNull)
                return false;
            for (int i = 0; i < deckData.animalUnits.Length; i++)
            {
                if (deckData.animalUnits[i].isNull)
                    return false;
            }
            return true;
        }
    }
}