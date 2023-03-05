
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MoveSpeedAuthoring : MonoBehaviour
{
    public float Linear;
    public float Angular;
}

public class MoveSpeedBaker : Baker<MoveSpeedAuthoring>
{
    public override void Bake(MoveSpeedAuthoring authoring) {
        AddComponent(new MoveSpeed {
            Linear = authoring.Linear,
            Angular = authoring.Angular
        });
    }
}