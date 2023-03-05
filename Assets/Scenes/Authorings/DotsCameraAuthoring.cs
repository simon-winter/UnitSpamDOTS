using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class DotsCameraAuthoring : MonoBehaviour
{
    public float ScrollCenterSensitivity;
    public float ScrollSpeed;
    public float MinScrollLevel;
    public float MaxScrollLevel;
    public float LimitX;
    public float LimitZ;
}

public class DotsCameraBaker : Baker<DotsCameraAuthoring>
{
    public override void Bake(DotsCameraAuthoring authoring) {
        AddComponent(new DotsCamera {
            ScrollCenterSensitivity = authoring.ScrollCenterSensitivity,
            ScrollSpeed = authoring.ScrollSpeed,
            MinScrollLevel = authoring.MinScrollLevel,
            MaxScrollLevel = authoring.MaxScrollLevel,
            LimitX = authoring.LimitX,
            LimitZ = authoring.LimitZ
        });
    }
}
