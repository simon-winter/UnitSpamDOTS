using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct SpawnZone : IComponentData
{
    public Entity UnitPrefab;
    public float SpawnTimer;
    public float elapsedTime;
}
