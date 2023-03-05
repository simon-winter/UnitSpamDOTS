using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial struct DotsCamera : IComponentData
{    
    public float ScrollCenterSensitivity;
    public float ScrollSpeed;
    public float MinScrollLevel;
    public float MaxScrollLevel;
    public float LimitX;
    public float LimitZ;
}
