using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
public partial struct MoveISystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        //SystemAPI.GetSingletonRW<RandomValue>()
        //foreach (MoveToTargetAspect moveToTarget in SystemAPI.Query<MoveToTargetAspect>()) {
        //    moveToTarget.Move(SystemAPI.Time.DeltaTime);
        //}
        float deltaTime = SystemAPI.Time.DeltaTime;        
        var jh = new MoveToTargetJob {
            deltaTime = deltaTime
        }.ScheduleParallel(state.Dependency);
        jh.Complete();
    }
}

[BurstCompile]
public partial struct MoveToTargetJob : IJobEntity
{
    public float deltaTime;
    [BurstCompile]
    public void Execute(MovementAspect ma) {
        ma.RotateToTarget(deltaTime);
        ma.MoveToTarget(deltaTime);
    }
}