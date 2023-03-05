using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial struct RandomValue : IComponentData
{
    public Unity.Mathematics.Random Value;   
}
