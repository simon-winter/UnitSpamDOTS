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
    private const float posThresholdSq = 0.025f;

    //private readonly Entity entity;

    private readonly TransformAspect unitTf;
    private readonly RefRO<MoveSpeed> speed;
    private readonly RefRO<Target> moveTarget;

    [BurstCompile]
    public void RotateToTarget(float deltaTime) {
        var rotTarget = Quaternion.LookRotation(moveTarget.ValueRO.Position - unitTf.WorldPosition);
        unitTf.WorldRotation = Quaternion.Slerp(unitTf.WorldRotation, rotTarget, speed.ValueRO.Angular * deltaTime);
    }

    [BurstCompile]
    public void MoveToTarget(float deltaTime) {
        if (math.distancesq(unitTf.LocalPosition, moveTarget.ValueRO.Position) > posThresholdSq) {
            float3 dir = math.normalize(moveTarget.ValueRO.Position - unitTf.LocalPosition);
            // 1 = aligned with target, 0 = perpendicular, -1 = target is behind unit
            // results in fancy backing up and turning around of units if needed,
            // also maxes speed out when aligned to target (use acos for linear scaling)
            var driveDirFactor = math.dot(dir, unitTf.Forward);            
            unitTf.LocalPosition += unitTf.Forward * driveDirFactor * deltaTime * speed.ValueRO.Linear;          
        }
    }
}
