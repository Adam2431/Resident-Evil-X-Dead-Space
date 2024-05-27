using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
using UnityEngine.UI;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player

        public float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs

        private int _animIDAiming;
        private int _animIDSpeed;
        private int _animIDPistol;
        private int _animIDRifle;
        private int _animIDShotgun;
        private int _animIDMagnum;
        private int _animIDGrenade;
        private int _animIDDead;
        private int _animIDHit;
        private int _animIDReload;
        private int _animIDShooting;
        private int _animIDRaising;
        private int _animIDPuttingAway;

        public bool isDead;

        private int _animIDDirectionX;
        private int _animIDDirectionY;

        private float footstepDelay = 0.5f;

        private bool walkingTransition;
        private bool runningTransition;

        [SerializeField] private GameObject Spine;

#if ENABLE_INPUT_SYSTEM 
        public PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        public StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        [SerializeField] private RawImage crosshairs;

        public GameObject pistol;
        public GameObject rifle;
        public GameObject rifle1;
        public GameObject shotgun;
        public GameObject shotgun1;
        public GameObject magnum;
        public GameObject handGrenade;
        public GameObject flashGrenade;

        [SerializeField] private CinemachineVirtualCamera VirtualCamera;
        [SerializeField] private CinemachineVirtualCamera VirtualCameraAim;

        [SerializeField] private GameObject Audio;
        private Audio AudioScript;
        public bool isBeingGrappled;
        public GameObject Enemy;
        public int health = 8;
        public float LeonAttackCooldownReset = 2;
        public float LeonAttackCooldown = 0;
        public bool LeonCanBeAttacked = true;

        private Transform objectHit;

        public Image HealthUI;
        public Text AmmoUI;
        [SerializeField] private GameObject PistolUI;
        [SerializeField] private GameObject RifleUI;
        [SerializeField] private GameObject ShotgunUI;
        [SerializeField] private GameObject MagnumUI;
        [SerializeField] private GameObject HandGrenadeUI;
        [SerializeField] private GameObject FlashGrenadeUI;

        public Color fullHealthColor = new(0, 0.86f, 0);
        public Color deadHealthColor = new(0.86f, 0, 0);

        public static ThirdPersonController instance;
        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            AudioScript = Audio.GetComponent<Audio>();
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            instance = this;
        }

        private void Update()
        {
            if (instance == null)
                instance = this;

            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            KeySprite();

            if (_input.sprint)
            {
                _animator.SetBool(_animIDReload, false);
            }

            if (!isBeingGrappled && !_animator.GetBool("isKnifing") && !isDead)
            {
                ShootWeapon();
                ReloadWeapon();
                Move();
                if ((_input.switchNextWeapon || _input.switchPreviousWeapon) && GameLogic.instance.currentWeapons.Count > 1)
                {
                    SwitchWeapon();
                }

                if (!LeonCanBeAttacked && LeonAttackCooldown <= LeonAttackCooldownReset)
                {
                    LeonAttackCooldown += Time.deltaTime;
                }
                else
                {
                    LeonCanBeAttacked = true;
                    LeonAttackCooldown = 0;
                }
            }
            else if (isBeingGrappled)
            {
                _animator.SetBool("isGrappled", true);
                Grappled();
            }

            if (_speed < 2)
            {
                footstepDelay = 0.62f;
                walkingTransition = true;
            }
            else if (_speed < 5)
            {
                runningTransition = true;
                if (walkingTransition)
                {
                    footstepDelay = 0f;
                    walkingTransition = false;
                }
                footstepDelay -= Time.deltaTime;
                if (footstepDelay <= 0)
                {
                    AudioScript.PlayFootstepSound();
                    footstepDelay = 0.62f;
                }
            }
            else
            {
                if (runningTransition)
                {
                    footstepDelay = 0f;
                    runningTransition = false;
                }
                footstepDelay -= Time.deltaTime;
                if (footstepDelay <= 0)
                {
                    AudioScript.PlayFootstepSound();
                    footstepDelay = 0.3f;
                }
            }
            //GroundedCheck();
            //JumpAndGravity();
        }

        private void KeySprite()
        {
            if (_playerInput.currentControlScheme == "KeyboardMouse")
            {
                GameLogic.instance.GoldCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.GoldCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.DoorCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.DoorCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.MerchantCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.MerchantCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.MagnumCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.MagnumCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.StrugglingCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.StrugglingCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.KnifeCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.KnifeCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.TreasureCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.TreasureCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);

                GameLogic.instance.ItemPickupPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                GameLogic.instance.ItemPickupPanel.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                GameLogic.instance.GoldCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.GoldCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.DoorCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.DoorCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.MerchantCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.MerchantCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.MagnumCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.MagnumCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.KeyCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.StrugglingCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.StrugglingCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.KnifeCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.KnifeCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.TreasureCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.TreasureCanvas.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);

                GameLogic.instance.ItemPickupPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameLogic.instance.ItemPickupPanel.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Axe"))
            {
                if (LeonCanBeAttacked)
                {
                    GetHit(3);
                }
            }
        }

        public void GrappleDefended()
        {
            Enemy.GetComponent<Animator>().SetBool("isDefended", true);
            Enemy.GetComponent<Enemy>().isGrappling = false;
            Enemy.GetComponent<Enemy>().grappleButtonBreakPressCounter = 0;
            GameLogic.instance.progressBar.GetComponent<RectTransform>().offsetMax = new Vector2(-435, 0);
        }

        public void PistolEquip()
        {
            AudioScript.PlayPistolEquip();
        }

        public void ShotgunEquip()
        {
            AudioScript.PlayShotgunEquip();
        }

        public void MagnumEquip()
        {
            AudioScript.PlayMagnumEquip();
        }

        public void PistolReload()
        {
            int refillAmmount = Mathf.Min(pistol.GetComponent<Weapon>().maxCapacity - pistol.GetComponent<Weapon>().capacity, GameLogic.instance.pistolAmmo);
            //RightAmmo.text = pistol.GetComponent<Weapon>().Reload(GameLogic.instance.pistolAmmo).ToString();
            AmmoUI.text = pistol.GetComponent<Weapon>().Reload(GameLogic.instance.pistolAmmo).ToString() + "/" + GameLogic.instance.pistolAmmo;
            GameLogic.instance.pistolAmmo -= refillAmmount;
            AudioScript.PlayPistolSlideForward();
            GameLogic.instance.GetComponent<GameLogic>().UpdateAmmo();
        }

        public void RifleReload()
        {
            int refillAmmount = Mathf.Min(rifle.GetComponent<Weapon>().maxCapacity - rifle.GetComponent<Weapon>().capacity, GameLogic.instance.rifleAmmo);
            //RightAmmo.text = rifle.GetComponent<Weapon>().Reload(GameLogic.instance.rifleAmmo).ToString();
            AmmoUI.text = rifle.GetComponent<Weapon>().Reload(GameLogic.instance.rifleAmmo).ToString() + "/" + GameLogic.instance.rifleAmmo;
            GameLogic.instance.rifleAmmo -= refillAmmount;
            AudioScript.PlayRifleFinishReload();
            GameLogic.instance.GetComponent<GameLogic>().UpdateAmmo();
        }

        public void ShotgunReload()
        {
            int refillAmmount = 1;
            //RightAmmo.text = shotgun.GetComponent<Weapon>().Reload(GameLogic.instance.shotgunAmmo).ToString();
            AmmoUI.text = shotgun.GetComponent<Weapon>().Reload(GameLogic.instance.shotgunAmmo).ToString() + "/" + GameLogic.instance.shotgunAmmo;
            GameLogic.instance.shotgunAmmo -= refillAmmount;
            AudioScript.PlayShotgunShellIn();
            if (!shotgun.GetComponent<Weapon>().CanReload())
            {
                _animator.SetBool(_animIDReload, false);
            }
            GameLogic.instance.GetComponent<GameLogic>().UpdateAmmo();
        }

        public void MagnumReload()
        {
            int refillAmmount = Mathf.Min(magnum.GetComponent<Weapon>().maxCapacity - magnum.GetComponent<Weapon>().capacity, GameLogic.instance.magnumAmmo);
            //RightAmmo.text = magnum.GetComponent<Weapon>().Reload(GameLogic.instance.magnumAmmo).ToString();
            AmmoUI.text = magnum.GetComponent<Weapon>().Reload(GameLogic.instance.magnumAmmo).ToString() + "/" + GameLogic.instance.magnumAmmo;
            GameLogic.instance.magnumAmmo -= refillAmmount;
            GameLogic.instance.GetComponent<GameLogic>().UpdateAmmo();
        }
        private void ReloadWeapon()
        {
            if (_input.reload && !_input.sprint && !_animator.GetBool(_animIDRaising) && !_animator.GetBool(_animIDPuttingAway))
            {
                if (pistol.activeSelf)
                {
                    if (!pistol.GetComponent<Weapon>().CanReload())
                        return;
                    _animator.SetBool(_animIDReload, true);
                }
                else if (rifle.activeSelf)
                {
                    if (!rifle.GetComponent<Weapon>().CanReload())
                        return;
                    _animator.SetBool(_animIDReload, true);
                }
                else if (shotgun.activeSelf)
                {
                    if (!shotgun.GetComponent<Weapon>().CanReload())
                        return;
                    _animator.SetBool(_animIDReload, true);
                }
                else if (magnum.activeSelf)
                {
                    if (!magnum.GetComponent<Weapon>().CanReload())
                        return;
                    _animator.SetBool(_animIDReload, true);
                }
            }
            else if (_input.sprint)
            {
                _animator.SetBool(_animIDReload, false);
            }
        }

        public void PlayPistolEquip()
        {
            AudioScript.PlayPistolEquip();
        }

        public void PlayPistolMagOut()
        {
            AudioScript.PlayPistolMagOut();
        }

        public void PlayPistolMagIn()
        {
            AudioScript.PlayPistolMagIn();
        }

        public void PlayPistolSlideBack()
        {
            AudioScript.PlayPistolSlideBack();
        }

        public void PlayRifleMagOut()
        {
            AudioScript.PlayRifleMagOut();
        }

        public void PlayRifleMagIn()
        {
            AudioScript.PlayRifleMagIn();
        }

        public void PlayRifleSlideBack()
        {
            AudioScript.PlayRifleSlideBack();
        }

        public void PlayRifleSlideForward()
        {
            AudioScript.PlayRifleSlideForward();
        }

        public void PlayMagnumReload()
        {
            AudioScript.PlayMagnumReload();
        }

        private void Grappled()
        {
            var targetRotation = Quaternion.LookRotation(Enemy.transform.position - transform.position);
            transform.rotation = targetRotation;

        }

        public void GetHit(int damage)
        {
            StartCoroutine(GameLogic.instance.PostProcessingVolume.GetComponent<TakeDamageEffect>().TakeDamage());
            health -= damage;
            HealthUI.fillAmount = (float)health / 8;
            HealthUI.color = Color.Lerp(deadHealthColor, fullHealthColor, (float)health / 8);
            if (health <= 0)
            {
                health = 0;
                _animator.SetBool(_animIDDead, true);
                isDead = true;
            }
            else
            {
                _animator.SetBool(_animIDHit, true);
            }
        }

        private void ShootWeapon()
        {
            if (!(GameLogic.instance.currentWeapons[_input.weaponIndex].Equals("Hand Grenade") || GameLogic.instance.currentWeapons[_input.weaponIndex].Equals("Flash Grenade")) && _input.shooting && _input.aim && !_animator.GetBool(_animIDRaising) && !_animator.GetBool(_animIDPuttingAway) && !_animator.GetBool(_animIDReload) && !_input.sprint && !GameLogic.instance.Store.activeSelf && !GameLogic.instance.PauseMenu.activeSelf && !GameLogic.instance.InventoryUI.activeSelf)
            {
                objectHit = null;

                Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                Vector3 hitPoint = new Vector3(0, 0, 0);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    objectHit = hit.transform;
                    hitPoint = hit.point;
                }

                Vector3 _direction = (transform.position - objectHit.position).normalized;

                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                if (pistol.activeSelf)
                {
                    if (pistol.GetComponent<Weapon>().Shoot())
                    {
                        AudioScript.PlayPistolShot();
                        _animator.SetBool(_animIDShooting, true);
                        AmmoUI.text = pistol.GetComponent<Weapon>().capacity.ToString() + "/" + GameLogic.instance.pistolAmmo;
                        if (objectHit != null)
                        {
                            if (objectHit.CompareTag("Enemy"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < pistol.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(pistol.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(pistol.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                            }
                            else if (objectHit.CompareTag("Jack"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < pistol.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Jack>().GetHit(pistol.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else
                                {
                                    objectHit.GetComponent<Jack>().GetHit(pistol.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);

                                }
                            }
                        }
                    }
                    else if (pistol.GetComponent<Weapon>().capacity == 0)
                    {
                        AudioScript.PlayPistolEmpty();
                    }
                    _input.shooting = false;
                }
                else if (rifle.activeSelf)
                {
                    if (rifle.GetComponent<Weapon>().Shoot())
                    {
                        AudioScript.PlayRifleShot();
                        rifle1.GetComponent<Weapon>().PlayMuzzleFlash();
                        _animator.SetBool(_animIDShooting, true);
                        string ammo = rifle.GetComponent<Weapon>().capacity.ToString();
                        AmmoUI.text = ammo + "/" + GameLogic.instance.rifleAmmo;
                        if (objectHit != null)
                        {
                            if (objectHit.CompareTag("Enemy"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < rifle.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(rifle.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(rifle.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                            }
                            else if (objectHit.CompareTag("Jack"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < rifle.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Jack>().GetHit(rifle.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else
                                {
                                    objectHit.GetComponent<Jack>().GetHit(rifle.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                            }
                        }
                    }
                    else if (rifle.GetComponent<Weapon>().capacity == 0 && _input.rifleShooting)
                    {
                        AudioScript.PlayRifleEmpty();
                    }
                    _input.rifleShooting = false;
                }
                else if (shotgun.activeSelf)
                {
                    if (shotgun.GetComponent<Weapon>().Shoot())
                    {
                        AudioScript.PlayShotgunShot();
                        shotgun1.GetComponent<Weapon>().PlayMuzzleFlash();
                        _animator.SetBool(_animIDShooting, true);
                        string ammo = shotgun.GetComponent<Weapon>().capacity.ToString();
                        AmmoUI.text = ammo + "/" + GameLogic.instance.shotgunAmmo;
                        if (objectHit != null)
                        {
                            if (objectHit.CompareTag("Enemy"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < shotgun.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(shotgun.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else if (Vector3.Distance(objectHit.position, transform.position) < shotgun.GetComponent<Weapon>().range * 2)
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(shotgun.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                            }
                            else if (objectHit.CompareTag("Jack"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < shotgun.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Jack>().GetHit(shotgun.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else if (Vector3.Distance(objectHit.position, transform.position) < shotgun.GetComponent<Weapon>().range * 2)
                                {
                                    objectHit.GetComponent<Jack>().GetHit(shotgun.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                            }

                        }
                    }
                    else if (shotgun.GetComponent<Weapon>().capacity == 0)
                    {
                        AudioScript.PlayShotgunEmpty();
                    }
                    _input.shooting = false;
                }
                else if (magnum.activeSelf)
                {
                    if (magnum.GetComponent<Weapon>().Shoot())
                    {
                        AudioScript.PlayMagnumShot();
                        AmmoUI.text = magnum.GetComponent<Weapon>().capacity.ToString() + "/" + GameLogic.instance.magnumAmmo;
                        if (objectHit != null)
                        {
                            if (objectHit.CompareTag("Enemy"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < magnum.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(magnum.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else
                                {
                                    objectHit.GetComponent<Enemy>().GetHit(magnum.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                            }
                            else if (objectHit.CompareTag("Jack"))
                            {
                                if (Vector3.Distance(objectHit.position, transform.position) < magnum.GetComponent<Weapon>().range)
                                {
                                    objectHit.GetComponent<Jack>().GetHit(magnum.GetComponent<Weapon>().damage, GameLogic.instance.Blood, hitPoint, _lookRotation);
                                }
                                else
                                {
                                    objectHit.GetComponent<Jack>().GetHit(magnum.GetComponent<Weapon>().damage / 2, GameLogic.instance.Blood, hitPoint, _lookRotation);

                                }
                            }
                        }
                    }
                    else if (magnum.GetComponent<Weapon>().capacity == 0)
                    {
                        AudioScript.PlayMagnumEmpty();
                    }
                    _input.shooting = false;
                }
            }
            else
            {
                _animator.SetBool(_animIDShooting, false);
            }
        }

        public void FinishRaising()
        {
            _animator.SetBool(_animIDRaising, false);
        }

        public void FinishPuttingAway()
        {
            _animator.SetBool(_animIDPuttingAway, false);
            _animator.SetBool(_animIDRaising, true);


            int currentWeaponsNumber = GameLogic.instance.currentWeapons.Count;
            List<string> currentWeapons = GameLogic.instance.currentWeapons;

            for (int i = 0; i < currentWeaponsNumber; i++)
            {
                if (_input.weaponIndex == i)
                {
                    if (currentWeapons[i].Equals("Pistol"))
                    {
                        _animator.SetBool(_animIDPistol, true);
                        _animator.SetBool(_animIDRifle, false);
                        _animator.SetBool(_animIDShotgun, false);
                        _animator.SetBool(_animIDMagnum, false);
                        _animator.SetBool(_animIDGrenade, false);

                        pistol.SetActive(true);
                        rifle.SetActive(false);
                        shotgun.SetActive(false);
                        magnum.SetActive(false);
                        handGrenade.SetActive(false);
                        flashGrenade.SetActive(false);

                        GetComponent<ThrowingTutorial>().throwable = 0;
                    }
                    else if (currentWeapons[i].Equals("Assault Rifle"))
                    {
                        _animator.SetBool(_animIDPistol, false);
                        _animator.SetBool(_animIDRifle, true);
                        _animator.SetBool(_animIDShotgun, false);
                        _animator.SetBool(_animIDMagnum, false);
                        _animator.SetBool(_animIDGrenade, false);

                        pistol.SetActive(false);
                        rifle.SetActive(true);
                        shotgun.SetActive(false);
                        magnum.SetActive(false);
                        handGrenade.SetActive(false);
                        flashGrenade.SetActive(false);

                        string ammo = rifle.GetComponent<Weapon>().capacity.ToString();
                        AmmoUI.text = ammo + "/" + GameLogic.instance.rifleAmmo;

                        GetComponent<ThrowingTutorial>().throwable = 0;
                    }
                    else if (currentWeapons[i].Equals("Shotgun"))
                    {
                        _animator.SetBool(_animIDPistol, false);
                        _animator.SetBool(_animIDRifle, false);
                        _animator.SetBool(_animIDShotgun, true);
                        _animator.SetBool(_animIDMagnum, false);
                        _animator.SetBool(_animIDGrenade, false);

                        pistol.SetActive(false);
                        rifle.SetActive(false);
                        shotgun.SetActive(true);
                        magnum.SetActive(false);
                        handGrenade.SetActive(false);
                        flashGrenade.SetActive(false);

                        string ammo = shotgun.GetComponent<Weapon>().capacity.ToString();
                        AmmoUI.text = ammo + "/" + GameLogic.instance.shotgunAmmo;

                        GetComponent<ThrowingTutorial>().throwable = 0;
                    }
                    else if (currentWeapons[i].Equals("Magnum"))
                    {
                        _animator.SetBool(_animIDPistol, false);
                        _animator.SetBool(_animIDRifle, false);
                        _animator.SetBool(_animIDShotgun, false);
                        _animator.SetBool(_animIDMagnum, true);
                        _animator.SetBool(_animIDGrenade, false);

                        pistol.SetActive(false);
                        rifle.SetActive(false);
                        shotgun.SetActive(false);
                        magnum.SetActive(true);
                        handGrenade.SetActive(false);
                        flashGrenade.SetActive(false);

                        string ammo = magnum.GetComponent<Weapon>().capacity.ToString();
                        AmmoUI.text = ammo + "/" + GameLogic.instance.magnumAmmo;

                        GetComponent<ThrowingTutorial>().throwable = 0;
                    }
                    else if (currentWeapons[i].Equals("Hand Grenade"))
                    {
                        _animator.SetBool(_animIDPistol, false);
                        _animator.SetBool(_animIDRifle, false);
                        _animator.SetBool(_animIDShotgun, false);
                        _animator.SetBool(_animIDMagnum, false);
                        _animator.SetBool(_animIDGrenade, true);

                        pistol.SetActive(false);
                        rifle.SetActive(false);
                        shotgun.SetActive(false);
                        magnum.SetActive(false);
                        handGrenade.SetActive(true);
                        flashGrenade.SetActive(false);

                        handGrenade.SetActive(true);
                        flashGrenade.SetActive(false);

                        GetComponent<ThrowingTutorial>().throwable = 1;
                    }
                    else if (currentWeapons[i].Equals("Flash Grenade"))
                    {
                        _animator.SetBool(_animIDPistol, false);
                        _animator.SetBool(_animIDRifle, false);
                        _animator.SetBool(_animIDShotgun, false);
                        _animator.SetBool(_animIDMagnum, false);
                        _animator.SetBool(_animIDGrenade, true);

                        pistol.SetActive(false);
                        rifle.SetActive(false);
                        shotgun.SetActive(false);
                        magnum.SetActive(false);
                        handGrenade.SetActive(false);
                        flashGrenade.SetActive(true);

                        GetComponent<ThrowingTutorial>().throwable = 2;
                    }
                }
            }

        }

        public void SwitchWeapon()
        {
            int currentWeaponsNumber = GameLogic.instance.currentWeapons.Count;

            _animator.SetBool(_animIDReload, false);
            _animator.SetBool(_animIDPuttingAway, true);

            if (_input.weaponIndex >= currentWeaponsNumber)
                _input.weaponIndex = 0;
            else if (_input.weaponIndex < 0)
                _input.weaponIndex = currentWeaponsNumber - 1;

            if (GameLogic.instance.currentWeapons[_input.weaponIndex] == "Pistol")
            {
                AmmoUI.gameObject.SetActive(true);
                AmmoUI.text = pistol.GetComponent<Weapon>().capacity.ToString() + "/" + GameLogic.instance.pistolAmmo;
                PistolUI.SetActive(true);
                RifleUI.SetActive(false);
                ShotgunUI.SetActive(false);
                MagnumUI.SetActive(false);
                HandGrenadeUI.SetActive(false);
                FlashGrenadeUI.SetActive(false);
            }

            else if (GameLogic.instance.currentWeapons[_input.weaponIndex] == "Shotgun")
            {
                AmmoUI.gameObject.SetActive(true);
                AmmoUI.text = shotgun.GetComponent<Weapon>().capacity.ToString() + "/" + GameLogic.instance.shotgunAmmo;
                PistolUI.SetActive(false);
                RifleUI.SetActive(false);
                ShotgunUI.SetActive(true);
                MagnumUI.SetActive(false);
                HandGrenadeUI.SetActive(false);
                FlashGrenadeUI.SetActive(false);
            }

            else if (GameLogic.instance.currentWeapons[_input.weaponIndex] == "Assault Rifle")
            {
                AmmoUI.gameObject.SetActive(true);
                AmmoUI.text = rifle.GetComponent<Weapon>().capacity.ToString() + "/" + GameLogic.instance.rifleAmmo;
                PistolUI.SetActive(false);
                RifleUI.SetActive(true);
                ShotgunUI.SetActive(false);
                MagnumUI.SetActive(false);
                HandGrenadeUI.SetActive(false);
                FlashGrenadeUI.SetActive(false);
            }

            else if (GameLogic.instance.currentWeapons[_input.weaponIndex] == "Magnum")
            {
                AmmoUI.gameObject.SetActive(true);
                AmmoUI.text = magnum.GetComponent<Weapon>().capacity.ToString() + "/" + GameLogic.instance.magnumAmmo;
                PistolUI.SetActive(false);
                RifleUI.SetActive(false);
                ShotgunUI.SetActive(false);
                MagnumUI.SetActive(true);
                HandGrenadeUI.SetActive(false);
                FlashGrenadeUI.SetActive(false);
            }

            else if (GameLogic.instance.currentWeapons[_input.weaponIndex] == "Hand Grenade")
            {
                AmmoUI.gameObject.SetActive(false);
                PistolUI.SetActive(false);
                RifleUI.SetActive(false);
                ShotgunUI.SetActive(false);
                MagnumUI.SetActive(false);
                HandGrenadeUI.SetActive(true);
                FlashGrenadeUI.SetActive(false);
            }

            else if (GameLogic.instance.currentWeapons[_input.weaponIndex] == "Flash Grenade")
            {
                AmmoUI.gameObject.SetActive(false);
                PistolUI.SetActive(false);
                RifleUI.SetActive(false);
                ShotgunUI.SetActive(false);
                MagnumUI.SetActive(false);
                HandGrenadeUI.SetActive(false);
                FlashGrenadeUI.SetActive(true);
            }

        }
        public void EquipWeapon(string weapon)
        {
            for (int i = 0; i < GameLogic.instance.currentWeapons.Count; i++)
            {
                if (GameLogic.instance.currentWeapons[i].Equals(weapon))
                {
                    _input.weaponIndex = i;
                    break;
                }
            }
            SwitchWeapon();
        }

        public void LeonJustDied()
        {
            _speed = 0;
            GameLogic.instance.LeonJustDied();
        }

        public void GameOver()
        {
            _speed = 0;
            GameLogic.instance.GameOver();
        }

        private void LateUpdate()
        {
            if (!GameLogic.instance.PauseMenu.activeSelf && !GameLogic.instance.InventoryUI.activeSelf && !GameLogic.instance.ItemPickupPanel.activeSelf)
                CameraRotation();
            if (_animator.GetBool(_animIDPuttingAway) && _animator.GetBool(_animIDRaising))
            {
                _animator.SetBool(_animIDRaising, false);
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDAiming = Animator.StringToHash("isAiming");
            _animIDSpeed = Animator.StringToHash("speed");
            _animIDDirectionX = Animator.StringToHash("directionX");
            _animIDDirectionY = Animator.StringToHash("directionY");
            _animIDPistol = Animator.StringToHash("pistol");
            _animIDRifle = Animator.StringToHash("rifle");
            _animIDShotgun = Animator.StringToHash("shotgun");
            _animIDMagnum = Animator.StringToHash("magnum");
            _animIDGrenade = Animator.StringToHash("grenade");
            _animIDDead = Animator.StringToHash("isDead");
            _animIDHit = Animator.StringToHash("isHit");
            _animIDReload = Animator.StringToHash("isReloading");
            _animIDRaising = Animator.StringToHash("isRaising");
            _animIDPuttingAway = Animator.StringToHash("isPuttingAway");
            _animIDShooting = Animator.StringToHash("isShooting");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyArea"))
            {
                if (other.name == "Greenhouse Enter")
                {
                    if (!GameLogic.instance.GreenhouseEnemiesEliminated)
                    {
                        GameLogic.instance.AlertGreenhouseEnemies();
                        Audio.GetComponent<Audio>().EnterEnemyArea();
                    }
                }
                else if (other.name == "003 Enter")
                {
                    if (!GameLogic.instance.ZeroZeroThreeEnemiesEliminated)
                    {
                        GameLogic.instance.AlertZeroZeroThreeEnimes();
                        Audio.GetComponent<Audio>().EnterEnemyArea();
                    }
                }
                else if (other.name == "102 Enter")
                {
                    if (!GameLogic.instance.OneZeroTwoEnemiesEliminated)
                    {
                        GameLogic.instance.AlertOneZeroTwoEnemies();
                        Audio.GetComponent<Audio>().EnterEnemyArea();
                    }
                }
                else if (other.name == "Spade Enter")
                {
                    if (!GameLogic.instance.SpadeEnemiesEliminated)
                    {
                        GameLogic.instance.AlertSpadeEnemies();
                        Audio.GetComponent<Audio>().EnterEnemyArea();
                    }
                }
                else if (other.name == "Heart Enter")
                {
                    if (!GameLogic.instance.HeartEnemiesEliminated)
                    {
                        GameLogic.instance.AlertHeartEnemies();
                        Audio.GetComponent<Audio>().EnterEnemyArea();
                    }
                }
                else if (other.name == "2ndFloor")
                {
                    Audio.GetComponent<Audio>().EnterSuspenseArea();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EnemyArea"))
            {
                if (other.name == "Greenhouse Enter")
                {
                    GameLogic.instance.UnalertGreenhouseEnemies();
                    if (!GameLogic.instance.GreenhouseEnemiesEliminated)
                    {
                        Audio.GetComponent<Audio>().LeaveEnemeyArea();
                    }
                }
                else if (other.name == "003 Enter")
                {
                    GameLogic.instance.UnalertZeroZeroThreeEnimes();
                    if (!GameLogic.instance.ZeroZeroThreeEnemiesEliminated)
                    {
                        Audio.GetComponent<Audio>().LeaveEnemeyArea();
                    }
                }
                else if (other.name == "102 Enter")
                {
                    GameLogic.instance.UnalertOneZeroTwoEnemies();
                    if (!GameLogic.instance.OneZeroTwoEnemiesEliminated)
                    {
                        Audio.GetComponent<Audio>().EnterSuspenseArea();
                    }
                }
                else if (other.name == "Spade Enter")
                {
                    GameLogic.instance.UnalertSpadeEnemies();
                    if (!GameLogic.instance.SpadeEnemiesEliminated)
                    {
                        Audio.GetComponent<Audio>().EnterSuspenseArea();
                    }
                }
                else if (other.name == "Heart Enter")
                {
                    GameLogic.instance.UnalertHeartEnemies();
                    if (!GameLogic.instance.HeartEnemiesEliminated)
                    {
                        Audio.GetComponent<Audio>().EnterSuspenseArea();
                    }
                }
                else if (other.name == "2ndFloor")
                {
                    Audio.GetComponent<Audio>().LeaveEnemeyArea();
                }

            }
        }
        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                //_animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);


            if (_input.aim && !_input.sprint)
            {
                crosshairs.gameObject.SetActive(true);
                VirtualCameraAim.Priority = VirtualCamera.Priority + 1;
                transform.rotation = Quaternion.Euler(0.0f, Mathf.LerpAngle(transform.rotation.eulerAngles.y, _cinemachineTargetYaw, Time.deltaTime * 500000000000), 0.0f);
                Spine.transform.localRotation = Quaternion.Euler(0, 0, -ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp));

                Quaternion targetRotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
                CinemachineCameraTarget.transform.rotation = Quaternion.Slerp(CinemachineCameraTarget.transform.rotation, targetRotation, Time.deltaTime * 500000000000);
            }
            else
            {
                crosshairs.gameObject.SetActive(false);
                VirtualCameraAim.Priority = VirtualCamera.Priority - 1;
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            _animator.SetFloat(_animIDSpeed, _speed);

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                if (!_input.aim || _input.sprint)
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            if (_input.aim && !_input.sprint)
            {
                _animator.SetBool(_animIDAiming, true);
                _animator.SetFloat(_animIDDirectionX, _input.move.x, 0.1f, Time.deltaTime);
                _animator.SetFloat(_animIDDirectionY, _input.move.y, 0.1f, Time.deltaTime);
            }
            else
            {
                _animator.SetBool(_animIDAiming, false);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                //_animator.SetFloat(_animIDSpeed, _animationBlend);
                //_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    //_animator.SetBool(_animIDJump, false);
                    //_animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        //_animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        //_animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}