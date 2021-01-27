using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Random Unit", menuName = "Skills/Summon Random Unit")]
public class SummonRandomUnit : Skill
{
    public override void SkillActivation(short playerNumber)
    {
        List<GameObject> deck;
        GameController gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameController>();
        if (playerNumber == 1)
        {
            deck = gameManager.player1.GetDeck();
            int rndAnimal = Random.Range(0, deck.Count);
            int row = 0;
            switch (deck[rndAnimal].GetComponent<AnimalController>().animalType.animalRowType)
            {
                case AnimalRowType.Front:
                    row = 3;
                    break;
                case AnimalRowType.Middle:
                    row = Random.Range(1, 3);
                    break;
                case AnimalRowType.Back:
                    row = 0;
                    break;
                default:
                    break;
            }
            int column = Random.Range(0, gameManager.columnSize);

            if (Grid.instance.IsCellNull(column, row))
            {
                GameObject unit = Instantiate(deck[rndAnimal], new Vector3(column, 1f, row), Quaternion.Euler(0f, 0f, 0f));
                unit.GetComponent<AnimalController>().playerNumber = 1;
                Grid.instance.SetUnit(column, row, unit);
            }
        }
        else
        {
            deck = gameManager.player2.GetDeck();
            int rndAnimal = Random.Range(0, deck.Count);
            int row = 0;
            switch (deck[rndAnimal].GetComponent<AnimalController>().animalType.animalRowType)
            {
                case AnimalRowType.Front:
                    row = 4;
                    break;
                case AnimalRowType.Middle:
                    row = Random.Range(5, 7);
                    break;
                case AnimalRowType.Back:
                    row = 7;
                    break;
                default:
                    break;
            }
            int column = Random.Range(0, gameManager.columnSize);

            if (Grid.instance.IsCellNull(column, row))
            {
                GameObject unit = Instantiate(deck[rndAnimal], new Vector3(column, 1f, row), Quaternion.Euler(0f, 180f, 0f));
                unit.GetComponent<AnimalController>().playerNumber = 2;
                Grid.instance.SetUnit(column, row, unit);
            }
        }
        
        

        Debug.Log("Summon Random Unit");
    }
}
