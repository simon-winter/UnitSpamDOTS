using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct MoveSpeed : IComponentData
{
    public float Linear;
    public float Angular;
}
