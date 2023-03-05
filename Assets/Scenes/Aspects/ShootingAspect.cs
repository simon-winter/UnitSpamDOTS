using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public readonly partial struct ShootingAspect : IAspect
{
    private const float targetOffsetThreshold = 0.02f;

    private readonly RefRW<Weapon> weapon;
    private readonly TransformAspect weaponTf;
    private readonly RefRO<Target> target;

    [BurstCompile]
    public void RotateToTarget(float deltaTime) {
        var rotTarget = Quaternion.LookRotation(target.ValueRO.Position - weaponTf.WorldPosition);
        weaponTf.WorldRotation = Quaternion.Slerp(weaponTf.WorldRotation, rotTarget, weapon.ValueRO.AimSpeed * deltaTime);
    }

    [BurstCompile]
    public void ShootAtTarget(float deltaTime,  EntityCommandBuffer.ParallelWriter buffer) {

        if (weapon.ValueRW.coolDownTimer < weapon.ValueRW.ReloadTime) {
            weapon.ValueRW.coolDownTimer += deltaTime;
            return;
        }        

        // Not squared, for easier handling in this demo
        if (math.distance(weaponTf.LocalPosition, target.ValueRO.Position) <= weapon.ValueRO.Range) {
            var targetOffset = math.dot(weaponTf.Forward, target.ValueRO.Position - weaponTf.WorldPosition);
            if (targetOffset < targetOffsetThreshold) {
                var shootDir = math.normalize(target.ValueRO.Position - weaponTf.LocalPosition);
                var projectile = buffer.Instantiate(0, weapon.ValueRO.Projectile);
                buffer.SetComponent(0, projectile, LocalTransform.FromRotation(
                    Quaternion.FromToRotation(Vector3.forward, shootDir)));

                weapon.ValueRW.coolDownTimer = 0;
            }
        }       
    }
}
