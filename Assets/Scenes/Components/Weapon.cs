using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct Weapon : IComponentData
{
    public Entity Projectile; 

    public int Damage;
    public float Range;
    public float AimSpeed;
    public float ReloadTime;

    public float coolDownTimer;
}
