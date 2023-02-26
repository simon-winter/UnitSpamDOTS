using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct MoveToTargetAspect : IAspect
{
    private readonly Entity entity;

    private readonly TransformAspect transform;
    private readonly RefRO<Speed> speed;
    private readonly RefRW<MoveTarget> moveTarget;

    public void Move(float deltaTime) {
        if (math.distance(transform.LocalPosition, moveTarget.ValueRO.value) > 0.1) {
            float3 dir = math.normalize(moveTarget.ValueRO.value - transform.LocalPosition);
            transform.LocalPosition += dir * deltaTime * speed.ValueRO.value;
        }      
    }
}
