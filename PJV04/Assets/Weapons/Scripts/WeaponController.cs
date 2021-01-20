using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject hand;
    public GameObject[] weapons;

    GameObject weapon;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetBool("Attack", true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            anim.SetBool("Attack", false);
        }

        for (var i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                if (i == 0)
                {
                    // Unarmed
                    Destroy(weapon);
                    weapon = null;
                    anim.SetInteger("WeaponType", (int)WeaponType.Unarmed);
                }
                else
                {
                    if (weapons.Length >= i)
                    {
                        Destroy(weapon);
                        weapon = Instantiate(weapons[i - 1], hand.transform);
                        anim.SetInteger("WeaponType", (int)weapon.GetComponent<Weapon>().type);
                    }
                    else
                    {
                        Debug.Log($"No weapon to equip in slot {i}.");
                    }
                }
            }
        }
    }
}
