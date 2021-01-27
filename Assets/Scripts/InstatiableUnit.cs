using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class InstatiableUnit
{
    public GameObject UnitGO { get; private set; }
    public Vector3 Position { get; private set; }
    public float Health { get; private set; }
    public Quaternion Rotation { get; private set; }

    public InstatiableUnit(GameObject unitGO, Vector3 position, float health, Quaternion rotation)
    {
        this.UnitGO = unitGO;
        this.Position = position;
        this.Health = health;
        this.Rotation = rotation;
    }
}
