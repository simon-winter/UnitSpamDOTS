using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct Speed : IComponentData
{
    public float linear;
    public float angular;
}
