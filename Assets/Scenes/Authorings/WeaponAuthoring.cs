
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class WeaponAuthoring : MonoBehaviour
{
    public GameObject Projectile;

    public float Range;
    public float AimSpeed;

    public float ReloadTime;
    public float coolDownTimer;

    public int Damage;
}

public class WeaponBaker : Baker<WeaponAuthoring>
{
    public override void Bake(WeaponAuthoring authoring) {
        AddComponent(new Weapon {
            Projectile = GetEntity(authoring.Projectile),
            Range = authoring.Range,
            AimSpeed = authoring.AimSpeed,
            ReloadTime = authoring.ReloadTime,
            coolDownTimer = authoring.coolDownTimer,
            Damage = authoring.Damage
        });
    }
}