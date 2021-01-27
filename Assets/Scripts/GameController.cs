using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TurnStage
{
    GenerateUnit,
    WaitingForMove,
    Move,
    Interaction,
    End
}

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();

                if (_instance == null)
                {
                    _instance = new GameObject("GameController").AddComponent<GameController>();
                }
            }

            return _instance;
        }
    }

    private byte _curPlayerNumber = 1;
    private float _gameSpeed = 3f;


    public short columnSize = 8;
    public short rowSize = 8;

    [SerializeField]
    public Player player1;
    [SerializeField]
    public Player player2;

    public GameObject king1;
    public GameObject king2;

    [SerializeField]
    private TurnStage turnStage = TurnStage.GenerateUnit;
    private SwipeDirection lastDirection;
    private SwipeDirection turnDirection;
    [SerializeField]
    private bool isSwiped = false;
    float turnTime = 7f;

    public GameObject gameOverPanel;

    [SerializeField]
    private Grid grid;

    private void Awake()
    {
        SwipeDetection.OnSwipe += CheckDirection;
        grid = Grid.instance;
        PlayerStartSetup();

    }

    private void Start()
    {
        gameOverPanel = GameObject.FindGameObjectWithTag("Game Canvas").transform.Find("Game Over").gameObject;

        GenerateStartupUnits();
    }

    private void PlayerStartSetup()
    {
        //if (GameObject.Find("ToTransfer") != null) // Исправить!
        //{
        //    player1 = new Player(1);
        //    player2 = new Player(2);
        //}

        player1.SetPlayer();
        player2.SetPlayer();

        king1 = Instantiate(player1.GetKing(), new Vector3(3.5f, 1f, -3f), Quaternion.Euler(0f, 0f, 0f));
        king1.GetComponent<KingController>().playerNumber = 1;
        king2 = Instantiate(player2.GetKing(), new Vector3(3.5f, 1f, 10f), Quaternion.Euler(0f, 180f, 0f));
        king2.GetComponent<KingController>().playerNumber = 2;
        //player1.SetKing(Instantiate(king1, new Vector3(3.5f, 1f, -3f), Quaternion.Euler(0f, 0f, 0f)));
        //player2.SetKing(Instantiate(king2, new Vector3(3.5f, 1f, 10f), Quaternion.Euler(0f, 180f, 0f)));
    }

    public void GenerateStartupUnits()
    {
        // Generate Startup units
        for (int i = 0; i <= 2; i++)
        {
            //GeneratePlayerAnimal();
            grid.GeneratePlayerAnimal(_curPlayerNumber, player1, player2);
            grid.isAnimalGenerated = false;
        }
        _curPlayerNumber = 2;
        for (int i = 0; i <= 2; i++)
        {
            grid.GeneratePlayerAnimal(_curPlayerNumber, player1, player2);
            grid.isAnimalGenerated = false;
        }
        _curPlayerNumber = 1;
    }



    private void CheckDirection(SwipeDetection.SwipeData data)
    {
        if (turnStage == TurnStage.WaitingForMove)
        {
            lastDirection = data.Direction;
            isSwiped = true;
        }
    }
    private void CheckDirectionOnPC()
    {
        if (turnStage == TurnStage.WaitingForMove)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                lastDirection = SwipeDirection.Up;
                isSwiped = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                lastDirection = SwipeDirection.Down;
                isSwiped = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                lastDirection = SwipeDirection.Left;
                isSwiped = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                lastDirection = SwipeDirection.Right;
                isSwiped = true;
            }

        }
    }

    private void Update()
    {
        PlayTurn();
    }

    private void PlayTurn()
    {
        switch (turnStage)
        {
            case TurnStage.GenerateUnit:
                Debug.Log("GameController: " + _curPlayerNumber + ", " + player1 + ", " + player2);
                grid.GeneratePlayerAnimal(_curPlayerNumber, player1, player2);
                if (grid.isAnimalGenerated == true)
                {
                    grid.isAnimalGenerated = false;
                    turnStage = TurnStage.WaitingForMove;
                }
                break;

            case TurnStage.WaitingForMove:
                // Waiting for Swipe and remember the direction
                CheckDirectionOnPC();
                if (isSwiped == true)
                {
                    turnDirection = lastDirection;
                    turnStage = TurnStage.Move;
                    isSwiped = false;
                    Debug.Log("SWIPE DETECTED");
                }
                break;

            case TurnStage.Move:
                grid.MoveAllAnimals(turnDirection, turnTime, _gameSpeed, _curPlayerNumber);
                if (grid.isAnimalMoved == true)
                {
                    grid.isMoveCalculated = false;
                    grid.isAnimalMoved = false;
                    //StopAllCoroutines();
                    grid.StopAllCoroutines();
                    turnStage = TurnStage.Interaction;
                }
                break;

            case TurnStage.Interaction:
                grid.AttackEnemy(turnDirection, _curPlayerNumber);
                turnStage = TurnStage.End;
                break;

            case TurnStage.End:
                if (_curPlayerNumber == 1)
                {
                    _curPlayerNumber = 2;
                    king1.GetComponent<KingController>().MinusCD();

                }
                else
                {
                    _curPlayerNumber = 1;
                    king2.GetComponent<KingController>().MinusCD();
                }

                if (king1.GetComponent<KingController>().curHP <= 0)
                {
                    GameOverScreen(2);
                }
                else if (king2.GetComponent<KingController>().curHP <= 0)
                {
                    GameOverScreen(1);
                }
                turnStage = TurnStage.GenerateUnit;
                break;

            default:
                Debug.LogError("Something wrong with turn stage");
                break;
        }
    }

    public void GameOverScreen(short playerNumberWhoWin)
    {
        gameOverPanel.SetActive(true);
        if (playerNumberWhoWin != 1)
        {
            gameOverPanel.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        gameObject.GetComponent<GameController>().enabled = false;
    }

    //num = player * 10 + skill slot num
    public void ActivateSkill(int num)
    {
        Debug.Log("Current player " + _curPlayerNumber);
        //Debug.Log(num / 10);
        //Debug.Log(num % 10);
        if (_curPlayerNumber == num / 10)
        {
            GameObject chosenPlayerKing = num / 10 == 1 ? king1 : king2;
            //Debug.Log(chosenPlayerKing);
            var skill = chosenPlayerKing.GetComponent<KingController>().kingType;
            if (num % 10 == 1)
            {
                if (chosenPlayerKing.GetComponent<KingController>().firstSkillCDTimeLeft == 0)
                {
                    skill.firstSkill.SkillActivation((short)(num / 10));
                    chosenPlayerKing.GetComponent<KingController>().firstSkillCDTimeLeft = skill.firstSkill.CD;
                    chosenPlayerKing.GetComponent<KingController>().UpdateCooldownInCanvas();
                }
                else
                {
                    Debug.Log("First skill timer: " + chosenPlayerKing.GetComponent<KingController>().firstSkillCDTimeLeft);
                }
            }
            else
            {
                if (chosenPlayerKing.GetComponent<KingController>().secondSkillCDTimeLeft == 0)
                {
                    skill.secondSkill.SkillActivation((short)(num / 10));
                    chosenPlayerKing.GetComponent<KingController>().secondSkillCDTimeLeft = skill.secondSkill.CD;
                    chosenPlayerKing.GetComponent<KingController>().UpdateCooldownInCanvas();
                }
                else
                {
                    Debug.Log("Second skill timer: " + chosenPlayerKing.GetComponent<KingController>().secondSkillCDTimeLeft);
                }
            };
        }
    }
}