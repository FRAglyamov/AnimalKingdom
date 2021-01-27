using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Random Unit", menuName = "Skills/Damage Random Unit")]
public class DamageRandomUnit : Skill
{
    public override void SkillActivation(short playerNumber)
    {
        List<GameObject> enemyUnits = GameObject.FindGameObjectsWithTag("Unit").
            Where(z => z.GetComponent<AnimalController>().playerNumber != playerNumber).ToList();
        int rnd = Random.Range(0, enemyUnits.Count);
        GameObject unit = enemyUnits.ElementAt(rnd);
        unit.GetComponent<AnimalController>().GetDamage(3f);
        if (unit.GetComponent<AnimalController>().curHealth <= 0)
        {
            Destroy(unit);
            Grid.instance.SetUnit((int)unit.transform.position.x, (int)unit.transform.position.z, null);
        }
        Debug.Log("Damage Random Unit");
    }
}
