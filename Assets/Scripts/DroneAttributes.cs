using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Resources;

[DisallowMultipleComponent]
public class DroneAttributes : MonoBehaviour {

    public uint HitPoints = 100;
    public uint DecomissionHP = 20;
    
    public bool NeedsDecomission { get; protected set; }

    public void TakeDamage( uint damageHP ) {
        HitPoints = System.Math.Max(0, HitPoints - damageHP);

        if (HitPoints < DecomissionHP)
            NeedsDecomission = true;

    }

    public void Start() {
    }
}
