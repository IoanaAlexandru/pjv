using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Unarmed = 0,
    OneHanded = 1,
    Gun = 2
}

public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public int damage;
    string name;

    public Weapon(WeaponType type, int damage, string name)
    {
        this.type = type;
        this.damage = damage;
        this.name = name;
    }

    private void Awake()
    {
        name = GetComponent<MeshFilter>().name.Split()[0];
    }
}
