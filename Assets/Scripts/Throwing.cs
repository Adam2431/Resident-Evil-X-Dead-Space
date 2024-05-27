using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
//using static System.IO.Enumeration.FileSystemEnumerable<TResult>;

public class ThrowingTutorial : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject HandGrenadeToThrow;
    public GameObject FlashGrenadeToThrow;
    private Animator animator;

    public int throwable;

    [Header("Settings")]
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    private GameObject projectile;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if((GameLogic.instance.currentWeapons[ThirdPersonController.instance._input.weaponIndex].Equals("Hand Grenade") || GameLogic.instance.currentWeapons[ThirdPersonController.instance._input.weaponIndex].Equals("Flash Grenade")) && ThirdPersonController.instance._input.shooting && ThirdPersonController.instance._input.aim && Time.timeScale == 1) // check if we have grenade and if we are ready to throw
        {
            ThirdPersonController.instance._input.shooting = false;
            animator.SetBool("isThrowing", true);
            Throw();
        }
    }

    private void Throw()
    {

        // instantiate object to throw
        if(throwable == 1)
        {
            projectile = Instantiate(HandGrenadeToThrow, attackPoint.position, cam.rotation);
        }

        else if (throwable == 2)
        {
            projectile = Instantiate(FlashGrenadeToThrow, attackPoint.position, cam.rotation);
        }

        if (GameLogic.instance.currentWeapons[ThirdPersonController.instance._input.weaponIndex].Equals("Hand Grenade"))
            GameLogic.instance.Inventory.GetComponent<Inventory>().RemoveFromInventory(GameLogic.instance.Inventory.GetComponent<Inventory>().scriptableObjects[4]);
        else if (GameLogic.instance.currentWeapons[ThirdPersonController.instance._input.weaponIndex].Equals("Flash Grenade"))
            GameLogic.instance.Inventory.GetComponent<Inventory>().RemoveFromInventory(GameLogic.instance.Inventory.GetComponent<Inventory>().scriptableObjects[5]);

        GameLogic.instance.currentWeapons.RemoveAt(ThirdPersonController.instance._input.weaponIndex);
        ThirdPersonController.instance._input.weaponIndex--;
        ThirdPersonController.instance.SwitchWeapon();

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }

}