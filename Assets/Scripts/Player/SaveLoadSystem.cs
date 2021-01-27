using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class SaveLoadSystem
{
    private string jsonDeckPath = Application.persistentDataPath + "/deck.json";

    public DeckData[] CreateEmptyDeck(int i)
    {
        DeckData[] tempDeck = new DeckData[i];

        for (int j = 0; j < i; j++)
        {
            tempDeck[j] = new DeckData();
        }

        string json = JsonConvert.SerializeObject(tempDeck, Formatting.Indented);
        File.WriteAllText(jsonDeckPath, json);

        return tempDeck;
    }

    public DeckData[] LoadDeck()
    {
        try
        {
            string dataAsJson = File.ReadAllText(jsonDeckPath);
            DeckData[] temp = JsonConvert.DeserializeObject<DeckData[]>(dataAsJson);

            Debug.Log(jsonDeckPath);
            Debug.Log("Success load decks!");

            return temp;
        }
        catch
        {
            Debug.Log("Success create new decks!");

            return CreateEmptyDeck(5);
        }
    }

    public void SaveDeck(DeckData[] deckArray)
    {
        string json = JsonConvert.SerializeObject(deckArray, Formatting.Indented);
        File.WriteAllText(jsonDeckPath, json);

        Debug.Log("Success save!");
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}