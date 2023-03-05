using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowCameraEntity : MonoBehaviour
{
    private Entity camera;

    void Start()
    {
        var entityArr = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(DotsCamera))
            .ToEntityArray(Unity.Collections.Allocator.Temp);
        if (entityArr.Length == 0) {
            Debug.LogError("Can't find camera entity!");
        }
        else {
            camera = entityArr[0];
        }
    }

    void LateUpdate()
    {
        if (camera != null) {
            var entityPos = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(camera).Position;
            transform.position = entityPos;
        }
    }

}
