using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Jack : MonoBehaviour
{
    public float health;
    public bool isDead;
    public bool isIdle;
    private bool isHit;
    private bool canHit;
    public bool isAlert;
    [SerializeField] private GameObject oldKey;

    public float delayBetweenAttacks;
    private float timeSinceAttack = 0;

    public float runningSpeed;

    public Animator EnemyAnimator;
    public Transform player;
    public NavMeshAgent agent;
    private AudioSource JackAudioSource;

    private float timeSinceLastSound;
    private float timeBetweenSounds = 7f;

    [SerializeField] private List<AudioClip> chasingClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> attackSuccessClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> damagedClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> killClips = new List<AudioClip>();
    [SerializeField] private AudioClip deathClip;

    private int chasingClipIndex = 0;
    private int attackSuccessClipIndex = 0;
    private int damagedClipIndex = 0;

    void Start()
    {
        GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator.runtimeAnimatorController;
        GetComponent<Animator>().avatar = EnemyAnimator.avatar;
        EnemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0;
        isDead = false;
        isIdle = true;
        canHit = true;
        JackAudioSource = GetComponent<AudioSource>();
        timeSinceLastSound = 0;

        EnemyAnimator.SetBool("isAlert", false);
        EnemyAnimator.SetFloat("speed", 0);
    }
    void Update()
    {
        if (!isIdle)
        {
            EnemyAnimator.SetBool("isEquipping", true);
            isIdle = true;
        }

        if (isAlert)
            EnemyAnimator.SetBool("isAlert", true);
        else
            EnemyAnimator.SetBool("isAlert", false);

        isHit = EnemyAnimator.GetBool("isHit");

        if (timeSinceAttack < delayBetweenAttacks && !canHit)
            timeSinceAttack += Time.deltaTime;

        else if (timeSinceAttack >= delayBetweenAttacks && !canHit)
        {
            canHit = true;
            EnemyAnimator.SetInteger("random", Random.Range(0, 3));
        }

        if (!isDead && !ThirdPersonController.instance.isBeingGrappled && !isHit)
        {

            if (EnemyAnimator.GetBool("isHit"))
            {
                EnemyAnimator.SetFloat("speed", 0f);
                agent.speed = 0f;
            }
            else if (isAlert)
            {
                EnemyAnimator.SetFloat("speed", runningSpeed);
                agent.speed = runningSpeed;
            }

            if (isAlert)
            {
                agent.destination = player.position;
            }

            if (CanHit())
            {
                var targetRotation = Quaternion.LookRotation(player.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1000 * Time.deltaTime);

                EnemyAnimator.SetInteger("random", Random.Range(0, 7));

                EnemyAnimator.SetBool("isHitting", true);

                timeSinceAttack = 0;
                canHit = false;
            }
        }
        if (isAlert && !isDead)
            JackIsChasing();
    }

    public void JackIsChasing()
    {
        if (timeSinceLastSound < timeBetweenSounds)
        {
            timeSinceLastSound += Time.deltaTime;
        }

        else
        {
            timeSinceLastSound = 0;
            if (!JackAudioSource.isPlaying)
            {
                JackAudioSource.clip = chasingClips[chasingClipIndex];
                JackAudioSource.Play();
                if (chasingClipIndex + 1 >= chasingClips.Count)
                {
                    chasingClipIndex = 0;
                }
                else
                {
                    chasingClipIndex++;
                }
            }
        }
    }

    public Boolean CanHit()
    {
        if (GetComponent<NavMeshAgent>().enabled && canHit && !isDead && !ThirdPersonController.instance.isBeingGrappled && agent.remainingDistance <= agent.stoppingDistance && (Vector3.Distance(transform.position, player.position) < 3) && ThirdPersonController.instance.LeonCanBeAttacked)
            return true;
        else
            return false;
    }

    public void GetHit(float damage, GameObject Blood, Vector3 hitPoint, Quaternion _lookRotation)
    {
        if (Blood != null)
            Instantiate(Blood, hitPoint, _lookRotation, transform);

        float oldHealth = health;
        health -= damage;
        health = Mathf.Max(health, 0);
        GetComponent<NavMeshAgent>().speed = 0;

        if (health == 0)
        {
            if (!isDead)
            {
                JackAudioSource.Stop();
                JackAudioSource.clip = deathClip;
                JackAudioSource.Play();
                GameObject oldKeyInstance = Instantiate(oldKey, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), Quaternion.identity);
                GameLogic.instance.OldKey = oldKeyInstance;
                GameLogic.instance.AudioScript.JackDefeated();
                GetComponent<NavMeshAgent>().enabled = false;
                EnemyAnimator.SetInteger("random", Random.Range(0, 6));
                EnemyAnimator.SetBool("isDead", true);
            }
            isDead = true;
        }

        else if (health % 6 > oldHealth % 6 || damage >= 6)
        {
            JackAudioSource.Stop();
            JackAudioSource.clip = damagedClips[damagedClipIndex];
            JackAudioSource.Play();
            if (damagedClipIndex + 1 >= damagedClips.Count)
            {
                damagedClipIndex = 0;
            }
            else
            {
                damagedClipIndex++;
            }
        }

        if (health % 12 > oldHealth % 12 || damage >= 12)
        {
            EnemyAnimator.SetBool("isHitting", false);
            EnemyAnimator.SetBool("isHit", true);
            EnemyAnimator.SetInteger("random", Random.Range(0, 2));
        }
    }

    public void AttackDistanceCheck()
    {
        float distanceFromPlayer = Vector3.Distance(agent.gameObject.transform.position, player.position);

        if (Math.Abs(agent.destination.x - player.position.x) < 0.5f && Math.Abs(agent.destination.z - player.position.z) < 0.5f && distanceFromPlayer < 2.7f)
        {
            ThirdPersonController.instance.GetHit(3);
            if (ThirdPersonController.instance.health <= 0 && isAlert)
            {
                JackAudioSource.Stop();
                int random = Random.Range(0, killClips.Count);
                JackAudioSource.clip = killClips[random];
                JackAudioSource.Play();
                isAlert = false;
            }
            else if (ThirdPersonController.instance.health > 0)
            {
                JackAudioSource.Stop();
                JackAudioSource.clip = attackSuccessClips[attackSuccessClipIndex];
                JackAudioSource.Play();
                if (attackSuccessClipIndex + 1 >= attackSuccessClips.Count)
                {
                    attackSuccessClipIndex = 0;
                }
                else
                {
                    attackSuccessClipIndex++;
                }
            }
        }
    }
}
