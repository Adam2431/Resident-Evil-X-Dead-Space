using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using static System.IO.Enumeration.FileSystemEnumerable<TResult>;

public class EnemyThrowing : MonoBehaviour
{
    [Header("References")]

    public Transform attackPoint;
    public GameObject Axe;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    private GameObject projectile;

    public void Throw()
    {

        // instantiate object to throw
        projectile = Instantiate(Axe, attackPoint.position, attackPoint.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = (GetComponent<Enemy>().player.position - attackPoint.transform.position + new Vector3(0, 1.5f, 0)).normalized;

        //RaycastHit hit;

        //if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        //{
        //    forceDirection = (hit.point - attackPoint.position).normalized;
        //}

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;


        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
        Destroy(attackPoint.gameObject);
    }
}