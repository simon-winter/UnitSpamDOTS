using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

public partial class CameraSystem : SystemBase
{
    protected override void OnCreate() {
    }

    protected override void OnUpdate() {

        foreach ((DotsCamera cam, TransformAspect tf) in SystemAPI.Query<DotsCamera, TransformAspect>()) {
            var scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta != 0) {
                var pos = tf.WorldTransform.Position;

                var zoomDelta = pos.y + (cam.ScrollSpeed * -scrollDelta);
                var y = math.clamp(zoomDelta, cam.MinScrollLevel, cam.MaxScrollLevel);

                // Zooming out, no lateral camera movement
                if (scrollDelta < 0) {
                    tf.LocalTransform = LocalTransform.FromPosition(pos.x, y, pos.z);
                }
                else {  // When zooming in, move middle of screen towards scroll target
                    Vector3 scrollTarget;
                    Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Ray centerScreenRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    // raycast for scrolling target, incase we introduce height to the map
                    RaycastHit hit;
                    if (Physics.Raycast(mouseRay, out hit, 100, LayerMask.GetMask("Ground"))) {
                        scrollTarget = hit.point;
                    }
                    else {
                        scrollTarget = mouseRay.GetPoint(pos.y);
                    }
                    // shift center of screen towards scroll target, rather then camera pos,
                    // so it can be tilted and scrolling still matches focus point
                    var scrollDir = scrollTarget - centerScreenRay.GetPoint(pos.y);


                    var lerpFactor = cam.ScrollCenterSensitivity * SystemAPI.Time.DeltaTime;
                    var x = math.lerp(pos.x, pos.x + scrollDir.x, lerpFactor);
                    var z = math.lerp(pos.z, pos.z + scrollDir.z, lerpFactor);

                    x = math.clamp(x, -cam.LimitX, cam.LimitX);
                    z = math.clamp(z, -cam.LimitZ, cam.LimitZ);

                    tf.LocalTransform = LocalTransform.FromPosition(x, y, z);
                }
            }
        }
    }
}

