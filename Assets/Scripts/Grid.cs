using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private static Grid _instance;
    public static Grid instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Grid>();

                if (_instance == null)
                {
                    _instance = new GameObject("Grid").AddComponent<Grid>();
                }
            }

            return _instance;
        }
    }

    private byte _columnSize = 8;
    private byte _rowSize = 8;
    private GameObject[,] _grid;

    List<GameObject> destroyableUnits = new List<GameObject>();
    List<MovableUnit> movableUnits = new List<MovableUnit>();
    List<InstatiableUnit> instantiableUnits = new List<InstatiableUnit>();
    public bool isAnimalGenerated = false;
    public bool isMoveCalculated = false;
    public bool isAnimalMoved = false;

    public void CreateGrid()
    {
        _grid = new GameObject[_columnSize, _rowSize];
    }

    public byte GetColumnSize() => _columnSize;
    public byte GetRowSize() => _rowSize;

    public bool IsCellNull(int column, int row)
    {
        if(_grid[column, row] == null)
            return true;
        else
            return false;
    }

    public void SetUnit(int column, int row, GameObject unit)
    {
        _grid[column, row] = unit;
    }

    private void Start()
    {
        CreateGrid();
    }


    public void GeneratePlayerAnimal(byte _playerNumber, Player player1, Player player2)
    {
        Debug.Log("Grid method: " + _playerNumber + ", " + player1 + ", " + player2);
        if (isAnimalGenerated == false)
        {
            if (_playerNumber == 1)
            {
                Debug.Log("Deck count: " + player1.GetDeck().Count);
                int rndAnimal = Random.Range(0, player1.GetDeck().Count);
                int row = 0;
                switch (player1.GetDeck()[rndAnimal].GetComponent<AnimalController>().animalType.animalRowType)
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
                //int rndRow = UnityEngine.Random.Range(0, _rowSize / 2 - 1);
                int column = Random.Range(0, _columnSize);

                if (_grid[column, row] == null)
                {
                    GameObject unit = Instantiate(player1.GetDeck()[rndAnimal], new Vector3(column, 1f, row), Quaternion.Euler(0f, 0f, 0f));
                    unit.GetComponent<AnimalController>().playerNumber = 1;
                    _grid[column, row] = unit;
                    Debug.Log("PLAYER 1 Generated on " + column + " " + row + " " + unit.name);
                    isAnimalGenerated = true;
                }

            }
            else if (_playerNumber == 2)
            {

                int rndAnimal = Random.Range(0, player2.GetDeck().Count);
                int row = 0;
                switch (player2.GetDeck()[rndAnimal].GetComponent<AnimalController>().animalType.animalRowType)
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
                int column = Random.Range(0, _columnSize);
                if (_grid[column, row] == null)
                {
                    GameObject unit = Instantiate(player2.GetDeck()[rndAnimal], new Vector3(column, 1f, row), Quaternion.Euler(0f, 180f, 0f));
                    unit.GetComponent<AnimalController>().playerNumber = 2;
                    _grid[column, row] = unit;
                    Debug.Log("PLAYER 2 Generated on " + column + " " + row + " " + unit.name);
                    isAnimalGenerated = true;
                }

            }
        }
    }

    public void CalculateMovesOnGrid(SwipeDirection turnDirection, byte _playerNumber)
    {
        byte k = 0;

        while (k < _columnSize || k < _rowSize)
        {
            //todo optimize
            switch (turnDirection)
            {
                case SwipeDirection.Up:
                    for (int j = _rowSize - 2; j >= 0; j--)
                    {
                        for (int i = 0; i <= _columnSize - 1; i++)
                        {
                            CalculateUniversal(i, j, 0, 1, k, _playerNumber);
                        }
                    }
                    break;
                case SwipeDirection.Down:
                    for (int j = 1; j <= _rowSize - 1; j++)
                    {
                        for (int i = 0; i <= _columnSize - 1; i++)
                        {
                            CalculateUniversal(i, j, 0, -1, k, _playerNumber);
                        }
                    }
                    break;
                case SwipeDirection.Left:
                    for (int i = 1; i <= _columnSize - 1; i++)
                    {
                        for (int j = 0; j <= _rowSize - 1; j++)
                        {
                            CalculateUniversal(i, j, -1, 0, k, _playerNumber);

                        }
                    }
                    break;
                case SwipeDirection.Right:
                    for (int i = _columnSize - 2; i >= 0; i--)
                    {
                        for (int j = 0; j <= _rowSize - 1; j++)
                        {
                            CalculateUniversal(i, j, 1, 0, k, _playerNumber);

                        }
                    }
                    break;
                default:
                    break;
            }

            k++;
        }
        isMoveCalculated = true;
    }

    public void MoveAllAnimals(SwipeDirection turnDirection, float turnTime, float _gameSpeed, byte _curPlayerNumber)
    {
        // Calculate All in Grid[,] k times (where k = grid size, e.g. 8)
        if (isMoveCalculated == false)
            CalculateMovesOnGrid(turnDirection, _curPlayerNumber);

        // Then move all units
        if (isAnimalGenerated == false)
            MoveUnitsByGrid(turnTime, _gameSpeed, _curPlayerNumber);
    }

    private void CalculateUniversal(int i, int j, int x, int y, byte k, byte _playerNumber)
    {
        // DEBUG FOR ALL CALCULATIONS OF MATRIX ON MOVE STAGE
        // Debug.Log("i, j = " + i + ", " + j + ", Object = " + _grid[i, j] + ", k = " + k);


        if (_grid[i, j] != null && _grid[i, j].CompareTag("Unit") && _grid[i, j].GetComponent<AnimalController>().playerNumber == _playerNumber)
        {

            int distance = _grid[i, j].GetComponent<AnimalController>().animalType.attackDistance;
            //Debug.Log("BEFORE CALCULATION WITH DISTANCE: " + _grid[i,j].name + ", i = " + i + ", j = " + j + ", x = " + x + ", y = " + y + ", distance = " + distance);
            if (i + (x * distance) <= (_columnSize - 1) && i + (x * distance) >= 0
                && j + (y * distance) <= (_rowSize - 1) && j + (y * distance) >= 0
                && _grid[i + (x * distance), j + (y * distance)] != null
                && _grid[i + (x * distance), j + (y * distance)].GetComponent<AnimalController>().playerNumber != _playerNumber)
            {
                return;
            }
            if (_grid[i + x, j + y] == null)
            {
                Debug.Log("TRY TO FIND: " + movableUnits.Any(z => z.UnitGO));
                if (movableUnits.Any(z => z.UnitGO == _grid[i, j]) == false)
                {
                    movableUnits.Add(new MovableUnit(_grid[i, j], new Vector3(i + x, 1f, j + y), 1));
                    Debug.Log("Add object: " + _grid[i, j].name + " Vector = " + movableUnits.First(z => z.UnitGO == _grid[i, j]).TargetPosition);
                }
                else
                {
                    movableUnits.First(z => z.UnitGO == _grid[i, j]).ChangeDestination(new Vector3(i + x, 1f, j + y));
                    Debug.Log("Change vector: " + _grid[i, j].name + " Vector = " + movableUnits.First(z => z.UnitGO == _grid[i, j]).TargetPosition);
                }

                _grid[i + x, j + y] = _grid[i, j];
                _grid[i, j] = null;
            }
            else if (_grid[i + x, j + y].GetComponent<AnimalController>().playerNumber == _playerNumber) // And if animalType is same -> Power Up, else stay
            {
                if (_grid[i + x, j + y].GetComponent<AnimalController>().animalType.name == _grid[i, j].GetComponent<AnimalController>().animalType.name
                    && _grid[i, j].GetComponent<AnimalController>().animalType.nextLevel != null
                    && _grid[i, j].GetComponent<AnimalController>().isLevelUped == false)
                {
                    Debug.Log("Level Up on k = " + k);

                    float newUnitHpPercent = (_grid[i, j].GetComponent<AnimalController>().curHealth + _grid[i + x, j + y].GetComponent<AnimalController>().curHealth) / 2 / _grid[i, j].GetComponent<AnimalController>().animalType.maxHP;
                    destroyableUnits.Add(_grid[i, j]);
                    destroyableUnits.Add(_grid[i + x, j + y]);
                    if (movableUnits.Any(z => z.UnitGO == _grid[i, j]) == false)
                        movableUnits.Add(new MovableUnit(_grid[i, j], new Vector3(i + x, 1f, j + y), 1));
                    else
                        movableUnits.First(z => z.UnitGO == _grid[i, j]).ChangeDestination(new Vector3(i + x, 1f, j + y));

                    float newHealth = (int)(newUnitHpPercent * _grid[i, j].GetComponent<AnimalController>().animalType.nextLevel.GetComponent<AnimalController>().animalType.maxHP);
                    Quaternion rotationBySide = Quaternion.Euler(0f, 0f, 0f);
                    if (_playerNumber == 2)
                    {
                        rotationBySide = Quaternion.Euler(0f, 180f, 0f);
                    }
                    instantiableUnits.Add(new InstatiableUnit(_grid[i, j].GetComponent<AnimalController>().animalType.nextLevel, new Vector3(i + x, 1f, j + y), newHealth, rotationBySide));
                    //!!!!!
                    instantiableUnits.Last().UnitGO.GetComponent<AnimalController>().isLevelUped = true;
                    instantiableUnits.Last().UnitGO.GetComponent<AnimalController>().playerNumber = _playerNumber;
                    _grid[i + x, j + y] = instantiableUnits.Last().UnitGO;
                    //_grid[i + x, j + y] = _grid[i, j].GetComponent<AnimalController>().animalType.nextLevel;
                    _grid[i, j] = null;
                }


            }
        }
    }
    public void AttackEnemy(SwipeDirection turnDirection, byte _curPlayerNumber)
    {
        List<GameObject> allUnitsOfThisPlayer = GameObject.FindGameObjectsWithTag("Unit").Where(z => z.GetComponent<AnimalController>().playerNumber == _curPlayerNumber).ToList();
        int x = 0, y = 0;
        switch (turnDirection)
        {
            case SwipeDirection.Up:
                x = 0;
                y = 1;
                break;
            case SwipeDirection.Down:
                x = 0;
                y = -1;
                break;
            case SwipeDirection.Left:
                x = -1;
                y = 0;
                break;
            case SwipeDirection.Right:
                x = 1;
                y = 0;
                break;
            default:
                break;
        }
        foreach (GameObject unit in allUnitsOfThisPlayer)
        {
            if (unit.GetComponent<AnimalController>().playerNumber == 1)
            {
                if (turnDirection == SwipeDirection.Up && (int)unit.transform.position.z >= _columnSize - unit.GetComponent<AnimalController>().animalType.attackDistance)
                {
                    // Attack Enemy King
                    GameObject enemyKing = GameObject.FindGameObjectsWithTag("King").Where(z => z.GetComponent<KingController>().playerNumber != _curPlayerNumber).First();
                    enemyKing.GetComponent<KingController>().curHP -= unit.GetComponent<AnimalController>().animalType.damage;

                    // Update King's HP in Canvas. Temporary solution!
                    enemyKing.GetComponent<KingController>().UpdateHpInCanvas();

                    if (enemyKing.GetComponent<KingController>().curHP <= 0)
                    {
                        // Win and Lose Game Screen
                        //GameOverScreen(_curPlayerNumber);
                    }
                    continue;
                }
                if (turnDirection == SwipeDirection.Down && (int)unit.transform.position.z == 0)
                {
                    continue;
                }
                if (turnDirection == SwipeDirection.Left && (int)unit.transform.position.x == 0)
                {
                    continue;
                }
                if (turnDirection == SwipeDirection.Right && (int)unit.transform.position.x == _columnSize - 1)
                {
                    continue;
                }
                //Check Enemy unit, and if find -> attack
                Debug.Log("Interaction debug: " + "pos x = " + (int)unit.transform.position.x + ", x = " + x + ", pos z = " + (int)unit.transform.position.z + ", y = " + y);

                int distance = unit.GetComponent<AnimalController>().animalType.attackDistance;
                if ((int)unit.transform.position.x + x * distance <= _columnSize - 1 && (int)unit.transform.position.x + x * distance >= 0
                    && (int)unit.transform.position.z + y * distance <= _rowSize - 1 && (int)unit.transform.position.z + y * distance >= 0
                    && _grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance] != null
                    && _grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance].GetComponent<AnimalController>().playerNumber != _curPlayerNumber)
                {
                    // Attack
                    _grid[(int)unit.transform.position.x, (int)unit.transform.position.z].GetComponent<AnimalController>().
                        Attack(_grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance].GetComponent<AnimalController>());
                    // Destroy enemy after attack, if hp <= 0
                    if (_grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance].GetComponent<AnimalController>().curHealth <= 0)
                    {
                        Destroy(_grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance]);
                        _grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance] = null;
                    }
                    continue;
                }
                if (_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y] != null
                    && _grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y].GetComponent<AnimalController>().playerNumber != _curPlayerNumber)
                {
                    // Attack
                    _grid[(int)unit.transform.position.x, (int)unit.transform.position.z].GetComponent<AnimalController>().
                        Attack(_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y].GetComponent<AnimalController>());
                    // Destroy enemy after attack, if hp <= 0
                    if (_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y].GetComponent<AnimalController>().curHealth <= 0)
                    {
                        Destroy(_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y]);
                        _grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y] = null;
                    }
                }
            }
            else if (unit.GetComponent<AnimalController>().playerNumber == 2)
            {
                if (turnDirection == SwipeDirection.Down && (int)unit.transform.position.z <= -1 + unit.GetComponent<AnimalController>().animalType.attackDistance)
                {
                    // Attack Enemy King
                    GameObject enemyKing = GameObject.FindGameObjectsWithTag("King").Where(z => z.GetComponent<KingController>().playerNumber != _curPlayerNumber).First();
                    enemyKing.GetComponent<KingController>().curHP -= unit.GetComponent<AnimalController>().animalType.damage;

                    // Update King's HP in Canvas. Temporary solution!
                    enemyKing.GetComponent<KingController>().UpdateHpInCanvas();

                    if (enemyKing.GetComponent<KingController>().curHP <= 0)
                    {
                        // Win and Lose Game Screen
                        //GameOverScreen(_curPlayerNumber);
                    }
                    continue;
                }
                if (turnDirection == SwipeDirection.Up && (int)unit.transform.position.z == _rowSize - 1)
                {
                    continue;
                }
                if (turnDirection == SwipeDirection.Left && (int)unit.transform.position.x == 0)
                {
                    continue;
                }
                if (turnDirection == SwipeDirection.Right && (int)unit.transform.position.x == _columnSize - 1)
                {
                    continue;
                }
                //Check Enemy unit, and if find -> attack

                int distance = unit.GetComponent<AnimalController>().animalType.attackDistance;
                if ((int)unit.transform.position.x + x * distance <= _columnSize - 1 && (int)unit.transform.position.x + x * distance >= 0
                    && (int)unit.transform.position.z + y * distance <= _rowSize - 1 && (int)unit.transform.position.z + y * distance >= 0
                    && _grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance] != null
                    && _grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance].GetComponent<AnimalController>().playerNumber != _curPlayerNumber)
                {
                    // Attack
                    _grid[(int)unit.transform.position.x, (int)unit.transform.position.z].GetComponent<AnimalController>().
                        Attack(_grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance].GetComponent<AnimalController>());
                    // Destroy enemy after attack, if hp <= 0
                    if (_grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance].GetComponent<AnimalController>().curHealth <= 0)
                    {
                        Destroy(_grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance]);
                        _grid[(int)unit.transform.position.x + x * distance, (int)unit.transform.position.z + y * distance] = null;
                    }
                    continue;
                }

                if (_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y] != null
                    && _grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y].GetComponent<AnimalController>().playerNumber != _curPlayerNumber)
                {
                    // Attack
                    _grid[(int)unit.transform.position.x, (int)unit.transform.position.z].GetComponent<AnimalController>().
                        Attack(_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y].GetComponent<AnimalController>());
                    // Destroy enemy after attack, if hp <= 0
                    if (_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y].GetComponent<AnimalController>().curHealth <= 0)
                    {
                        Destroy(_grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y]);
                        _grid[(int)unit.transform.position.x + x, (int)unit.transform.position.z + y] = null;
                    }
                }
            }
        }
    }
    public void MoveUnitsByGrid(float turnTime, float _gameSpeed, byte _curPlayerNumber)
    {
        foreach (var unit in movableUnits)
        {
            StartCoroutine(MoveToPosition(unit.UnitGO.transform, unit.TargetPosition, unit.Displacement, _gameSpeed));
        }
        movableUnits.Clear();
        StartCoroutine(WaitTurnTime(turnTime, _gameSpeed, _curPlayerNumber));
    }

    IEnumerator WaitTurnTime(float turnTime, float _gameSpeed, byte _curPlayerNumber)
    {
        yield return new WaitForSeconds(turnTime / _gameSpeed);
        foreach (var unit in destroyableUnits)
        {
            Destroy(unit);
        }
        destroyableUnits.Clear();
        foreach (var unit in instantiableUnits)
        {
            GameObject newUnit = Instantiate(unit.UnitGO, unit.Position, unit.Rotation);
            newUnit.GetComponent<AnimalController>().playerNumber = _curPlayerNumber;
            newUnit.GetComponent<AnimalController>().curHealth = unit.Health;
            newUnit.GetComponent<AnimalController>().isLevelUped = false;
            _grid[(int)unit.Position.x, (int)unit.Position.z] = newUnit;
        }
        instantiableUnits.Clear();
        isAnimalMoved = true;
        //turnStage = TurnStage.Interaction;
    }
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove, float _gameSpeed)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove * _gameSpeed;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return new WaitForEndOfFrame();
        }
    }
}
