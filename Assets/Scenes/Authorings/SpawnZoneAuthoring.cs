
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnZoneAuthoring : MonoBehaviour
{
    public GameObject UnitPrefab;

    public float SpawnTimer;
    public float elapsedTime;

    public GameObject SpawnTarget;
}

public class SpawnZoneBaker : Baker<SpawnZoneAuthoring>
{
    public override void Bake(SpawnZoneAuthoring authoring) {
        AddComponent(new SpawnZone {
            UnitPrefab = GetEntity(authoring.UnitPrefab),
            SpawnTimer = authoring.SpawnTimer,
            elapsedTime = authoring.elapsedTime,
            SpawnTarget = GetComponent<Transform>(authoring.SpawnTarget).position
        });
    }
}