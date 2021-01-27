using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HotSeatChooseScript : MonoBehaviour
{
    public Transform decksContainer;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI warningText;
    public HotSeatInfoTransfer dataTransfer;
    public MenuController menuController;

    private int player = 1;

    public void CheckIsFull()
    {
        foreach (Transform t in decksContainer)
        {
            Deck deck = t.GetComponent<Deck>();
            deck.UpdateDeck();
            if (deck.isFull)
            {
                warningText.gameObject.SetActive(false);
                t.gameObject.SetActive(true);
            }
        }
    }

    public void Reset()
    {
        foreach (Transform t in decksContainer)
        {
            t.gameObject.SetActive(false);
        }
    }

    public void SetChoose(int deckId)
    {
        if (player == 1)
        {
            playerText.text = "Player 2";
            dataTransfer.player1 = decksContainer.GetChild(deckId - 1).GetComponent<Deck>().deckData;
            player = 2;
        }
        else if (player == 2)
        {
            dataTransfer.player2 = decksContainer.GetChild(deckId - 1).GetComponent<Deck>().deckData;
            menuController.LoadGameScene(1);
            SetDefault();
        }
    }

    public void SetDefault()
    {
        warningText.gameObject.SetActive(true);
        playerText.text = "Player 1";
        player = 1;
        Reset();
    }
}
