using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Player : ScriptableObject
{

    public Player(byte plNumber)
    {
        this.playerNumber = plNumber;
    }

    [SerializeField]
    private GameObject _king;
    [SerializeField]
    private List<GameObject> _deck; //Животные игрока в колоде
    //private List<AnimalController> animalsOnDesk; // Животные игрока на игровом поле
    public byte playerNumber;

    public void SetPlayer()
    {
        // If transfer != null => get from transfer
        // else set some default settings
        GameObject transferGO = GameObject.Find("ToTransfer");
        if (transferGO != null)
        {
            HotSeatInfoTransfer transferData = transferGO.GetComponent<HotSeatInfoTransfer>();
            if (playerNumber == 1)
            {
                this._deck = transferData.GetGameObjectsList(true);
                this._king = transferData.GetKing(true);
            }
            else if(playerNumber == 2)
            {
                this._deck = transferData.GetGameObjectsList(false);
                this._king = transferData.GetKing(false);

            }
            else
            {
                Debug.LogError("Set Player - player number not equal to 1 or 2");
            }
        }
        else
        {
            if (playerNumber == 1)
            {
                this._king.GetComponent<KingController>().playerNumber = playerNumber;
            }
            else if (playerNumber == 2)
            {
                this._king.GetComponent<KingController>().playerNumber = playerNumber;
            }
        }
    }

    public GameObject GetKing() => this._king;
    public void SetKing(GameObject king)
    {
        if (king.tag == "King")
        {
            this._king = king;
            this._king.GetComponent<KingController>().playerNumber = playerNumber;
        }
        else
            Debug.LogError("Set King - tag error");
    }
    public List<GameObject> GetDeck() => this._deck;

}
