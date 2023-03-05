
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class RandomValueAuthoring : MonoBehaviour
{
    public uint Seed; 
}

public class RandomValueBaker : Baker<RandomValueAuthoring>
{
    public override void Bake(RandomValueAuthoring authoring) {
        AddComponent(new RandomValue {
            Value = new Unity.Mathematics.Random(authoring.Seed)
        });
    }
}