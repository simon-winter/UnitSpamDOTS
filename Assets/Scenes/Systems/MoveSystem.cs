using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

public partial struct MoveISystem : ISystem
{
    public void OnUpdate(ref SystemState state) {        
        foreach(MoveToTargetAspect moveToTarget in SystemAPI.Query<MoveToTargetAspect>()) {
            moveToTarget.Move(SystemAPI.Time.DeltaTime);
        }
    }
}
