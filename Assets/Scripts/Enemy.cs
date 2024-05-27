using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float health;
    public bool isDead;
    public bool isIdle;
    private bool isHit;

    private bool canHit;
    public float delayBetweenAttacks;
    private float timeSinceAttack = 0;
    public bool isArmed;
    private Audio AudioScript;

    public float runningSpeed;

    public Animator EnemyAnimator;
    public Transform player;
    public NavMeshAgent agent;
    private AudioSource EnemyAudioSource;
    [SerializeField] private GameObject Audio;

    [SerializeField] private float enemySoundDelay;
    private float timeSinceLastSound;
    private float timeBetweenSounds = 10f;

    public bool isGrappling = false;

    private float grappleButtonBreakPress = 20;
    public float grappleButtonBreakPressCounter = 0;
    private float grappleTime = 4;
    private float timeSinceGrapple = 0;

    public Transform IdleTarget1;
    public Transform IdleTarget2;

    [SerializeField] private bool Male;

    private readonly float idleTime = 3;
    private float timeSinceIdle = 0;

    public bool isAlert = false;

    private float TimeBetweenSwitchingTargets = 1f;

    public Transform neck;

    void Start()
    {
        runningSpeed = 5.25f;
        AudioScript = Audio.GetComponent<Audio>();
        GetComponent<Animator>().runtimeAnimatorController = EnemyAnimator.runtimeAnimatorController;
        GetComponent<Animator>().avatar = EnemyAnimator.avatar;
        EnemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = 6;
        isDead = false;
        isIdle = false;
        canHit = true;
        EnemyAudioSource = GetComponent<AudioSource>();
        timeSinceLastSound = 0;

        if (isArmed)
            EnemyAnimator.SetBool("isArmed", true);
        else
            EnemyAnimator.SetBool("isArmed", false);

        agent.destination = IdleTarget1.position;
        EnemyAnimator.SetBool("isIdle", false);
        EnemyAnimator.SetBool("isAlert", false);
        EnemyAnimator.SetInteger("random", Random.Range(0, 2));
        EnemyAnimator.SetFloat("speed", 1.5f);
        isIdle = false;
    }
    void Update()
    {

        if (!isDead && agent.remainingDistance <= agent.stoppingDistance && TimeBetweenSwitchingTargets <= 0 && !isAlert)
        {
            if (timeSinceIdle < idleTime)
            {
                if (!EnemyAnimator.GetBool("isIdle"))
                    EnemyAnimator.SetInteger("random", Random.Range(0, 2));
                timeSinceIdle += Time.deltaTime;
                GetComponent<NavMeshAgent>().speed = 0;
                EnemyAnimator.SetBool("isIdle", true);
                EnemyAnimator.SetFloat("speed", 0f);
            }
            else
            {
                EnemyAnimator.SetInteger("random", Random.Range(0, 2));
                EnemyAnimator.SetBool("isIdle", false);
                EnemyAnimator.SetFloat("speed", 1.5f);
                GetComponent<NavMeshAgent>().speed = 1.5f;
                timeSinceIdle = 0;
                if (agent.destination.x.Equals(IdleTarget1.position.x) && agent.destination.z.Equals(IdleTarget1.position.z))
                {
                    agent.destination = IdleTarget2.position;
                }
                else if (agent.destination.x.Equals(IdleTarget2.position.x) && agent.destination.z.Equals(IdleTarget2.position.z))
                {
                    agent.destination = IdleTarget1.position;
                }
                TimeBetweenSwitchingTargets = 1f;
            }
        }
        else if (TimeBetweenSwitchingTargets > 0)
        {
            TimeBetweenSwitchingTargets -= Time.deltaTime;
        }

        if (!isDead && agent.remainingDistance > agent.stoppingDistance && !isAlert)
        {
            isIdle = false;
            EnemyAnimator.SetBool("isIdle", false);
            EnemyAnimator.SetFloat("speed", 1.5f);
            GetComponent<NavMeshAgent>().speed = 1.5f;
        }

        EnemySound();

        isHit = EnemyAnimator.GetBool("isHit");

        if (isGrappling)
        {
            EnemyAnimator.SetBool("isGrappling", true);
        }

        else
        {
            EnemyAnimator.SetBool("isGrappling", false);
        }

        //if (!isHit && isAlert && !isGrappling)
        //{
        //    GetComponent<NavMeshAgent>().speed = 4.5f;
        //}
        if (isGrappling)
            Grappling();

        if (timeSinceAttack < delayBetweenAttacks && !canHit && !isDead)
            timeSinceAttack += Time.deltaTime;

        else if (timeSinceAttack >= delayBetweenAttacks && !canHit && !isDead)
        {
            canHit = true;
            EnemyAnimator.SetInteger("random", Random.Range(0, 3));
        }

        if (!isDead && !EnemyAnimator.GetBool("isKnockedDown") && !isGrappling && !ThirdPersonController.instance.isBeingGrappled && !isHit && isAlert)
        {

            if (EnemyAnimator.GetBool("isHit"))
            {
                EnemyAnimator.SetFloat("speed", 0f);
                agent.speed = 0f;
            }
            else if (!EnemyAnimator.GetBool("isKnockedDown") && !isGrappling)
            {

                EnemyAnimator.SetFloat("speed", runningSpeed);
                agent.speed = runningSpeed;
            }

            agent.destination = player.position;
            if (CanHit())
            {
                var targetRotation = Quaternion.LookRotation(player.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1000 * Time.deltaTime);

                EnemyAnimator.SetInteger("random", Random.Range(0, 2));

                EnemyAnimator.SetBool("isHitting", true);

                timeSinceAttack = 0;
                canHit = false;

                if (EnemyAnimator.GetBool("isReaching"))
                {
                    GrappleLeon();
                }
            }

            else if (CanThrow())
            {
                if (Random.Range(0, 999) == 100)
                {
                    EnemyAnimator.SetBool("isThrowing", true);
                }
            }

            else if (CanGrab())
            {
                if (Random.Range(0, 749) == 100)
                {
                    EnemyAnimator.SetBool("isReaching", true);
                }
            }
        }
    }

    public Boolean CanGrab()
    {
        if (GetComponent<NavMeshAgent>().enabled && canHit && !isDead && !EnemyAnimator.GetBool("isKnockedDown") && !isGrappling && !ThirdPersonController.instance.isBeingGrappled && agent.remainingDistance <= agent.stoppingDistance + 7 && (Vector3.Distance(transform.position, player.position) < 6) && ThirdPersonController.instance.LeonCanBeAttacked && !isArmed)
            return true;
        else
            return false;
    }

    public Boolean CanThrow()
    {
        if (GetComponent<NavMeshAgent>().enabled && canHit && !isDead && !EnemyAnimator.GetBool("isKnockedDown") && !EnemyAnimator.GetBool("isHit") && !isGrappling && !ThirdPersonController.instance.isBeingGrappled && agent.remainingDistance <= agent.stoppingDistance + 7 && (Vector3.Distance(transform.position, player.position) > 6) && ThirdPersonController.instance.LeonCanBeAttacked && isArmed)
            return true;
        else
            return false;
    }

    public Boolean CanHit()
    {
        if (GetComponent<NavMeshAgent>().enabled && canHit && !isDead && !EnemyAnimator.GetBool("isKnockedDown") && !EnemyAnimator.GetBool("isHit") && !isGrappling && !ThirdPersonController.instance.isBeingGrappled && agent.remainingDistance <= agent.stoppingDistance && (Vector3.Distance(transform.position, player.position) < 3) && ThirdPersonController.instance.LeonCanBeAttacked)
            return true;
        else
            return false;
    }

    public void KnockDown()
    {
        EnemyAnimator.SetBool("isKnockedDown", true);
    }

    public void CancelKnockDown()
    {
        EnemyAnimator.SetBool("isKnockedDown", false);
    }

    public void DropCoins()
    {
        GameObject gold = Instantiate(GameLogic.instance.Gold, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), Quaternion.identity);
        GameLogic.instance.GoldCoins.Add(gold);
        if (GetComponent<MeshCollider>() != null)
            Destroy(GetComponent<MeshCollider>());
        if (GetComponent<CapsuleCollider>() != null)
            Destroy(GetComponent<CapsuleCollider>());
    }

    public void GetHit(float damage, GameObject Blood, Vector3 hitPoint, Quaternion _lookRotation)
    {
        if (isAlert)
        {
            if (Blood != null)
                Instantiate(Blood, hitPoint, _lookRotation, transform);

            health -= damage;
            health = Mathf.Max(health, 0);
            GetComponent<NavMeshAgent>().speed = 0;

            if (health == 0)
            {
                if (!isDead)
                {
                    EnemyAnimator.SetInteger("random", Random.Range(0, 6));
                    EnemyAnimator.SetBool("isDead", true);
                    GetComponent<NavMeshAgent>().enabled = false;
                    if (Male)
                        AudioScript.PlayMaleEnemyDeathSound(EnemyAudioSource);
                    else
                        AudioScript.PlayFemaleEnemyDeathSound(EnemyAudioSource);
                }
                isDead = true;
            }

            else
            {
                EnemyAnimator.SetBool("isHitting", false);
                EnemyAnimator.SetBool("isGrappling", false);
                isGrappling = false;
                player.GetComponent<Animator>().SetBool("isGrappled", false);
                ThirdPersonController.instance.isBeingGrappled = false;
                EnemyAnimator.SetBool("isHit", true);
                EnemyAnimator.SetInteger("random", Random.Range(0, 2));

                if (Male)
                    AudioScript.PlayMaleEnemyHitSound(EnemyAudioSource);
                else
                    AudioScript.PlayFemaleEnemyHitSound(EnemyAudioSource);
            }
        }
    }
    public void GetFlashed()
    {
        isGrappling = false;
        EnemyAnimator.SetBool("isHitting", false);
        EnemyAnimator.SetBool("isKnockedDown", true);
        EnemyAnimator.SetBool("isGrappling", false);
        player.GetComponent<Animator>().SetBool("isGrappled", false);
        ThirdPersonController.instance.isBeingGrappled = false;
        GameLogic.instance.StrugglingCanvas.SetActive(false);
    }

    public void ReachLeon() { }

    public void AttackDistanceCheck()
    {
        float distanceFromPlayer = Vector3.Distance(agent.gameObject.transform.position, player.position);

        if (Math.Abs(agent.destination.x - player.position.x) < 0.5f && Math.Abs(agent.destination.z - player.position.z) < 0.5f && distanceFromPlayer < 2.3f)
        {
            if (isArmed)
                ThirdPersonController.instance.GetHit(2);
            else
                ThirdPersonController.instance.GetHit(1);
        }
    }

    public void GrappleExecute()
    {

        player.GetComponent<Animator>().SetBool("grappleExecuted", true);
        ThirdPersonController.instance.GetHit(5);
    }

    private void EnemySound()
    {
        if (!isDead && !ThirdPersonController.instance.isDead)
        {
            if (timeSinceLastSound - enemySoundDelay < timeBetweenSounds)
            {
                timeSinceLastSound += Time.deltaTime;
            }

            else
            {
                if (Male && isAlert)
                    AudioScript.PlayMaleAlertedSound(EnemyAudioSource);

                else if (Male)
                    AudioScript.PlayMaleEnemyIdleSound(EnemyAudioSource);

                else
                    AudioScript.PlayFemaleAlertedSound(EnemyAudioSource);

                timeSinceLastSound = enemySoundDelay;
            }
        }
    }

    public void GrappleLeon()
    {
        agent.speed = 0;

        ThirdPersonController.instance.Enemy = gameObject;
        ThirdPersonController.instance.isBeingGrappled = true;
        ThirdPersonController.instance.LeonCanBeAttacked = false;
        EnemyAnimator.SetBool("isHitting", false);
        EnemyAnimator.SetBool("isGrappling", true);
        GameLogic.instance.StrugglingCanvas.SetActive(true);
        GameLogic.instance.GrapplingEnemy = gameObject;
        isGrappling = true;
    }

    public void Grappling()
    {
        if (timeSinceGrapple <= grappleTime)
            timeSinceGrapple += Time.deltaTime;

        if (grappleButtonBreakPressCounter >= grappleButtonBreakPress)
        {
            player.GetComponent<Animator>().SetBool("defendGrapple", true);
            timeSinceGrapple = 0;
            agent.speed = 0;
            grappleButtonBreakPressCounter = 0;
            GameLogic.instance.StrugglingCanvas.SetActive(false);
            GameLogic.instance.progressBar.GetComponent<RectTransform>().offsetMax = new Vector2(-435, 0);
            GameLogic.instance.GrapplingEnemy = null;
        }

        else if (timeSinceGrapple > grappleTime)
        {
            EnemyAnimator.SetBool("isExecuting", true);
            timeSinceGrapple = 0;
            isGrappling = false;
            grappleButtonBreakPressCounter = 0;
            GameLogic.instance.StrugglingCanvas.SetActive(false);
            GameLogic.instance.progressBar.GetComponent<RectTransform>().offsetMax = new Vector2(-435, 0);
            GameLogic.instance.GrapplingEnemy = null;
        }
    }
}
