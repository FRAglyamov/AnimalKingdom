using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New KingType", menuName = "KingType", order = 52)]
public class KingType : ScriptableObject
{
    public float maxHP;
    public Skill firstSkill;
    public Skill secondSkill;
}
