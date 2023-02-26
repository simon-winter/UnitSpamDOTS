
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class RandomValueAuthoring : MonoBehaviour
{
    public uint seed; 
}

public class RandomValueBaker : Baker<RandomValueAuthoring>
{
    public override void Bake(RandomValueAuthoring authoring) {
        AddComponent(new RandomValue {
            value = new Unity.Mathematics.Random(authoring.seed)
        });
    }
}