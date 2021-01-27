using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage All Enemies", menuName = "Skills/Damage All Enemies")]
public class DamageAllUnits : Skill
{
    public override void SkillActivation(short playerNumber)
    {
        List<GameObject> enemyUnits = GameObject.FindGameObjectsWithTag("Unit").
            Where(z => z.GetComponent<AnimalController>().playerNumber != playerNumber).ToList();
        foreach (var unit in enemyUnits)
        {
            unit.GetComponent<AnimalController>().GetDamage(1f);
            if (unit.GetComponent<AnimalController>().curHealth <= 0)
            {
                Destroy(unit);
                Grid.instance.SetUnit((int)unit.transform.position.x, (int)unit.transform.position.z, null);
            }
        }
        Debug.Log("DAMAGE ALL!!!");
    }
}
