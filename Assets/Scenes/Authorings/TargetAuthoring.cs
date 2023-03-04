using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TargetAuthoring : MonoBehaviour
{
    public float3 position;

}

public class TargetBaker : Baker<TargetAuthoring>
{
    public override void Bake(TargetAuthoring authoring) {
        AddComponent(new Target {
            position = authoring.position
        });
    }
}
