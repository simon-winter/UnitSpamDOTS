
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class SpeedAuthoring : MonoBehaviour
{
    public float linear;
    public float angular;
}

public class SpeedBaker : Baker<SpeedAuthoring>
{
    public override void Bake(SpeedAuthoring authoring) {
        AddComponent(new Speed {
            linear = authoring.linear,
            angular = authoring.angular
        });
    }
}