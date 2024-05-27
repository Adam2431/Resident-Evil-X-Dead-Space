using System;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    public bool isAuto;
    public float damage;
    public float shotInterval;
    public int range;
    public int capacity;
    public int maxCapacity;

    private float timeSinceLastShot;
    private bool shotFired;

    [SerializeField] private GameLogic GameLogic;

    public ParticleSystem muzzleFlash;

    public bool CanReload()
    {
        bool doesHaveAmmo = false;

        if (name.Equals("Pistol"))
            doesHaveAmmo = GameLogic.GetComponent<GameLogic>().pistolAmmo > 0;
        else if (name.Equals("Rifle"))
            doesHaveAmmo = GameLogic.GetComponent<GameLogic>().rifleAmmo > 0;
        else if (name.Equals("Shotgun"))
            doesHaveAmmo = GameLogic.GetComponent<GameLogic>().shotgunAmmo > 0;
        else if (name.Equals("Magnum"))
            doesHaveAmmo = GameLogic.GetComponent<GameLogic>().magnumAmmo > 0;

        return capacity < maxCapacity && doesHaveAmmo;
    }

    public int Reload(int ammo)
    {
        if (name == "Shotgun")
        {
            if (ammo > 0)
            {
                capacity += 1;
            }
            return capacity;
        }
        else
        {
            int emptyInWeapon = maxCapacity - capacity;
            int refillAmount = Math.Min(emptyInWeapon, 30);
            refillAmount = Math.Min(refillAmount, ammo);
            capacity += refillAmount;
            return capacity;
        }
    }

    public void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    public bool Shoot()
    {
        if ((capacity > 0 || Cheats.instance.infiniteAmmoActive) && !shotFired)
        {
            capacity--;
            if(capacity < 0)
            {
                capacity = 0;
            }
            shotFired = true;
            PlayMuzzleFlash();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Update()
    {
        if (shotFired)
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > shotInterval)
            {
                timeSinceLastShot = 0;
                shotFired = false;
            }
        }
    }
}
