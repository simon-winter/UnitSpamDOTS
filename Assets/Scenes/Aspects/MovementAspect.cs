using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public readonly partial struct MovementAspect : IAspect
{
    private const float PosThresholdSq = 0.025f;

    private readonly Entity entity;

    private readonly TransformAspect unitTf;
    private readonly RefRO<Speed> speed;
    private readonly RefRW<Target> moveTarget;

    [BurstCompile]
    public void RotateToTarget(float deltaTime) {
        var rotTarget = Quaternion.LookRotation(moveTarget.ValueRO.position - unitTf.WorldPosition);
        unitTf.WorldRotation = Quaternion.Slerp(unitTf.WorldRotation, rotTarget, speed.ValueRO.angular * deltaTime);
    }

    
    public void MoveToTarget(float deltaTime) {
        if (math.distancesq(unitTf.LocalPosition, moveTarget.ValueRO.position) > PosThresholdSq) {
            float3 dir = math.normalize(moveTarget.ValueRO.position - unitTf.LocalPosition);
            var driveDirFactor = math.dot(dir, unitTf.Forward);
            unitTf.LocalPosition += unitTf.Forward * driveDirFactor * deltaTime * speed.ValueRO.linear;
            Debug.Log(driveDirFactor);
        }
    }
}
