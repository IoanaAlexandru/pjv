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

    public Transform muzzle;
    public GameObject projectile;

    IEnumerator shooting;
    RaycastHit? hit;

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

    public void StartShooting(RaycastHit hit)
    {
        this.hit = hit;
        if (shooting == null)
        {
            shooting = ShootCoroutine();
            StartCoroutine(shooting);
        }
    }

    public void StopShooting()
    {
        StopCoroutine(shooting);
        shooting = null;
    }


    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            var newProjectile = Instantiate(projectile, muzzle);
            var rigidbody = newProjectile.GetComponent<Rigidbody>();
            rigidbody.velocity = WeaponController.instance.fpsCamera.transform.forward * 5f;
            //if (hit != null)
            //{
            //    rigidbody.velocity = (((RaycastHit)hit).transform.position - newProjectile.transform.position).normalized * 5f;
            //} else
            //{
            //    rigidbody.velocity = WeaponController.instance.fpsCamera.transform.forward * 5f;
            //}
            rigidbody.useGravity = false;
            yield return new WaitForSeconds(1);
        }
    }
}
