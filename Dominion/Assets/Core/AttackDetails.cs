using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct KatanaAttackDetails
{
    public string attackName;
    public string attackDamage;
    public float knockbackStrength;
    public Vector3 knockbackDirection;
}