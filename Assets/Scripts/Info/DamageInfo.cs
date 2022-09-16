using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public int damage;
    public DamageType type;
    public Character attacker;

    public DamageInfo(int _damage, DamageType _type, Character _attacker)
    {
        damage = _damage;
        type = _type;
        attacker = _attacker;
    }
}

public enum DamageType { DEFAULT = 0, HAZARD = 1 }