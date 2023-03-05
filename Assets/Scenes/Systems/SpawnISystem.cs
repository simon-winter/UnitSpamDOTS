using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public partial struct SpawnISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        //  state.World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var deltaTime = SystemAPI.Time.DeltaTime;
        // Can't get singleton in OnCreate, whats the lifetime of a CommandBufferSystem?
        var cmdBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        var rndSeed = SystemAPI.GetSingletonRW<RandomValue>();

        var jh = new SpawnJob {
            buffer = cmdBuffer,
            deltaTime = deltaTime,
            // rndSeed is sadly not random between jobs, as it seems its 1 job with many execute functions,
            // so all spawnZones have same randomValues within a frame
            // could move random to SpawnZone, but thats unnecessary for this project
            rndSeed = rndSeed.ValueRW.Value.NextUInt()
        }.ScheduleParallel(state.Dependency);
        jh.Complete();
    }
}

[BurstCompile]
public partial struct SpawnJob : IJobEntity
{
    public float deltaTime;
    public EntityCommandBuffer.ParallelWriter buffer;
    public uint rndSeed;

    [BurstCompile]
    public void Execute(RefRW<SpawnZone> spawnZone, TransformAspect tf, PostTransformScale scale) {
        spawnZone.ValueRW.elapsedTime += deltaTime;
        if (spawnZone.ValueRW.elapsedTime >= spawnZone.ValueRW.SpawnTimer) {
            spawnZone.ValueRW.elapsedTime -= spawnZone.ValueRW.SpawnTimer;
            var rnd = new Random(rndSeed);
            // calculate random spawn position in spawn Zone
            var x_max = tf.WorldPosition.x + (scale.Value.c0.x / 2f);
            var x_min = tf.WorldPosition.x - (scale.Value.c0.x / 2f);
            var z_max = tf.WorldPosition.z + (scale.Value.c2.z / 2f);
            var z_min = tf.WorldPosition.z - (scale.Value.c2.z / 2f);

            var x = rnd.NextFloat(x_min, x_max);
            var z = rnd.NextFloat(z_min, z_max);

            var spawnedEntity = buffer.Instantiate(0, spawnZone.ValueRW.UnitPrefab);
            buffer.SetComponent(1, spawnedEntity, LocalTransform.FromPosition(x, 0, z));
            buffer.SetComponent(1, spawnedEntity, new Target {
                Position = spawnZone.ValueRO.SpawnTarget
            });
        }
    }
}