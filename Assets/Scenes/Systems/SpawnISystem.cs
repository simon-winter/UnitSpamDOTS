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
        // Can't get singleton in OnCreate, whats the lifetime of a CommandBufferSystem?
        var cmdBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        var deltaTime = SystemAPI.Time.DeltaTime;

        var rndSeed = SystemAPI.GetSingletonRW<RandomValue>();

        var jh = new SpawnJob {
            buffer = cmdBuffer,
            deltaTime = deltaTime,
            // not sure why, but this still generates all jobs with same seed,
            // for different random positions add Random to each spawnZone component
            rndSeed = rndSeed.ValueRW.value.NextUInt() 
        }.Schedule(state.Dependency);
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
            var x_max = tf.LocalPosition.x + (scale.Value.c0.x / 2);
            var x_min = tf.LocalPosition.x - (scale.Value.c0.x / 2);
            var z_max = tf.LocalPosition.z + (scale.Value.c2.z / 2);
            var z_min = tf.LocalPosition.z - (scale.Value.c2.z / 2);

            var x = rnd.NextFloat(x_min, x_max);
            var z = rnd.NextFloat(z_min, z_max);

            var spawnedEntity = buffer.Instantiate(0, spawnZone.ValueRW.UnitPrefab);
            buffer.SetComponent(1, spawnedEntity, new LocalTransform {
                Position = new float3(x, 1, z),
                Scale = 1,
            });
        }
    }
}