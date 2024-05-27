using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    private const float StabRadius = 2f;
    [SerializeField] private GameObject Player;
    public GameObject KnifeCanvas;

    public void Stab(Enemy nearestEnemy)
    {
        if (nearestEnemy != null)
        {
            ThirdPersonController.instance._input.interact = false;

            Vector3 _direction = (Player.transform.position - transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);

            nearestEnemy.GetHit(6, GameLogic.instance.Blood, nearestEnemy.GetComponent<Enemy>().neck.position, _lookRotation);
            nearestEnemy.GetComponent<Animator>().SetBool("isDead", true);
            nearestEnemy.GetComponent<Animator>().SetInteger("random", 4);
            GetComponent<ThirdPersonController>().LeonCanBeAttacked = false;
            GetComponent<ThirdPersonController>().LeonAttackCooldown = 0;
            GetComponent<Animator>().SetBool("isKnifing", true);
            transform.rotation = Quaternion.LookRotation(nearestEnemy.transform.position - transform.position);
            nearestEnemy.transform.rotation = Quaternion.LookRotation(transform.position - nearestEnemy.transform.position);
        }
    }

    public Enemy FindNearestEnemy()
    {
        if (GetComponent<Animator>().GetBool("isKnifing"))
        {
            KnifeCanvas.SetActive(false);
            return null;
        }
        float minDistance = float.MaxValue;
        Enemy nearestEnemy = null;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, StabRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && enemy.EnemyAnimator.GetBool("isKnockedDown") && !enemy.isDead)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }
        if(nearestEnemy != null)
        {
            KnifeCanvas.SetActive(true);
        }
        else
        {
            KnifeCanvas.SetActive(false);
        }
        return nearestEnemy;
    }
}
