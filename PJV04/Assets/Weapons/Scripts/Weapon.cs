using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Unarmed = 0,
    OneHanded = 1
}

public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public int damage;
    string name;

    private void Awake()
    {
        name = GetComponent<MeshFilter>().name.Split()[0];
    }
}
