using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit
{
    public GameObject UnitGO { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public byte Displacement { get; private set; }
    public MovableUnit(GameObject unitGO, Vector3 targetPosition, byte displacement)
    {
        this.UnitGO = unitGO;
        this.TargetPosition = targetPosition;
        this.Displacement = displacement;
    }
    public void ChangeDestination(Vector3 newTargetPosition)
    {
        this.TargetPosition = newTargetPosition;
        this.Displacement += 1;
    }
}
