using UnityEngine;

public class EnemyAnimationHandler : StateMachineBehaviour
{   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Standing Melee Attack Downward") || stateInfo.IsName("Standing Melee Attack Horizontal") || stateInfo.IsName("Zombie Punching") || stateInfo.IsName("Zombie Attack") || stateInfo.IsName("Knocked Down") || stateInfo.IsName("Upward Thurst") || stateInfo.IsName("Melee Attack Backhand") || stateInfo.IsName("Melee Attack 360") || stateInfo.IsName("Melee Attack Kick") || stateInfo.IsName("Melee Combo"))
        {
            if (animator.GetComponent<Enemy>() != null)
                animator.GetComponent<Enemy>().agent.speed = 0f;
            else
                animator.GetComponent<Jack>().agent.speed = 0f;
        }

        else if(stateInfo.IsName("Reach To Grab"))
        {
            animator.GetComponent<Enemy>().agent.speed = 3f;
            if (animator.GetComponent<Enemy>().CanHit())
            {
                animator.GetComponent<Enemy>().GrappleLeon();
            }   
        }
        else if (stateInfo.IsName("Idle") || stateInfo.IsName("Unarmed Idle Looking Ver_ 1"))
        {
            animator.SetBool("isThrowing", false);
        }
        else if (stateInfo.IsName("Zombie Reaction Hit") || stateInfo.IsName("Zombie Reaction Hit 1"))
        {
            if (animator.GetComponent<Enemy>() != null)
                animator.GetComponent<Enemy>().agent.speed = 0f;
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.IsName("Standing Melee Attack Downward") || stateInfo.IsName("Standing Melee Attack Horizontal") || stateInfo.IsName("Zombie Punching") || stateInfo.IsName("Zombie Attack") || stateInfo.IsName("Reach To Grab") || stateInfo.IsName("Upward Thurst") || stateInfo.IsName("Melee Attack Backhand") || stateInfo.IsName("Melee Attack 360") || stateInfo.IsName("Melee Attack Kick") || stateInfo.IsName("Melee Combo")) { 
            animator.SetBool("isHitting", false);
            animator.SetBool("isReaching", false);
            if(animator.GetComponent<Enemy>() != null)
                animator.GetComponent<Enemy>().agent.speed = animator.GetComponent<Enemy>().runningSpeed;
            else
                animator.GetComponent<Jack>().agent.speed = animator.GetComponent<Jack>().runningSpeed;
        }

        else if (stateInfo.IsName("Zombie Reaction Hit") || stateInfo.IsName("Zombie Reaction Hit 1"))
        {
            animator.SetBool("isHit", false);
            animator.SetInteger("random", Random.Range(0, 3));
        }

        else if(stateInfo.IsName("Standing Disarm Over Shoulder"))
        {
            animator.SetBool("isEquipping", false);
            animator.GetComponent<Enemy>().agent.speed = 1.5f;
        }
        
        else if (stateInfo.IsName("Unarmed Equip Underarm") || stateInfo.IsName("Unarmed Equip Over Shoulder"))
        {
            animator.SetBool("isEquipping", false);
        }

        else if (stateInfo.IsName("Execute")) {
            animator.SetBool("isExecuting", false);
            animator.SetBool("isGrappling", false);
        }

        else if(stateInfo.IsName("Defended Against"))
        {
            animator.SetBool("isDefended", false);
            animator.SetBool("isGrappling", false);
        }

        else if (stateInfo.IsName("Throwing"))
        {
            animator.SetBool("isThrowing", false);
            animator.SetBool("isArmed", false);
            animator.GetComponent<Enemy>().isArmed = false;
        }

        else if(stateInfo.IsName("Knocked Down"))
        {
            animator.SetBool("isKnockedDown", false);
        }

        else if (stateInfo.IsName("Battlecry"))
        {
            animator.SetBool("isEquipping", false);
            animator.GetComponent<Jack>().isAlert = true;
        }
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
