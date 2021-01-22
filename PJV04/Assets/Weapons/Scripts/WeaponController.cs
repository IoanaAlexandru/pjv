using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject hand;
    public GameObject[] weapons;
    public Texture2D unarmed;

    GameObject weapon;
    Animator anim;

    // singleton
    public static WeaponController instance;

    void Awake()
    {
        instance = this;
    }

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
                    Unequip();
                }
                else
                {
                    if (weapons.Length >= i)
                    {
                        Equip(weapons[i - 1]);
                    }
                    else
                    {
                        Debug.Log($"No weapon to equip in slot {i}.");
                    }
                }
            }
        }
    }

    void Equip(GameObject newWeapon)
    {
        Destroy(weapon);
        weapon = Instantiate(newWeapon, hand.transform);
        anim.SetInteger("WeaponType", (int)weapon.GetComponent<Weapon>().type);
    }

    void Unequip()
    {
        Destroy(weapon);
        weapon = null;
        anim.SetInteger("WeaponType", (int)WeaponType.Unarmed);
    }


    private void OnGUI()
    {
        int size = 60, padding = 10;
        int weaponBarSize = 10 * (size + padding);
        for (var i = 0; i < 10; i++)
        {
            if (i == 9)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - weaponBarSize / 2 + (size + padding) * i, Screen.height - size - padding, size, size), unarmed))
                {
                    Unequip();
                }

            }
            else if (weapons.Length <= i)
            {
                GUI.Button(new Rect(Screen.width / 2 - weaponBarSize / 2 + (size + padding) * i, Screen.height - size - padding, size, size), "");
            }
            else
            {
                var name = weapons[i].GetComponent<Weapon>().name;
                var thumbnail = UnityEditor.AssetPreview.GetAssetPreview(weapons[i]);
                if (GUI.Toggle(new Rect(Screen.width / 2 - weaponBarSize / 2 + (size + padding) * i, Screen.height - size - padding, size, size), weapon == weapons[i], new GUIContent(thumbnail, name), GUI.skin.button))
                {
                    Equip(weapons[i]);
                }

            }
            GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 0f);
            GUI.TextArea(new Rect(Screen.width / 2 - weaponBarSize / 2 + (size + padding) * i, Screen.height - size - padding, size, size), $"{(i + 1) % 10}");
            GUI.backgroundColor = Color.white;
        }
    }
}
