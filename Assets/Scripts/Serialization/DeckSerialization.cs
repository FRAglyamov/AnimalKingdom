using System;

[Serializable]
public class DeckData
{
    public DeckData()
    {
        king = new KingData();
        for (int i = 0; i < animalUnits.Length; i++)
        {
            animalUnits[i] = new AnimalData();
        }
    }

    public string deckName;
    public int deckId;
    public KingData king;
    public AnimalData[] animalUnits = new AnimalData[4];
}

[Serializable]
public class KingData
{
    public bool isNull = true;
    public bool isAnimal;
    public int unitId;
}

[Serializable]
public class AnimalData
{
    public bool isNull = true;
    public bool isAnimal;
    public int unitId;
}