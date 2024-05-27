using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeonAnimationHandler : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject rifle = animator.GetComponent<ThirdPersonController>().rifle;
        GameObject rifle1 = animator.GetComponent<ThirdPersonController>().rifle1;
        GameObject shotgun = animator.GetComponent<ThirdPersonController>().shotgun;
        GameObject shotgun1 = animator.GetComponent<ThirdPersonController>().shotgun1;

        if (stateInfo.IsName("Rifle Reload") || stateInfo.IsName("Rifle Putaway") || stateInfo.IsName("Rifle Sprint Putaway") || stateInfo.IsName("Rifle Raise") || stateInfo.IsName("Rifle Sprint Raise"))
        {
            rifle.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            rifle1.SetActive(false);
        }

        else if (stateInfo.IsName("Shotgun Reload") || stateInfo.IsName("Shotgun Putaway") || stateInfo.IsName("Shotgun Sprint Putaway") || stateInfo.IsName("Shotgun Raise") || stateInfo.IsName("Shotgun Sprint Raise"))
        {
            shotgun.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            shotgun1.SetActive(false);
        }

        if(stateInfo.IsName("Shotgun Raise") || stateInfo.IsName("Shotgun Sprint Raise"))
        {
            rifle.SetActive(false);
            rifle1.SetActive(false);
        }

        else if (stateInfo.IsName("Rifle Raise") || stateInfo.IsName("Rifle Sprint Raise"))
        {
            shotgun.SetActive(false);
            shotgun1.SetActive(false);
        }

        else if(stateInfo.IsName("Pistol Raise") || stateInfo.IsName("Pistol Sprint Raise") || stateInfo.IsName("Magnum Raise") || stateInfo.IsName("Magnum Sprint Raise") || stateInfo.IsName("Grenade Walk Sprint Blend Tree"))
        {
            rifle.SetActive(false);
            rifle1.SetActive(false);
            shotgun.SetActive(false);
            shotgun1.SetActive(false);
        }
    }

    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    GameObject rifle = animator.GetComponent<ThirdPersonController>().rifle;
    //    GameObject rifle1 = animator.GetComponent<ThirdPersonController>().rifle1;
    //    GameObject shotgun = animator.GetComponent<ThirdPersonController>().shotgun;
    //    GameObject shotgun1 = animator.GetComponent<ThirdPersonController>().shotgun1;
    //    if ((stateInfo.IsName("Rifle Walk Sprint Blend Tree") && animator.GetComponent<ThirdPersonController>()._speed > 0) || stateInfo.IsName("Rifle Aiming Blend Tree"))
    //    {
    //        rifle.transform.localScale = new Vector3(0, 0, 0);
    //        rifle1.SetActive(true);
    //    }

    //    else if ((stateInfo.IsName("Shotgun Walk Sprint Blend Tree") && animator.GetComponent<ThirdPersonController>()._speed > 0) || stateInfo.IsName("Shotgun Aiming Blend Tree"))
    //    {
    //        shotgun.transform.localScale = new Vector3(0, 0, 0);
    //        shotgun1.SetActive(true);
    //    }
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject rifle = animator.GetComponent<ThirdPersonController>().rifle;
        GameObject rifle1 = animator.GetComponent<ThirdPersonController>().rifle1;
        GameObject shotgun = animator.GetComponent<ThirdPersonController>().shotgun;
        GameObject shotgun1 = animator.GetComponent<ThirdPersonController>().shotgun1;


        if (stateInfo.IsTag("getup"))
        {
            animator.GetComponent<ThirdPersonController>().isBeingGrappled = false;
            animator.SetBool("isGrappled", false);
            animator.SetBool("grappleExecuted", false);
        }

        else if (stateInfo.IsName("Kick"))
        {
            animator.GetComponent<ThirdPersonController>().isBeingGrappled = false;
            animator.SetBool("isGrappled", false);
            animator.SetBool("defendGrapple", false);
        }

        else if (stateInfo.IsName("Pistol Flinch") || stateInfo.IsName("Shotgun Flinch") || stateInfo.IsName("Rifle Flinch"))
        {
            animator.SetBool("isHit", false);
        }

        else if (stateInfo.IsName("Pistol Reload"))
        {
            animator.SetBool("isReloading", false);
        }

        else if (stateInfo.IsName("Rifle Reload"))
        {
            animator.SetBool("isReloading", false);

            rifle.transform.localScale = new Vector3(0, 0, 0);
            rifle1.SetActive(true);
        }

        else if (stateInfo.IsName("Shotgun Reload"))
        {
            animator.SetBool("isReloading", false);

            shotgun.transform.localScale = new Vector3(0, 0, 0);
            shotgun1.SetActive(true);
        }

        else if (stateInfo.IsName("Magnum Reload"))
        {
            animator.SetBool("isReloading", false);
        }

        else if (stateInfo.IsName("Pistol Flinch") || stateInfo.IsName("Rifle Flinch") || stateInfo.IsName("Magnum Flinch"))
            animator.SetBool("isHit", false);

        else if (stateInfo.IsName("Grenade Throw"))
        {
            animator.SetBool("isThrowing", false);
        }

        else if (stateInfo.IsName("Knife Animation"))
        {
            animator.SetBool("isKnifing", false);
            animator.GetComponent<ThirdPersonController>().LeonCanBeAttacked = false;
            animator.GetComponent<ThirdPersonController>().LeonAttackCooldown = 0;
        }

        else if (stateInfo.IsName("Rifle Raise") || stateInfo.IsName("Rifle Sprint Raise"))
        {
            rifle.transform.localScale = new Vector3(0, 0, 0);
            rifle1.SetActive(true);
        }

        else if (stateInfo.IsName("Shotgun Raise") || stateInfo.IsName("Shotgun Sprint Raise"))
        {
            shotgun.transform.localScale = new Vector3(0, 0, 0);
            shotgun1.SetActive(true);
        }

        //animator.SetBool("isReloading", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
