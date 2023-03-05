using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public partial struct ShootISystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var cmdBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
          .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        var jh = new ShootTargetJob {
            deltaTime = deltaTime,
            buffer = cmdBuffer
        }.ScheduleParallel(state.Dependency);
        jh.Complete();
    }
}

[BurstCompile]
public partial struct ShootTargetJob : IJobEntity
{
    public float deltaTime;
    public EntityCommandBuffer.ParallelWriter buffer;
    [BurstCompile]
    public void Execute(ShootingAspect sa) {
        sa.RotateToTarget(deltaTime);
        sa.ShootAtTarget(deltaTime, buffer);
    }
}