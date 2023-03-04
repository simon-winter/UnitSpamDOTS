using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct SpawnZone : IComponentData
{
    public Entity UnitPrefab;

    public float SpawnTimer;
    public float elapsedTime;

    public float3 SpawnTarget;
}
