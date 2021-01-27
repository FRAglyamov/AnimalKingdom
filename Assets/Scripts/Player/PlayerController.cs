using UnityEngine;
using System;

[Serializable]
public class PlayerController : MonoBehaviour
{
    public Transform deckContainer;
    public SaveLoadSystem sls;
    public DeckData[] playerDeck;

    //For deck
    public DeckConstructor deckConstructor;
    public Transform king;
    public Transform units;
    public TextMesh deckName = new TextMesh();
    public int chosenDeck;

    void Awake()
    {
        sls = new SaveLoadSystem();
        playerDeck = sls.LoadDeck();
    }

    public void UpdateDeck()
    {
        //Debug.Log(playerDeck[0]);
        //Debug.Log(deckConstructor.deckId);
        DeckData tempDeck = new DeckData();

        //tempDeck.deckName = deckName.name;

        foreach (Transform t in king)
        {
            if (t.childCount > 0)
            {
                var temp = t.GetChild(0).GetComponent<King>();
                //tempDeck.king = new KingData();
                tempDeck.king.isNull = false;
                tempDeck.king.isAnimal = temp.isAnimal;
                tempDeck.king.unitId = temp.unitId;
            }
        }

        foreach (Transform t in units)
        {
            if (t.childCount > 0)
            {
                int i = t.GetComponent<SlotHandler>().slotID - 1;
                var temp = t.GetChild(0).GetComponent<Animal>();
                //tempDeck.animalUnits[i] = new AnimalData();
                tempDeck.animalUnits[i].isNull = false;
                tempDeck.animalUnits[i].isAnimal = temp.isAnimal;
                tempDeck.animalUnits[i].unitId = temp.unitId;
            }
        }

        //Debug.Log(tempDeck);
        //Debug.Log(deckConstructor.deckId);
        //Debug.Log(playerDeck[deckConstructor.deckId - 1]);

        playerDeck[deckConstructor.deckId - 1] = tempDeck;
        playerDeck[deckConstructor.deckId - 1].deckId = deckConstructor.deckId;
        sls.SaveDeck(playerDeck);
    }
}
