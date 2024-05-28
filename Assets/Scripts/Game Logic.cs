using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;

    public GameObject Player;
    public GameObject Jack;
    public GameObject GrapplingEnemy;

    public GameObject Blood;

    public GameObject[] GreenhouseEnemies;
    public GameObject[] ZeroZeroThreeEnemies;
    public GameObject[] OneZeroTwoEnemies;
    public GameObject[] SpadeEnemies;
    public GameObject[] HeartEnemies;

    [SerializeField] private GameObject Merchant;

    public bool GreenhouseEnemiesEliminated;
    public bool ZeroZeroThreeEnemiesEliminated;
    public bool OneZeroTwoEnemiesEliminated;
    public bool SpadeEnemiesEliminated;
    public bool HeartEnemiesEliminated;

    public List<GameObject> GoldCoins;
    public GameObject OldKey;
    [SerializeField] private List<GameObject> Doors;
    [SerializeField] private List<GameObject> Keys;
    [SerializeField] private List<GameObject> Treasures;
    [SerializeField] private GameObject Magnum;

    [SerializeField] private GameObject DeathScreen;

    [SerializeField] private GameObject MerchantImage;
    public GameObject Store;
    public GameObject Inventory;

    public GameObject InventoryUI;

    [SerializeField] private GameObject Credits;

    public List<string> currentWeapons;

    public GameObject PauseMenu;
    [SerializeField] private GameObject OptionButtons;
    [SerializeField] private GameObject PauseButtons;

    [SerializeField] private CanvasGroup PauseMenuCanvas;
    [SerializeField] private CanvasGroup OptionsCanvas;

    [SerializeField] private Animator ContinueAnimator;
    [SerializeField] private Animator RestartAnimator;
    [SerializeField] private Animator OptionsAnimator;
    [SerializeField] private Animator ExitAnimator;

    public GameObject Gold;
    public GameObject GoldManager;
    public GameObject GoldCanvas;
    public GameObject TreasureCanvas;
    public GameObject KeyCanvas;
    public GameObject DoorCanvas;
    public GameObject MagnumCanvas;
    public GameObject MerchantCanvas;
    public GameObject KnifeCanvas;

    [SerializeField] private GameObject[] Mirrors;

    private bool isFading = false;
    private float fadeTimeElapsed = 0f;
    private int fader = 0;

    private bool isRotatingDoor = false;
    private Transform RotatingDoor;

    public Audio AudioScript;

    public Image progressBar;

    public GameObject PostProcessingVolume;
    public GameObject StrugglingCanvas;

    public Text TopLeftAmmo;
    public Text TopMiddleAmmo;
    public Text TopRightAmmo;
    public Text BottomLeftAmmo;
    public Text BottomMiddleAmmo;
    public Text BottomRightAmmo;

    [SerializeField] private GameObject SlotOptions;

    public int pistolAmmo;
    public int rifleAmmo;
    public int shotgunAmmo;
    public int magnumAmmo;

    public int storagePistolAmmo;
    public int storageRifleAmmo;
    public int storageShotgunAmmo;
    public int storageMagnumAmmo;

    bool youWon = false;

    [SerializeField] private CinemachineVirtualCamera StoreCamera;

    public static GameLogic instance;

    [SerializeField] private GameObject PauseMenuFirst;
    [SerializeField] private GameObject OptionsMenuFirst;
    [SerializeField] private GameObject InventoryFirst;
    [SerializeField] private GameObject StoreFirst;
    [SerializeField] private GameObject CreditsFirst;
    [SerializeField] private GameObject DeathScreenFirst;

    [SerializeField] private Text MasterText;
    [SerializeField] private Text SFXText;
    [SerializeField] private Text MusicText;
    [SerializeField] private Text VoiceText;

    public GameObject ItemPickupPanel;

    [SerializeField] private GameObject HeartKeyPanel;
    [SerializeField] private GameObject SpadeKeyPanel;
    [SerializeField] private GameObject DiamondKeyPanel;
    [SerializeField] private GameObject ClubKeyPanel;
    [SerializeField] private GameObject OldKeyPanel;
    [SerializeField] private GameObject KeycardPanel;
    [SerializeField] private GameObject GoldbarsPanel;
    [SerializeField] private GameObject DiamondPanel;
    [SerializeField] private GameObject RubyPanel;
    [SerializeField] private GameObject EmeraldPanel;
    [SerializeField] private GameObject MagnumPanel;


    private GameObject EventSystemSelectedObject;
    private Color NotSelectedColor = new(1, 1, 1, 1f);
    private Color SelectedColor = new(166f / 255f, 166f / 255f, 166f / 255f, 1f);

    private float timeSinceBlend = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        instance = this;
        AudioScript.PlaySaferoomMusic();
        currentWeapons = new List<string>
        {
            "Pistol"
        };
    }

    public bool conatinsOneWeapon()
    {
        if (currentWeapons.Count == 1)
        {
            return true;
        }
        else if (!currentWeapons.Contains("Pistol") && !currentWeapons.Contains("Assault Rifle") && !currentWeapons.Contains("Shotgun"))
        {
            return true;
        }
        else if (!currentWeapons.Contains("Pistol") && !currentWeapons.Contains("Assault Rifle") && !currentWeapons.Contains("Magnum"))
        {
            return true;
        }
        else if (!currentWeapons.Contains("Pistol") && !currentWeapons.Contains("Shotgun") && !currentWeapons.Contains("Magnum"))
        {
            return true;
        }
        else if (!currentWeapons.Contains("Assault Rifle") && !currentWeapons.Contains("Shotgun") && !currentWeapons.Contains("Magnum"))
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        if (instance == null)
            instance = this;

        if (!InventoryUI.activeSelf && !ThirdPersonController.instance.isDead)
            Interact();

        else if (ThirdPersonController.instance.isDead)
        {
            MerchantCanvas.SetActive(false);
            KnifeCanvas.SetActive(false);
            GoldCanvas.SetActive(false);
            TreasureCanvas.SetActive(false);
            KeyCanvas.SetActive(false);
            DoorCanvas.SetActive(false);
            MagnumCanvas.SetActive(false);
        }

        if (Store.activeSelf)
        {
            if (ThirdPersonController.instance._playerInput.currentControlScheme.Equals("KeyboardMouse"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        else if (timeSinceBlend <= 0)
        {

            Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0.2f;
        }
        else
        {
            timeSinceBlend -= Time.deltaTime;
        }

        CheckEnemiesDead();
        PauseMenuFading();

        string currentWeapon = currentWeapons[ThirdPersonController.instance._input.weaponIndex];

        if (!InventoryUI.activeSelf)
        {
            SlotOptions.transform.GetChild(0).gameObject.SetActive(false);
            SlotOptions.transform.GetChild(1).gameObject.SetActive(false);
            SlotOptions.transform.GetChild(2).gameObject.SetActive(false);
            SlotOptions.transform.GetChild(3).gameObject.SetActive(false);
            SlotOptions.transform.GetChild(4).gameObject.SetActive(false);
            SlotOptions.transform.GetChild(5).gameObject.SetActive(false);
        }

        for (int i = 0; i < Inventory.GetComponent<Inventory>().scriptableObjects.Length; i++)
        {
            if (currentWeapon == Inventory.GetComponent<Inventory>().scriptableObjects[i].itemName)
            {
                Inventory.GetComponent<Inventory>().equippedWeapon = Inventory.GetComponent<Inventory>().scriptableObjects[i];
                Inventory.GetComponent<Inventory>().weapon.gameObject.SetActive(true);
                Inventory.GetComponent<Inventory>().weapon.sprite = Inventory.GetComponent<Inventory>().equippedWeapon.sprite;
            }
        }
        if (ThirdPersonController.instance._input.inventory && !PauseMenu.activeSelf && !Store.activeSelf && !ThirdPersonController.instance.isDead)
        {
            ThirdPersonController.instance._input.inventory = false;
            InventoryUI.SetActive(!InventoryUI.activeSelf);

            if (InventoryUI.activeSelf)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                GoldCanvas.SetActive(false);
                TreasureCanvas.SetActive(false);
                KeyCanvas.SetActive(false);
                DoorCanvas.SetActive(false);
                MagnumCanvas.SetActive(false);
                MerchantCanvas.SetActive(false);
                KnifeCanvas.SetActive(false);

                ThirdPersonController.instance.enabled = false;

                EventSystem.current.SetSelectedGameObject(InventoryFirst);
            }

            else
            {
                ResumeGame();
            }

        }
        if (ThirdPersonController.instance._input.pause && !ThirdPersonController.instance.isDead)
        {
            ThirdPersonController.instance._input.pause = false;
            if (Store.activeSelf)
            {
                CloseStore();
            }

            else
            {

                if (!PauseMenu.activeSelf && !InventoryUI.activeSelf && !ItemPickupPanel.activeSelf)
                {
                    PauseMenu.SetActive(true);
                    AudioScript.PlayPauseSoundtrack();
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    EventSystem.current.SetSelectedGameObject(PauseMenuFirst);

                    ThirdPersonController.instance.enabled = false;
                }
                else
                {
                    ResumeGame();
                }
            }
        }
        if (isRotatingDoor)
        {

            if (RotatingDoor.gameObject.name.Equals("Storage Door"))
            {
                RotatingDoor.localRotation = Quaternion.Slerp(RotatingDoor.localRotation, Quaternion.Euler(RotatingDoor.localRotation.x, -60f, RotatingDoor.localRotation.z), 5 * Time.deltaTime);
            }
            else if (RotatingDoor.gameObject.name.Equals("Club Door") || RotatingDoor.gameObject.name.Equals("Diamond Door Hinge"))
            {
                RotatingDoor.localRotation = Quaternion.Slerp(RotatingDoor.localRotation, Quaternion.Euler(RotatingDoor.localRotation.x, 0, RotatingDoor.localRotation.z), 5 * Time.deltaTime);
            }
            else
            {
                RotatingDoor.localRotation = Quaternion.Slerp(RotatingDoor.localRotation, Quaternion.Euler(RotatingDoor.localRotation.x, -28f, RotatingDoor.localRotation.z), 5 * Time.deltaTime);
            }
        }

        if (EventSystemSelectedObject && EventSystem.current.currentSelectedGameObject && EventSystemSelectedObject.name != EventSystem.current.currentSelectedGameObject.name && EventSystem.current.currentSelectedGameObject.name == "Master Slider")
        {
            MasterText.color = SelectedColor;
            SFXText.color = NotSelectedColor;
            MusicText.color = NotSelectedColor;
            VoiceText.color = NotSelectedColor;

            AudioScript.PlayHoverSound();
        }

        else if (EventSystemSelectedObject && EventSystem.current.currentSelectedGameObject && EventSystemSelectedObject.name != EventSystem.current.currentSelectedGameObject.name && EventSystem.current.currentSelectedGameObject.name == "SFX Slider")
        {
            SFXText.color = SelectedColor;
            MasterText.color = NotSelectedColor;
            MusicText.color = NotSelectedColor;
            VoiceText.color = NotSelectedColor;

            AudioScript.PlayHoverSound();
        }

        else if (EventSystemSelectedObject && EventSystem.current.currentSelectedGameObject && EventSystemSelectedObject.name != EventSystem.current.currentSelectedGameObject.name && EventSystem.current.currentSelectedGameObject.name == "Music Slider")
        {
            MusicText.color = SelectedColor;
            MasterText.color = NotSelectedColor;
            SFXText.color = NotSelectedColor;
            VoiceText.color = NotSelectedColor;

            AudioScript.PlayHoverSound();
        }

        else if (EventSystemSelectedObject && EventSystem.current.currentSelectedGameObject && EventSystemSelectedObject.name != EventSystem.current.currentSelectedGameObject.name && EventSystem.current.currentSelectedGameObject.name == "Voice Slider")
        {
            VoiceText.color = SelectedColor;
            MasterText.color = NotSelectedColor;
            SFXText.color = NotSelectedColor;
            MusicText.color = NotSelectedColor;

            AudioScript.PlayHoverSound();
        }

        else if (EventSystemSelectedObject && EventSystem.current.currentSelectedGameObject && EventSystemSelectedObject.name != EventSystem.current.currentSelectedGameObject.name && EventSystem.current.currentSelectedGameObject.name == "Back")
        {
            MasterText.color = NotSelectedColor;
            SFXText.color = NotSelectedColor;
            MusicText.color = NotSelectedColor;
            VoiceText.color = NotSelectedColor;

            AudioScript.PlayHoverSound();
        }

        EventSystemSelectedObject = EventSystem.current.currentSelectedGameObject;

    }


    public void UpdateAmmo()
    {
        //Remove All Ammo
        int i = 6;

        while (i > 0)
        {
            Inventory.GetComponent<Inventory>().RemoveFromInventory(Inventory.GetComponent<Inventory>().scriptableObjects[6]);
            Inventory.GetComponent<Inventory>().RemoveFromInventory(Inventory.GetComponent<Inventory>().scriptableObjects[7]);
            Inventory.GetComponent<Inventory>().RemoveFromInventory(Inventory.GetComponent<Inventory>().scriptableObjects[8]);
            Inventory.GetComponent<Inventory>().RemoveFromInventory(Inventory.GetComponent<Inventory>().scriptableObjects[9]);
            i--;
        }

        TopLeftAmmo.gameObject.SetActive(false);
        TopMiddleAmmo.gameObject.SetActive(false);
        TopRightAmmo.gameObject.SetActive(false);
        BottomLeftAmmo.gameObject.SetActive(false);
        BottomMiddleAmmo.gameObject.SetActive(false);
        BottomRightAmmo.gameObject.SetActive(false);

        float numOfPistolAmmo = Mathf.Ceil((float)pistolAmmo / Inventory.GetComponent<Inventory>().maxPistolAmmo);
        float numOfRifleAmmo = Mathf.Ceil((float)rifleAmmo / Inventory.GetComponent<Inventory>().maxRifleAmmo);
        float numOfShotgunAmmo = Mathf.Ceil((float)shotgunAmmo / Inventory.GetComponent<Inventory>().maxShotgunAmmo);
        float numOfMagnumAmmo = Mathf.Ceil((float)magnumAmmo / Inventory.GetComponent<Inventory>().maxMagnumAmmo);

        while (numOfPistolAmmo > 0)
        {
            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[8]);

            if (numOfPistolAmmo == 1)
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    if (pistolAmmo % Inventory.GetComponent<Inventory>().maxPistolAmmo == 0)
                    {
                        TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    }
                    else
                    {
                        TopLeftAmmo.text = (pistolAmmo - Mathf.Floor((float)pistolAmmo / Inventory.GetComponent<Inventory>().maxPistolAmmo) * Inventory.GetComponent<Inventory>().maxPistolAmmo).ToString();
                    }
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    if (pistolAmmo % Inventory.GetComponent<Inventory>().maxPistolAmmo == 0)
                    {
                        TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    }
                    else
                    {
                        TopMiddleAmmo.text = (pistolAmmo - Mathf.Floor((float)pistolAmmo / Inventory.GetComponent<Inventory>().maxPistolAmmo) * Inventory.GetComponent<Inventory>().maxPistolAmmo).ToString();
                    }
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    if (pistolAmmo % Inventory.GetComponent<Inventory>().maxPistolAmmo == 0)
                    {
                        TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    }
                    else
                    {
                        TopRightAmmo.text = (pistolAmmo - Mathf.Floor((float)pistolAmmo / Inventory.GetComponent<Inventory>().maxPistolAmmo) * Inventory.GetComponent<Inventory>().maxPistolAmmo).ToString();
                    }
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    if (pistolAmmo % Inventory.GetComponent<Inventory>().maxPistolAmmo == 0)
                    {
                        BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    }
                    else
                    {
                        BottomLeftAmmo.text = (pistolAmmo - Mathf.Floor((float)pistolAmmo / Inventory.GetComponent<Inventory>().maxPistolAmmo) * Inventory.GetComponent<Inventory>().maxPistolAmmo).ToString();
                    }
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    if (pistolAmmo % Inventory.GetComponent<Inventory>().maxPistolAmmo == 0)
                    {
                        BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    }
                    else
                    {
                        BottomMiddleAmmo.text = (pistolAmmo - Mathf.Floor((float)pistolAmmo / Inventory.GetComponent<Inventory>().maxPistolAmmo) * Inventory.GetComponent<Inventory>().maxPistolAmmo).ToString();
                    }
                    BottomMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    if (pistolAmmo % Inventory.GetComponent<Inventory>().maxPistolAmmo == 0)
                    {
                        BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    }
                    else
                    {
                        BottomRightAmmo.text = (pistolAmmo - Mathf.Floor((float)pistolAmmo / Inventory.GetComponent<Inventory>().maxPistolAmmo) * Inventory.GetComponent<Inventory>().maxPistolAmmo).ToString();
                    }
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            else
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    BottomMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxPistolAmmo.ToString();
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            numOfPistolAmmo--;
        }

        while (numOfRifleAmmo > 0)
        {
            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[6]);
            if (numOfRifleAmmo == 1)
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    if (rifleAmmo % Inventory.GetComponent<Inventory>().maxRifleAmmo == 0)
                    {
                        TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    }
                    else
                    {
                        TopLeftAmmo.text = (rifleAmmo - Mathf.Floor((float)rifleAmmo / Inventory.GetComponent<Inventory>().maxRifleAmmo) * Inventory.GetComponent<Inventory>().maxRifleAmmo).ToString();
                    }
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    if (rifleAmmo % Inventory.GetComponent<Inventory>().maxRifleAmmo == 0)
                    {
                        TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    }
                    else
                    {
                        TopMiddleAmmo.text = (rifleAmmo - Mathf.Floor((float)rifleAmmo / Inventory.GetComponent<Inventory>().maxRifleAmmo) * Inventory.GetComponent<Inventory>().maxRifleAmmo).ToString();
                    }
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    if (rifleAmmo % Inventory.GetComponent<Inventory>().maxRifleAmmo == 0)
                    {
                        TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    }
                    else
                    {
                        TopRightAmmo.text = (rifleAmmo - Mathf.Floor((float)rifleAmmo / Inventory.GetComponent<Inventory>().maxRifleAmmo) * Inventory.GetComponent<Inventory>().maxRifleAmmo).ToString();
                    }
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    if (rifleAmmo % Inventory.GetComponent<Inventory>().maxRifleAmmo == 0)
                    {
                        BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    }
                    else
                    {
                        BottomLeftAmmo.text = (rifleAmmo - Mathf.Floor((float)rifleAmmo / Inventory.GetComponent<Inventory>().maxRifleAmmo) * Inventory.GetComponent<Inventory>().maxRifleAmmo).ToString();
                    }
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    if (rifleAmmo % Inventory.GetComponent<Inventory>().maxRifleAmmo == 0)
                    {
                        BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    }
                    else
                    {
                        BottomMiddleAmmo.text = (rifleAmmo - Mathf.Floor((float)rifleAmmo / Inventory.GetComponent<Inventory>().maxRifleAmmo) * Inventory.GetComponent<Inventory>().maxRifleAmmo).ToString();
                    }
                    BottomMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    if (rifleAmmo % Inventory.GetComponent<Inventory>().maxRifleAmmo == 0)
                    {
                        BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    }
                    else
                    {
                        BottomRightAmmo.text = (rifleAmmo - Mathf.Floor((float)rifleAmmo / Inventory.GetComponent<Inventory>().maxRifleAmmo) * Inventory.GetComponent<Inventory>().maxRifleAmmo).ToString();
                    }
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            else
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    BottomMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxRifleAmmo.ToString();
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            numOfRifleAmmo--;
        }

        while (numOfShotgunAmmo > 0)
        {
            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[7]);
            if (numOfShotgunAmmo == 1)
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    if (shotgunAmmo % Inventory.GetComponent<Inventory>().maxShotgunAmmo == 0)
                    {
                        TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    }
                    else
                    {
                        TopLeftAmmo.text = (shotgunAmmo - Mathf.Floor((float)shotgunAmmo / Inventory.GetComponent<Inventory>().maxShotgunAmmo) * Inventory.GetComponent<Inventory>().maxShotgunAmmo).ToString();
                    }
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    if (shotgunAmmo % Inventory.GetComponent<Inventory>().maxShotgunAmmo == 0)
                    {
                        TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    }
                    else
                    {
                        TopMiddleAmmo.text = (shotgunAmmo - Mathf.Floor((float)shotgunAmmo / Inventory.GetComponent<Inventory>().maxShotgunAmmo) * Inventory.GetComponent<Inventory>().maxShotgunAmmo).ToString();
                    }
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    if (shotgunAmmo % Inventory.GetComponent<Inventory>().maxShotgunAmmo == 0)
                    {
                        TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    }
                    else
                    {
                        TopRightAmmo.text = (shotgunAmmo - Mathf.Floor((float)shotgunAmmo / Inventory.GetComponent<Inventory>().maxShotgunAmmo) * Inventory.GetComponent<Inventory>().maxShotgunAmmo).ToString();
                    }
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    if (shotgunAmmo % Inventory.GetComponent<Inventory>().maxShotgunAmmo == 0)
                    {
                        BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    }
                    else
                    {
                        BottomLeftAmmo.text = (shotgunAmmo - Mathf.Floor((float)shotgunAmmo / Inventory.GetComponent<Inventory>().maxShotgunAmmo) * Inventory.GetComponent<Inventory>().maxShotgunAmmo).ToString();
                    }
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    if (shotgunAmmo % Inventory.GetComponent<Inventory>().maxShotgunAmmo == 0)
                    {
                        BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    }
                    else
                    {
                        BottomMiddleAmmo.text = (shotgunAmmo - Mathf.Floor((float)shotgunAmmo / Inventory.GetComponent<Inventory>().maxShotgunAmmo) * Inventory.GetComponent<Inventory>().maxShotgunAmmo).ToString();
                    }
                    BottomMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    if (shotgunAmmo % Inventory.GetComponent<Inventory>().maxShotgunAmmo == 0)
                    {
                        BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    }
                    else
                    {
                        BottomRightAmmo.text = (shotgunAmmo - Mathf.Floor((float)shotgunAmmo / Inventory.GetComponent<Inventory>().maxShotgunAmmo) * Inventory.GetComponent<Inventory>().maxShotgunAmmo).ToString();
                    }
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            else
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    BottomMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxShotgunAmmo.ToString();
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            numOfShotgunAmmo--;
        }

        while (numOfMagnumAmmo > 0)
        {
            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[9]);
            if (numOfMagnumAmmo == 1)
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    if (magnumAmmo % Inventory.GetComponent<Inventory>().maxMagnumAmmo == 0)
                    {
                        TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    }
                    else
                    {
                        TopLeftAmmo.text = (magnumAmmo - Mathf.Floor((float)magnumAmmo / Inventory.GetComponent<Inventory>().maxMagnumAmmo) * Inventory.GetComponent<Inventory>().maxMagnumAmmo).ToString();
                    }
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    if (magnumAmmo % Inventory.GetComponent<Inventory>().maxMagnumAmmo == 0)
                    {
                        TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    }
                    else
                    {
                        TopMiddleAmmo.text = (magnumAmmo - Mathf.Floor((float)magnumAmmo / Inventory.GetComponent<Inventory>().maxMagnumAmmo) * Inventory.GetComponent<Inventory>().maxMagnumAmmo).ToString();
                    }
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    if (magnumAmmo % Inventory.GetComponent<Inventory>().maxMagnumAmmo == 0)
                    {
                        TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    }
                    else
                    {
                        TopRightAmmo.text = (magnumAmmo - Mathf.Floor((float)magnumAmmo / Inventory.GetComponent<Inventory>().maxMagnumAmmo) * Inventory.GetComponent<Inventory>().maxMagnumAmmo).ToString();
                    }
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    if (magnumAmmo % Inventory.GetComponent<Inventory>().maxMagnumAmmo == 0)
                    {
                        BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    }
                    else
                    {
                        BottomLeftAmmo.text = (magnumAmmo - Mathf.Floor((float)magnumAmmo / Inventory.GetComponent<Inventory>().maxMagnumAmmo) * Inventory.GetComponent<Inventory>().maxMagnumAmmo).ToString();
                    }
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    if (magnumAmmo % Inventory.GetComponent<Inventory>().maxMagnumAmmo == 0)
                    {
                        BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    }
                    else
                    {
                        BottomMiddleAmmo.text = (magnumAmmo - Mathf.Floor((float)magnumAmmo / Inventory.GetComponent<Inventory>().maxMagnumAmmo) * Inventory.GetComponent<Inventory>().maxMagnumAmmo).ToString();
                    }
                    BottomMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    if (magnumAmmo % Inventory.GetComponent<Inventory>().maxMagnumAmmo == 0)
                    {
                        BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    }
                    else
                    {
                        BottomRightAmmo.text = (magnumAmmo - Mathf.Floor((float)magnumAmmo / Inventory.GetComponent<Inventory>().maxMagnumAmmo) * Inventory.GetComponent<Inventory>().maxMagnumAmmo).ToString();
                    }
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            else
            {
                if (Inventory.GetComponent<Inventory>().items.Count == 1)
                {
                    TopLeftAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    TopLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 2)
                {
                    TopMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    TopMiddleAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 3)
                {
                    TopRightAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    TopRightAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 4)
                {
                    BottomLeftAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    BottomLeftAmmo.gameObject.SetActive(true);
                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 5)
                {
                    BottomMiddleAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    BottomMiddleAmmo.gameObject.SetActive(true);

                }
                else if (Inventory.GetComponent<Inventory>().items.Count == 6)
                {
                    BottomRightAmmo.text = Inventory.GetComponent<Inventory>().maxMagnumAmmo.ToString();
                    BottomRightAmmo.gameObject.SetActive(true);
                }
            }
            numOfMagnumAmmo--;
        }
        if (currentWeapons[ThirdPersonController.instance._input.weaponIndex] == "Pistol")
        {
            ThirdPersonController.instance.AmmoUI.text = ThirdPersonController.instance.pistol.GetComponent<Weapon>().capacity + "/" + pistolAmmo;
        }
        else if (currentWeapons[ThirdPersonController.instance._input.weaponIndex] == "Shotgun")
        {
            ThirdPersonController.instance.AmmoUI.text = ThirdPersonController.instance.shotgun.GetComponent<Weapon>().capacity + "/" + shotgunAmmo;
        }

        else if (currentWeapons[ThirdPersonController.instance._input.weaponIndex] == "Assault Rifle")
        {
            ThirdPersonController.instance.AmmoUI.text = ThirdPersonController.instance.rifle.GetComponent<Weapon>().capacity + "/" + rifleAmmo;
        }

        else if (currentWeapons[ThirdPersonController.instance._input.weaponIndex] == "Magnum")
        {
            ThirdPersonController.instance.AmmoUI.text = ThirdPersonController.instance.magnum.GetComponent<Weapon>().capacity + "/" + magnumAmmo;
        }
    }

    private void Interact()
    {

        if (GrapplingEnemy && GrapplingEnemy.GetComponent<Enemy>().isGrappling)
        {
            if (ThirdPersonController.instance._input.interact)
            {
                ThirdPersonController.instance._input.interact = false;
                GrapplingEnemy.GetComponent<Enemy>().grappleButtonBreakPressCounter++;
                GameLogic.instance.progressBar.GetComponent<RectTransform>().offsetMax = new Vector2(-435 + 435 * GrapplingEnemy.GetComponent<Enemy>().grappleButtonBreakPressCounter / 20, 0);
            }
        }
        else
        {
            GrapplingEnemy = null;
        }

        Vector3 dirFromAtoB;
        float dotProd;

        if (OldKey)
        {

            dirFromAtoB = (OldKey.transform.position - Player.transform.position).normalized;
            dotProd = Vector3.Dot(dirFromAtoB, Player.transform.forward);

            if (Vector3.Distance(Player.transform.position, OldKey.transform.position) < 2 && dotProd > 0.25)
            {
                KeyCanvas.SetActive(true);
                if (ThirdPersonController.instance._input.interact)
                {
                    if (Inventory.GetComponent<Inventory>().items.Count < 6)
                    {
                        ThirdPersonController.instance._input.interact = false;
                        Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[28]);
                        AudioScript.PlayPickItemSound();
                        Destroy(OldKey);
                        KeyCanvas.SetActive(false);

                        ItemPickUp("Old Key");
                    }
                    else
                    {
                        AudioScript.PlayErrorSound();
                    }
                }

            }
            else
            {
                KeyCanvas.SetActive(false);
            }
        }

        if (Vector3.Distance(Player.transform.position, Merchant.transform.position) < 5)
        {
            if (Store.activeSelf)
                MerchantCanvas.SetActive(false);
            else
                MerchantCanvas.SetActive(true);


            if (ThirdPersonController.instance._input.interact)
            {
                ThirdPersonController.instance._input.interact = false;
                OpenStore();
            }
        }
        else
        {
            MerchantCanvas.SetActive(false);
        }

        dirFromAtoB = (Magnum.transform.position - Player.transform.position).normalized;
        dotProd = Vector3.Dot(dirFromAtoB, Player.transform.forward);

        if (Vector3.Distance(Player.transform.position, Magnum.transform.position) < 3 && dotProd > 0.25 && Magnum.activeSelf)
        {
            MagnumCanvas.SetActive(true);
            if (ThirdPersonController.instance._input.interact)
            {
                ThirdPersonController.instance._input.interact = false;
                if (Inventory.GetComponent<Inventory>().items.Count < 6)
                {
                    Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[3]);
                    currentWeapons.Add("Magnum");
                    ThirdPersonController.instance.EquipWeapon("Magnum");
                    Magnum.SetActive(false);

                    ItemPickUp("Magnum");
                    AudioScript.PlayTreasurePickupSound();
                }
                else
                {
                    AudioScript.PlayErrorSound();
                }
            }
        }
        else
        {
            MagnumCanvas.SetActive(false);
        }

        Enemy nearestStunnedEnemy = Player.GetComponent<KnifeScript>().FindNearestEnemy();
        if (nearestStunnedEnemy != null)
        {
            if (ThirdPersonController.instance._input.interact)
                Player.GetComponent<KnifeScript>().Stab(nearestStunnedEnemy);
        }

        for (int i = 0; i < Keys.Count; i++)
        {
            dirFromAtoB = (Keys[i].transform.position - Player.transform.position).normalized;
            dotProd = Vector3.Dot(dirFromAtoB, Player.transform.forward);

            if (Vector3.Distance(Player.transform.position, Keys[i].transform.position) < 3 && dotProd > 0.25 && !youWon)
            {
                KeyCanvas.SetActive(true);
                if (ThirdPersonController.instance._input.interact)
                {
                    ThirdPersonController.instance._input.interact = false;
                    if (Inventory.GetComponent<Inventory>().items.Count < 6)
                    {
                        Keys[i].SetActive(false);
                        KeyCanvas.SetActive(false);
                        if (Keys[i].name.Equals("Spade Key"))
                        {
                            ItemPickUp(Keys[i].name);
                            Keys.RemoveAt(i);
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[21]);
                            AudioScript.PlayPickItemSound();
                        }
                        else if (Keys[i].name.Equals("Heart Key"))
                        {
                            ItemPickUp(Keys[i].name);
                            Keys.RemoveAt(i);
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[22]);
                            AudioScript.PlayPickItemSound();
                        }
                        else if (Keys[i].name.Equals("Keycard"))
                        {
                            ItemPickUp(Keys[i].name);
                            Keys.RemoveAt(i);
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[20]);
                            AudioScript.PlayPickItemSound();
                        }
                        else if (Keys[i].name.Equals("Club Key"))
                        {
                            ItemPickUp(Keys[i].name);
                            Keys.RemoveAt(i);
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[23]);
                            AudioScript.PlayPickItemSound();
                        }
                        else if (Keys[i].name.Equals("Diamond Key"))
                        {
                            ItemPickUp(Keys[i].name);
                            Keys.RemoveAt(i);
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[24]);
                            AudioScript.PlayPickItemSound();
                        }
                    }
                    else
                    {
                        AudioScript.PlayErrorSound();
                    }
                }

                break;
            }
            else
            {
                KeyCanvas.SetActive(false);
            }
        }

        for (int i = 0; i < Doors.Count; i++)
        {
            dirFromAtoB = (Doors[i].transform.position - Player.transform.position).normalized;
            dotProd = Vector3.Dot(dirFromAtoB, Player.transform.forward);

            if (Vector3.Distance(Player.transform.position, Doors[i].transform.position) < 2 && dotProd > 0.25 && !youWon)
            {
                DoorCanvas.SetActive(true);

                if (ThirdPersonController.instance._input.interact)
                {
                    ThirdPersonController.instance._input.interact = false;
                    Inventory.GetComponent<Inventory>().items.ForEach(item =>
                    {

                        if (Doors[i].name.Equals("Diamond Door") && item.itemName.Equals("Diamond Key"))
                        {
                            RotatingDoor = Doors[i].transform.GetChild(1);
                            isRotatingDoor = true;
                            Inventory.GetComponent<Inventory>().RemoveFromInventory(item);
                            UpdateAmmo();
                            AudioScript.PlayDoorOpenSound();
                            Doors.RemoveAt(i);
                        }
                        else if (Doors[i].name.Equals("Spade Door") && item.itemName.Equals("Spade Key"))
                        {
                            RotatingDoor = Doors[i].transform.GetChild(1);
                            isRotatingDoor = true;
                            Inventory.GetComponent<Inventory>().RemoveFromInventory(item);
                            UpdateAmmo();
                            AudioScript.PlayDoorOpenSound();
                            Doors.RemoveAt(i);
                        }
                        else if (Doors[i].name.Equals("Heart Door") && item.itemName.Equals("Heart Key"))
                        {
                            RotatingDoor = Doors[i].transform.GetChild(1);
                            isRotatingDoor = true;
                            Inventory.GetComponent<Inventory>().RemoveFromInventory(item);
                            UpdateAmmo();
                            AudioScript.PlayDoorOpenSound();
                            Doors.RemoveAt(i);
                        }

                        else if (Doors[i].name.Equals("Storage Door") && item.itemName.Equals("Keycard"))
                        {
                            RotatingDoor = Doors[i].transform.GetChild(1);
                            isRotatingDoor = true;
                            Inventory.GetComponent<Inventory>().RemoveFromInventory(item);
                            UpdateAmmo();
                            AudioScript.PlayDoorOpenSound();
                            Doors.RemoveAt(i);
                        }
                        else if (Doors[i].name.Equals("Club Door Hinge") && item.itemName.Equals("Club Key"))
                        {
                            RotatingDoor = Doors[i].transform;
                            Jack.GetComponent<Jack>().isIdle = false;
                            isRotatingDoor = true;
                            Inventory.GetComponent<Inventory>().RemoveFromInventory(item);
                            UpdateAmmo();
                            AudioScript.PlayJackSoundtrack();
                            AudioScript.PlayDoorOpenSound();
                            Doors.RemoveAt(i);
                        }
                        else if (Doors[i].name.Equals("Front Door") && item.itemName.Equals("Old Key"))
                        {
                            YouWon();
                        }
                        else
                        {
                            AudioScript.PlayLockedDoorSound();
                        }
                    });
                }
                break;
            }
            else
            {
                DoorCanvas.SetActive(false);
            }
        }

        for (int i = 0; i < GoldCoins.Count; i++)
        {
            dirFromAtoB = (GoldCoins[i].transform.position - Player.transform.position).normalized;
            dotProd = Vector3.Dot(dirFromAtoB, Player.transform.forward);

            if (Vector3.Distance(Player.transform.position, GoldCoins[i].transform.position) < 2 && dotProd > 0.25f)
            {
                GoldCanvas.SetActive(true);
                if (ThirdPersonController.instance._input.interact)
                {
                    ThirdPersonController.instance._input.interact = false;
                    GoldCanvas.SetActive(false);
                    GameLogic.instance.GoldManager.GetComponent<Gold>().PickupGold(35);
                    GameLogic.instance.AudioScript.PlayGoldPickupSound();
                    Destroy(GoldCoins[i]);
                    GoldCoins.RemoveAt(i);
                }
                break;
            }
            else
            {
                GoldCanvas.SetActive(false);
            }
        }

        for (int i = 0; i < Treasures.Count; i++)
        {
            dirFromAtoB = (Treasures[i].transform.position - Player.transform.position).normalized;
            dotProd = Vector3.Dot(dirFromAtoB, Player.transform.forward);

            if (Vector3.Distance(Player.transform.position, Treasures[i].transform.position) < 2.3 && dotProd > 0.25)
            {
                TreasureCanvas.SetActive(true);
                if (ThirdPersonController.instance._input.interact)
                {

                    if (Inventory.GetComponent<Inventory>().items.Count < 6)
                    {
                        ThirdPersonController.instance._input.interact = false;
                        TreasureCanvas.SetActive(false);

                        if (Treasures[i].name.Equals("Diamond"))
                        {
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[25]);
                            ItemPickUp(Treasures[i].name);
                        }
                        else if (Treasures[i].name.Equals("Ruby"))
                        {
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[26]);
                            ItemPickUp(Treasures[i].name);
                        }
                        else if (Treasures[i].name.Equals("Gold Bars"))
                        {
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[27]);
                            ItemPickUp(Treasures[i].name);
                        }
                        else if (Treasures[i].name.Equals("Emerald"))
                        {
                            Inventory.GetComponent<Inventory>().AddToInventory(Inventory.GetComponent<Inventory>().scriptableObjects[17]);
                            ItemPickUp(Treasures[i].name);
                        }

                        GameLogic.instance.AudioScript.PlayTreasurePickupSound();
                        Destroy(Treasures[i]);
                        Treasures.RemoveAt(i);
                    }
                    else
                    {
                        AudioScript.PlayErrorSound();
                    }
                }
                break;
            }
            else
            {
                TreasureCanvas.SetActive(false);
            }
        }
        ThirdPersonController.instance._input.interact = false;
    }

    private void ResetItemPickUp()
    {
        HeartKeyPanel.SetActive(false);
        SpadeKeyPanel.SetActive(false);
        DiamondKeyPanel.SetActive(false);
        ClubKeyPanel.SetActive(false);
        OldKeyPanel.SetActive(false);
        KeycardPanel.SetActive(false);
        DiamondPanel.SetActive(false);
        RubyPanel.SetActive(false);
        EmeraldPanel.SetActive(false);
        GoldbarsPanel.SetActive(false);
        MagnumPanel.SetActive(false);
    }

    private void ItemPickUp(string item)
    {
        ItemPickupPanel.SetActive(true);
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(ItemPickupPanel);
        if (item.Equals("Heart Key"))
        {
            ResetItemPickUp();
            HeartKeyPanel.SetActive(true);
        }
        else if (item.Equals("Spade Key"))
        {
            ResetItemPickUp();
            SpadeKeyPanel.SetActive(true);
        }
        else if (item.Equals("Diamond Key"))
        {
            ResetItemPickUp();
            DiamondKeyPanel.SetActive(true);
        }
        else if (item.Equals("Club Key"))
        {
            ResetItemPickUp();
            ClubKeyPanel.SetActive(true);
        }
        else if (item.Equals("Keycard"))
        {
            ResetItemPickUp();
            KeycardPanel.SetActive(true);
        }
        else if (item.Equals("Ruby"))
        {
            ResetItemPickUp();
            RubyPanel.SetActive(true);
        }
        else if (item.Equals("Gold Bars"))
        {
            ResetItemPickUp();
            GoldbarsPanel.SetActive(true);
        }
        else if (item.Equals("Emerald"))
        {
            ResetItemPickUp();
            EmeraldPanel.SetActive(true);
        }
        else if (item.Equals("Diamond"))
        {
            ResetItemPickUp();
            DiamondPanel.SetActive(true);
        }
        else if (item.Equals("Old Key"))
        {
            ResetItemPickUp();
            OldKeyPanel.SetActive(true);
        }
        else if (item.Equals("Magnum"))
        {
            ResetItemPickUp();
            MagnumPanel.SetActive(true);
        }
    }

    private void YouWon()
    {
        Credits.SetActive(true);
        AudioScript.PlayCreditsMusic();
        KeyCanvas.SetActive(false);
        youWon = true;
        ThirdPersonController.instance.HealthUI.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(CreditsFirst);
        ThirdPersonController.instance.enabled = false;
    }

    public void LeonJustDied()
    {
        CloseStore();
        ThirdPersonController.instance.HealthUI.gameObject.SetActive(true);
        UnalertGreenhouseEnemies();
        UnalertZeroZeroThreeEnimes();
        UnalertOneZeroTwoEnemies();
        UnalertSpadeEnemies();
        UnalertHeartEnemies();
        AudioScript.PlayDeathMusic();
        Jack.GetComponent<Jack>().isAlert = false;
        Jack.GetComponent<Jack>().EnemyAnimator.SetBool("isAlert", false);
        Jack.GetComponent<Jack>().EnemyAnimator.SetFloat("speed", 0);
    }

    public void GameOver()
    {
        DeathScreen.SetActive(true);
        ThirdPersonController.instance.HealthUI.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(DeathScreenFirst);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
    }

    public void OpenStore()
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0.5f;
        StoreCamera.Priority = 3;
        AudioScript.OpenStore();
        Store.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        ThirdPersonController.instance.enabled = false;

        EventSystem.current.SetSelectedGameObject(StoreFirst);

        ThirdPersonController.instance.HealthUI.gameObject.SetActive(false);
    }

    public void CloseStore()
    {
        StoreCamera.Priority = 0;
        AudioScript.CloseStore();
        MerchantImage.SetActive(false);

        Store.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ThirdPersonController.instance.enabled = true;

        timeSinceBlend = 0.5f;

        ThirdPersonController.instance.HealthUI.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isFading = false;
        fader = 0;
        PauseMenuCanvas.DOFade(1, 0.35f);
        OptionsCanvas.DOFade(0, 0.35f);
        PauseButtons.SetActive(true);
        PauseMenu.SetActive(false);
        OptionButtons.SetActive(false);
        AudioScript.Unpause();
        AudioScript.PlayClickSound();
        ItemPickupPanel.SetActive(false);
        InventoryUI.SetActive(false);

        ThirdPersonController.instance.enabled = true;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Main Level Omar");
        AudioScript.PlayClickSound();
    }

    public void Back()
    {
        isFading = true;
        fader = 2;
        OptionsCanvas.DOFade(0, 0.25f).SetUpdate(true);
        AudioScript.PlayClickSound();
        EventSystem.current.SetSelectedGameObject(PauseMenuFirst);
    }

    private void PauseMenuFading()
    {
        if (isFading)
        {
            fadeTimeElapsed += Time.unscaledDeltaTime;
        }
        if (fadeTimeElapsed > 0.25f && isFading)
        {
            if (fader == 1)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                OptionButtons.SetActive(true);
                PauseButtons.SetActive(false);
                OptionsCanvas.DOFade(1, 0.35f).SetUpdate(true);
            }
            else if (fader == 2)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                OptionButtons.SetActive(false);
                PauseButtons.SetActive(true);
                PauseMenuCanvas.DOFade(1, 0.35f).SetUpdate(true);
            }
            ContinueAnimator.Play("Unhover Animation");
            RestartAnimator.Play("Unhover Animation");
            OptionsAnimator.Play("Unhover Animation");
            ExitAnimator.Play("Unhover Animation");
        }
    }

    public void Options()
    {
        isFading = true;
        fader = 1;
        ContinueAnimator.Play("Hover Animation");
        RestartAnimator.Play("Hover Animation");
        OptionsAnimator.Play("Hover Animation");
        ExitAnimator.Play("Hover Animation");
        PauseMenuCanvas.DOFade(0, 0.25f).SetUpdate(true);
        AudioScript.PlayClickSound();
        EventSystem.current.SetSelectedGameObject(OptionsMenuFirst);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Main Menu");
        AudioScript.PlayClickSound();
    }

    public void CheckEnemiesDead()
    {
        for (int i = 0; i < GreenhouseEnemies.Length; i++)
        {
            if (!GreenhouseEnemies[i].GetComponent<Enemy>().isDead)
                break;
            else if (i == GreenhouseEnemies.Length - 1 && !GreenhouseEnemiesEliminated)
            {
                GreenhouseEnemiesEliminated = true;
                AudioScript.LeaveEnemeyArea();
            }
        }

        for (int i = 0; i < ZeroZeroThreeEnemies.Length; i++)
        {
            if (!ZeroZeroThreeEnemies[i].GetComponent<Enemy>().isDead)
                break;
            else if (i == ZeroZeroThreeEnemies.Length - 1 && !ZeroZeroThreeEnemiesEliminated)
            {
                ZeroZeroThreeEnemiesEliminated = true;
                AudioScript.LeaveEnemeyArea();
            }
        }

        for (int i = 0; i < OneZeroTwoEnemies.Length; i++)
        {
            if (!OneZeroTwoEnemies[i].GetComponent<Enemy>().isDead)
                break;
            else if (i == OneZeroTwoEnemies.Length - 1 && !OneZeroTwoEnemiesEliminated)
            {
                OneZeroTwoEnemiesEliminated = true;
                AudioScript.EnterSuspenseArea();
            }
        }

        for (int i = 0; i < SpadeEnemies.Length; i++)
        {
            if (!SpadeEnemies[i].GetComponent<Enemy>().isDead)
                break;
            else if (i == SpadeEnemies.Length - 1 && !SpadeEnemiesEliminated)
            {
                SpadeEnemiesEliminated = true;
                AudioScript.EnterSuspenseArea();
            }
        }

        for (int i = 0; i < HeartEnemies.Length; i++)
        {
            if (!HeartEnemies[i].GetComponent<Enemy>().isDead)
                break;
            else if (i == HeartEnemies.Length - 1 && !HeartEnemiesEliminated)
            {
                HeartEnemiesEliminated = true;
                AudioScript.EnterSuspenseArea();
            }
        }
    }

    public void AlertGreenhouseEnemies()
    {
        foreach (GameObject enemy in GreenhouseEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = true;
                enemy.GetComponent<Animator>().SetBool("isAlert", true);
                enemy.GetComponent<Animator>().SetBool("isIdle", false);

                if (enemy.GetComponent<Enemy>().isArmed)
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
            }
        }
    }

    public void UnalertGreenhouseEnemies()
    {
        foreach (GameObject enemy in GreenhouseEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = false;
                enemy.GetComponent<Enemy>().isIdle = false;
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isIdle", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isAlert", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetInteger("random", Random.Range(0, 2));
                if (enemy.GetComponent<Enemy>().isArmed)
                {
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 0f);
                }
                else
                {
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 1.5f);
                }
                enemy.GetComponent<Enemy>().agent.destination = enemy.GetComponent<Enemy>().IdleTarget1.position;
            }
        }
    }

    public void AlertOneZeroTwoEnemies()
    {

        foreach (GameObject enemy in OneZeroTwoEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = true;
                enemy.GetComponent<Animator>().SetBool("isAlert", true);
                enemy.GetComponent<Animator>().SetBool("isIdle", false);

                if (enemy.GetComponent<Enemy>().isArmed)
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
            }
        }
    }

    public void UnalertOneZeroTwoEnemies()
    {
        foreach (GameObject enemy in OneZeroTwoEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = false;
                enemy.GetComponent<Enemy>().isIdle = false;
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isIdle", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isAlert", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetInteger("random", Random.Range(0, 2));
                if (enemy.GetComponent<Enemy>().isArmed)
                {
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 0f);
                }
                else
                {
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 1.5f);
                }
                enemy.GetComponent<Enemy>().agent.destination = enemy.GetComponent<Enemy>().IdleTarget1.position;
            }
        }
    }

    public void AlertSpadeEnemies()
    {
        foreach (GameObject enemy in SpadeEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = true;
                enemy.GetComponent<Animator>().SetBool("isAlert", true);
                enemy.GetComponent<Animator>().SetBool("isIdle", false);

                if (enemy.GetComponent<Enemy>().isArmed)
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
            }
        }
    }

    public void UnalertSpadeEnemies()
    {
        foreach (GameObject enemy in SpadeEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = false;
                enemy.GetComponent<Enemy>().isIdle = false;
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isIdle", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isAlert", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetInteger("random", Random.Range(0, 2));
                if (enemy.GetComponent<Enemy>().isArmed)
                {
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 0f);
                }
                else
                {
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 1.5f);
                }
                enemy.GetComponent<Enemy>().agent.destination = enemy.GetComponent<Enemy>().IdleTarget1.position;
            }
        }
    }

    public void AlertHeartEnemies()
    {
        foreach (GameObject enemy in HeartEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = true;
                enemy.GetComponent<Animator>().SetBool("isAlert", true);
                enemy.GetComponent<Animator>().SetBool("isIdle", false);

                if (enemy.GetComponent<Enemy>().isArmed)
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
            }
        }
    }

    public void UnalertHeartEnemies()
    {
        foreach (GameObject enemy in HeartEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = false;
                enemy.GetComponent<Enemy>().isIdle = false;
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isIdle", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isAlert", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetInteger("random", Random.Range(0, 2));
                if (enemy.GetComponent<Enemy>().isArmed)
                {
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 0f);
                }
                else
                {
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 1.5f);
                }
                enemy.GetComponent<Enemy>().agent.destination = enemy.GetComponent<Enemy>().IdleTarget1.position;
            }
        }
    }

    public void AlertZeroZeroThreeEnimes()
    {
        foreach (GameObject enemy in ZeroZeroThreeEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = true;
                enemy.GetComponent<Animator>().SetBool("isAlert", true);
                enemy.GetComponent<Animator>().SetBool("isIdle", false);

                if (enemy.GetComponent<Enemy>().isArmed)
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
            }
        }
    }

    public void UnalertZeroZeroThreeEnimes()
    {
        foreach (GameObject enemy in ZeroZeroThreeEnemies)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                enemy.GetComponent<Enemy>().isAlert = false;
                enemy.GetComponent<Enemy>().isIdle = false;
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isIdle", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetBool("isAlert", false);
                enemy.GetComponent<Enemy>().EnemyAnimator.SetInteger("random", Random.Range(0, 2));

                if (enemy.GetComponent<Enemy>().isArmed)
                {
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 0f);
                    enemy.GetComponent<Animator>().SetBool("isEquipping", true);
                }
                else
                {
                    enemy.GetComponent<Enemy>().EnemyAnimator.SetFloat("speed", 1.5f);
                }

                enemy.GetComponent<Enemy>().agent.destination = enemy.GetComponent<Enemy>().IdleTarget1.position;
            }
        }
    }
}
