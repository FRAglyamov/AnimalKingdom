using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public short initialCD;
    public short CD;
    public abstract void SkillActivation(short playerNumber);
    public Sprite skillIcon;
}
