using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MoveTargetAuthoring : MonoBehaviour
{
    public float3 value;

}

public class MoveTargetBaker : Baker<MoveTargetAuthoring>
{
    public override void Bake(MoveTargetAuthoring authoring) {
        AddComponent(new MoveTarget {
            value = authoring.value
        });
    }
}
