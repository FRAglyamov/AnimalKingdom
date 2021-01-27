using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalRowType
{
    Front,
    Middle,
    Back
}

[CreateAssetMenu(fileName = "New AnimalType", menuName = "AnimalType", order = 51)]
public class AnimalType : ScriptableObject
{

    public AnimalRowType animalRowType;
    public float maxHP;
    public int attackDistance;
    public int damage;

    public GameObject nextLevel;
}
