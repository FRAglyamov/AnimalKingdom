using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal All Units", menuName = "Skills/Heal All Units")]
public class HealAllUnits : Skill
{
    public override void SkillActivation(short playerNumber)
    {
        List<GameObject> friendlyUnits = GameObject.FindGameObjectsWithTag("Unit").
            Where(z => z.GetComponent<AnimalController>().playerNumber == playerNumber).ToList();
        foreach (var unit in friendlyUnits)
        {
            unit.GetComponent<AnimalController>().GetHeal(1);
            //if (unit.GetComponent<AnimalController>().curHealth != unit.GetComponent<AnimalController>().animalType.maxHP)
            //{
            //    unit.GetComponent<AnimalController>().curHealth += 1;
            //    unit.GetComponent<AnimalController>().UpdateHealthText();
            //}
        }
        Debug.Log("HEAL ALL!!!");
    }
}
