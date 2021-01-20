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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
