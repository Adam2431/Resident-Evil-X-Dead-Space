using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StarterAssets;
using System;

public class Inventory : MonoBehaviour
{
    public InventoryItem[] scriptableObjects;

    const int maxInventoryCapacity = 6;

    public List<InventoryItem> items;
    public List<InventoryItem> storageItems;

    public InventoryItem equippedWeapon;
    public InventoryItem equippedGrenade;
    public Image weapon;
    public Image grenade;

    [SerializeField] GameLogic GameLogic;

    Gold gold;
    public GameObject goldManager;

    public Button[] slots;
    public Button[] sellSlots;
    public Button[] storageInventorySlots;
    public Button[] storageSlots;

    public GameObject[] options;

    int storageIndex = 0;

    int combineItems = 2;
    public Image combineOption1;
    public Image combineOption2;
    string item1;
    string item2;

    public int maxPistolAmmo = 30;
    public int maxRifleAmmo = 100;
    public int maxShotgunAmmo = 20;
    public int maxMagnumAmmo = 10;

    void Start()
    {
        items = new List<InventoryItem>();
        storageItems = new List<InventoryItem>();

        gold = goldManager.GetComponent<Gold>();

        AddToInventory(scriptableObjects[2]);
        AddToInventory(scriptableObjects[10]);
        equippedWeapon = scriptableObjects[2];
    }

    public int GetAmmoNumberInventory(int slotNumber)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == slotNumber)
            {
                if (slotNumber == 0)
                {
                    return Int32.Parse(GameLogic.instance.TopLeftAmmo.text);
                }
                else if (slotNumber == 1)
                {
                    return Int32.Parse(GameLogic.instance.TopMiddleAmmo.text);
                }
                else if (slotNumber == 2)
                {
                    return Int32.Parse(GameLogic.instance.TopRightAmmo.text);
                }
                else if (slotNumber == 3)
                {
                    return Int32.Parse(GameLogic.instance.BottomLeftAmmo.text);
                }
                else if (slotNumber == 4)
                {
                    return Int32.Parse(GameLogic.instance.BottomMiddleAmmo.text);
                }
                else if (slotNumber == 5)
                {
                    return Int32.Parse(GameLogic.instance.BottomRightAmmo.text);
                }
            }
        }
        return 0;
    }
    public void UpdateSellSlots()
    {
        int j = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (!(items[i].itemName == "Diamond Key" || items[i].itemName == "Spade Key" || items[i].itemName == "Club Key" ||
            items[i].itemName == "Heart Key" || items[i].itemName == "Keycard"))
            {
                sellSlots[j].gameObject.SetActive(true);

                Image img = sellSlots[j].transform.Find("png").GetComponent<Image>();
                img.sprite = items[i].sprite;
                Text text = sellSlots[j].transform.Find("text").GetComponentInChildren<Text>();
                text.text = items[i].itemName;
                Text price = sellSlots[j].transform.Find("price").GetComponentInChildren<Text>();
                price.text = GetSellPrice(items[i]).ToString();
                j++;
            }
        }
        while (j < sellSlots.Length)
        {
            sellSlots[j].gameObject.SetActive(false);
            j++;
        }
    }

    public void SlotSelection(GameObject slotOptions)
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].SetActive(false);
        }

        string name = EventSystem.current.currentSelectedGameObject.name;
        if (name == "Assault Rifle" || name == "Shotgun" || name == "Pistol" || name == "Magnum" ||
            name == "Hand Grenade" || name == "Flash Grenade")
        {
            slotOptions.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }

        else if (name == "Green Herb" || name == "Red Herb")
        {
            slotOptions.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }

        else if (name == "G+G Mixture" || name == "G+R Mixture" || name == "R+R Mixture")
        {
            slotOptions.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }

        else if (name == "Emerald" || name == "Ruby" || name == "Diamond" || name == "Gold Bars" || name == "AR Ammo" || name == "Shotgun Ammo" ||
                name == "Pistol Ammo" || name == "Revolver Ammo")
        {
            slotOptions.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }

        else if (name == "Normal Gunpowder" || name == "High-grade Gunpowder")
        {
            slotOptions.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            slotOptions.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            slotOptions.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }
    }

    public void Equip(Button item)
    {
        InventoryItem so = null;
        for (int i = 0; i < scriptableObjects.Length; i++)
        {
            if (item.name == scriptableObjects[i].itemName)
            {
                so = scriptableObjects[i];
            }
        }

        if (item.name == "Hand Grenade" || item.name == "Flash Grenade")
        {
            equippedGrenade = so;
            grenade.gameObject.SetActive(true);
            grenade.sprite = so.sprite;
        }

        if (item.name == "Assault Rifle" || item.name == "Shotgun" || item.name == "Pistol" || item.name == "Revolver")
        {
            equippedWeapon = so;
            weapon.gameObject.SetActive(true);
            weapon.sprite = so.sprite;
        }
        GameLogic.instance.currentWeapons.ForEach(weapon =>
        {
            if (item.name == weapon)
            {
                ThirdPersonController.instance.EquipWeapon(weapon);
            }
        });
        GameObject button = EventSystem.current.currentSelectedGameObject;
        button.transform.parent.parent.gameObject.SetActive(false);
    }

    public void UnequipWeapon(Image unequip)
    {
        equippedWeapon = null;
        unequip.gameObject.SetActive(false);
    }

    public void UnequipGrenade(Image unequip)
    {
        equippedGrenade = null;
        unequip.gameObject.SetActive(false);
    }

    public void Use(Button item)
    {
        if (item.name == "Green Herb")
        {
            ThirdPersonController.instance.health += 2;
            if (ThirdPersonController.instance.health > 8)
            {
                ThirdPersonController.instance.health = 8;
            }
            GameLogic.instance.AudioScript.PlayUseItemSound();

            ThirdPersonController.instance.HealthUI.fillAmount = (float)ThirdPersonController.instance.health / 8;
            ThirdPersonController.instance.HealthUI.color = Color.Lerp(ThirdPersonController.instance.deadHealthColor, ThirdPersonController.instance.fullHealthColor, (float)ThirdPersonController.instance.health / 8);
        }
        else if (item.name == "G+R Mixture")
        {
            ThirdPersonController.instance.health = 8;
            GameLogic.instance.AudioScript.PlayUseItemSound();

            ThirdPersonController.instance.HealthUI.fillAmount = (float)ThirdPersonController.instance.health / 8;
            ThirdPersonController.instance.HealthUI.color = Color.Lerp(ThirdPersonController.instance.deadHealthColor, ThirdPersonController.instance.fullHealthColor, (float)ThirdPersonController.instance.health / 8);
        }
        Discard(item);
    }

    public void Discard(Button item)
    {
        InventoryItem so = null;
        for (int i = 0; i < scriptableObjects.Length; i++)
        {
            if (item.name == scriptableObjects[i].itemName)
            {
                so = scriptableObjects[i];
            }
        }

        if (item.name == "Pistol" || item.name == "Assault Rifle" || item.name == "Shotgun" || item.name == "Magnum")
        {
            if (GameLogic.instance.conatinsOneWeapon())
            {
                GameLogic.instance.AudioScript.PlayErrorSound();
                return;
            }
        }

        if (item.name == "Pistol" || item.name == "Assault Rifle" || item.name == "Shotgun" || item.name == "Magnum" || item.name == "Hand Grenade" || item.name == "Flash Grenade")
        {
            int oldWeaponIndex = ThirdPersonController.instance._input.weaponIndex;

            int weaponRemovedIndex = GameLogic.instance.currentWeapons.IndexOf(item.name);
            GameLogic.instance.currentWeapons.Remove(item.name);

            if (ThirdPersonController.instance._input.weaponIndex > 0 && weaponRemovedIndex <= oldWeaponIndex)
                ThirdPersonController.instance._input.weaponIndex--;

            if (weaponRemovedIndex == oldWeaponIndex)
            {
                ThirdPersonController.instance.GetComponent<Animator>().SetBool("isPuttingAway", true);
            }
        }

        if (item.name == "Pistol Ammo")
        {
            GameLogic.instance.pistolAmmo -= GetAmmoNumberInventory(items.IndexOf(so));
        }
        else if (item.name == "AR Ammo")
        {
            GameLogic.instance.rifleAmmo -= GetAmmoNumberInventory(items.IndexOf(so));
        }
        else if (item.name == "Shotgun Ammo")
        {
            GameLogic.instance.shotgunAmmo -= GetAmmoNumberInventory(items.IndexOf(so));
        }
        else if (item.name == "Magnum Ammo")
        {
            GameLogic.instance.magnumAmmo -= GetAmmoNumberInventory(items.IndexOf(so));
        }

        RemoveFromInventory(so);
        GameObject button = EventSystem.current.currentSelectedGameObject;
        button.transform.parent.parent.gameObject.SetActive(false);
        GameLogic.instance.UpdateAmmo();
    }

    public void CombineOptions(Button item)
    {
        InventoryItem so = null;
        for (int i = 0; i < scriptableObjects.Length; i++)
        {
            if (item.name == scriptableObjects[i].itemName)
            {
                so = scriptableObjects[i];
            }
        }

        if (combineItems == 2)
        {
            item1 = item.name;
            combineOption1.gameObject.SetActive(true);
            combineOption1.sprite = so.sprite;
            combineItems--;
        }
        else if (combineItems == 1)
        {
            item2 = item.name;
            combineOption2.gameObject.SetActive(true);
            combineOption2.sprite = so.sprite;
            combineItems--;
        }
    }

    public void RemoveCombineOptions()
    {
        if (combineItems == 1)
        {
            combineOption1.gameObject.SetActive(false);
        }
        else if (combineItems == 0)
        {
            combineOption2.gameObject.SetActive(false);
        }
        combineItems++;
    }

    public void Combine()
    {
        if (combineItems == 0)
        {
            InventoryItem combineItem1 = null;
            InventoryItem combineItem2 = null;
            for (int i = 0; i < scriptableObjects.Length; i++)
            {
                if (item1 == scriptableObjects[i].itemName)
                {
                    combineItem1 = scriptableObjects[i];
                }
                if (item2 == scriptableObjects[i].itemName)
                {
                    combineItem2 = scriptableObjects[i];
                }
            }

            InventoryItem newItem = null;
            if (item1 == "Green Herb" && item2 == "Green Herb")
            {
                newItem = scriptableObjects[12];
            }
            else if ((item1 == "Green Herb" && item2 == "Red Herb") || (item1 == "Red Herb" && item2 == "Green Herb"))
            {
                newItem = scriptableObjects[13];
            }
            else if (item1 == "Red Herb" && item2 == "Red Herb")
            {
                newItem = scriptableObjects[14];
            }
            else if (item1 == "Normal Gunpowder" && item2 == "Normal Gunpowder")
            {
                newItem = scriptableObjects[8];
            }
            else if ((item1 == "Normal Gunpowder" && item2 == "High-grade Gunpowder") || (item2 == "Normal Gunpowder" && item1 == "High-grade Gunpowder"))
            {
                newItem = scriptableObjects[7];
            }
            else if (item1 == "High-grade Gunpowder" && item2 == "High-grade Gunpowder")
            {
                newItem = scriptableObjects[6];
            }

            RemoveFromInventory(combineItem1);
            RemoveFromInventory(combineItem2);
            AddToInventory(newItem);
            GameLogic.instance.UpdateAmmo();

            combineOption1.gameObject.SetActive(false);
            combineOption2.gameObject.SetActive(false);
            combineItems = 2;

            for (int i = 0; i < options.Length; i++)
            {
                options[i].SetActive(false);
            }
        }
    }

    public void AddToInventory(InventoryItem item)
    {
        items.Add(item);

        int i = items.Count - 1;

        slots[i].name = item.itemName;
        slots[i].image.sprite = items[i].sprite;
        slots[i].gameObject.SetActive(true);

        storageInventorySlots[i].name = item.itemName;
        storageInventorySlots[i].image.sprite = items[i].sprite;
        storageInventorySlots[i].gameObject.SetActive(true);

        UpdateSellSlots();
    }

    public void RemoveFromInventory(InventoryItem item)
    {
        items.Remove(item);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
            sellSlots[i].gameObject.SetActive(false);
            storageInventorySlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Count; i++)
        {
            slots[i].name = items[i].itemName;
            slots[i].image.sprite = items[i].sprite;
            slots[i].gameObject.SetActive(true);

            storageInventorySlots[i].name = items[i].itemName;
            storageInventorySlots[i].image.sprite = items[i].sprite;
            storageInventorySlots[i].gameObject.SetActive(true);

            if (!(items[i].itemName == "Assault Rifle" || items[i].itemName == "Shotgun" || items[i].itemName == "Pistol" ||
                items[i].itemName == "Revolver" || items[i].itemName == "AR Ammo" || items[i].itemName == "Shotgun Ammo" ||
                items[i].itemName == "Pistol Ammo" || items[i].itemName == "Revolver Ammo"))
            {
                sellSlots[i].gameObject.SetActive(true);
                Image img = sellSlots[i].transform.Find("png").GetComponent<Image>();
                img.sprite = items[i].sprite;
                Text text = sellSlots[i].transform.Find("text").GetComponentInChildren<Text>();
                text.text = items[i].itemName;
                Text price = sellSlots[i].transform.Find("price").GetComponentInChildren<Text>();
                price.text = GetSellPrice(item).ToString();
            }
        }

        UpdateSellSlots();
    }

    public void MoveFromStorage(Button slot)
    {
        if (items.Count == maxInventoryCapacity)
        {
            return;
        }
        InventoryItem item = null;
        for (int i = 0; i < storageItems.Count; i++)
        {
            if (storageItems[i].itemName == slot.name)
            {
                item = storageItems[i];
                break;
            }
        }

        float numOfPistolAmmo = Mathf.Ceil((float)GameLogic.instance.storagePistolAmmo / maxPistolAmmo);
        float numOfRifleAmmo = Mathf.Ceil((float)GameLogic.instance.storageRifleAmmo / maxRifleAmmo);
        float numOfShotgunAmmo = Mathf.Ceil((float)GameLogic.instance.storageShotgunAmmo / maxShotgunAmmo);
        float numOfMagnumAmmo = Mathf.Ceil((float)GameLogic.instance.storageMagnumAmmo / maxMagnumAmmo);

        if (item.itemName == "Pistol Ammo" && numOfPistolAmmo > 1)
        {
            GameLogic.instance.pistolAmmo += maxPistolAmmo;
            GameLogic.instance.storagePistolAmmo -= maxPistolAmmo;
        }
        else if (item.itemName == "Pistol Ammo" && numOfPistolAmmo == 1)
        {

            GameLogic.instance.pistolAmmo += GameLogic.instance.storagePistolAmmo;
            GameLogic.instance.storagePistolAmmo = 0;
        }

        if (item.itemName == "AR Ammo" && numOfRifleAmmo > 1)
        {
            GameLogic.instance.rifleAmmo += maxRifleAmmo;
            GameLogic.instance.storageRifleAmmo -= maxRifleAmmo;
        }
        else if (item.itemName == "AR Ammo" && numOfRifleAmmo == 1)
        {
            GameLogic.instance.rifleAmmo += GameLogic.instance.storageRifleAmmo;
            GameLogic.instance.storageRifleAmmo = 0;
        }

        if (item.itemName == "Shotgun Ammo" && numOfShotgunAmmo > 1)
        {
            GameLogic.instance.shotgunAmmo += maxShotgunAmmo;
            GameLogic.instance.storageShotgunAmmo -= maxShotgunAmmo;
        }
        else if (item.itemName == "Shotgun Ammo" && numOfShotgunAmmo == 1)
        {
            GameLogic.instance.shotgunAmmo += GameLogic.instance.storageShotgunAmmo;
            GameLogic.instance.storageShotgunAmmo = 0;
        }

        if (item.itemName == "Magnum Ammo" && numOfMagnumAmmo > 1)
        {
            GameLogic.instance.magnumAmmo += maxMagnumAmmo;
            GameLogic.instance.storageMagnumAmmo -= maxMagnumAmmo;
        }
        else if (item.itemName == "Magnum Ammo" && numOfMagnumAmmo == 1)
        {
            GameLogic.instance.magnumAmmo += GameLogic.instance.storageMagnumAmmo;
            GameLogic.instance.storageMagnumAmmo = 0;
        }

        storageItems.Remove(item);
        storageIndex--;
        storageSlots[storageIndex].gameObject.SetActive(false);
        AddToInventory(item);

        if (item.name == "Pistol" || item.name == "Assault Rifle" || item.name == "Shotgun" || item.name == "Hand Grenade" || item.name == "Flash Grenade" || item.name == "Magnum")
        {
            GameLogic.instance.currentWeapons.Add(item.name);
        }

        for (int i = 0; i < storageItems.Count; i++)
        {
            storageSlots[i].name = storageItems[i].itemName;
            storageSlots[i].image.sprite = storageItems[i].sprite;
            storageSlots[i].gameObject.SetActive(true);
        }
        GameLogic.instance.UpdateAmmo();

        UpdateSellSlots();
    }

    public void MoveToStorage(Button slot)
    {
        if (slot.name == "Pistol" || slot.name == "Assault Rifle" || slot.name == "Shotgun" || slot.name == "Magnum")
        {
            if (GameLogic.instance.conatinsOneWeapon())
            {
                GameLogic.instance.AudioScript.PlayErrorSound();
                return;
            }
        }
        InventoryItem item = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == slot.name)
            {
                item = items[i];
                break;
            }
        }
        if (item.name == "Pistol" || item.name == "Assault Rifle" || item.name == "Shotgun" || item.name == "Hand Grenade" || item.name == "Flash Grenade" || item.name == "Magnum")
        {
            int oldWeaponIndex = ThirdPersonController.instance._input.weaponIndex;
            GameLogic.instance.currentWeapons.Remove(item.name);
            if (oldWeaponIndex > GameLogic.instance.currentWeapons.Count - 1)
            {
                ThirdPersonController.instance._input.weaponIndex = GameLogic.instance.currentWeapons.Count - 1;
                ThirdPersonController.instance.GetComponent<Animator>().SetBool("isPuttingAway", true);
            }
            else if (oldWeaponIndex == 0)
            {
                ThirdPersonController.instance.GetComponent<Animator>().SetBool("isPuttingAway", true);
            }
        }

        if (item.itemName == "Pistol Ammo")
        {
            int changeInAmmo = (int)(GameLogic.instance.pistolAmmo - Mathf.Floor((float)GameLogic.instance.pistolAmmo / maxPistolAmmo) * maxPistolAmmo);
            if (changeInAmmo == 0)
                changeInAmmo = GameLogic.instance.pistolAmmo;
            GameLogic.instance.pistolAmmo -= changeInAmmo;
            GameLogic.instance.storagePistolAmmo += changeInAmmo;
        }

        else if (item.itemName == "AR Ammo")
        {
            int changeInAmmo = (int)(GameLogic.instance.rifleAmmo - Mathf.Floor((float)GameLogic.instance.rifleAmmo / maxRifleAmmo) * maxRifleAmmo);
            if (changeInAmmo == 0)
                changeInAmmo = GameLogic.instance.rifleAmmo;
            GameLogic.instance.rifleAmmo -= changeInAmmo;
            GameLogic.instance.storageRifleAmmo += changeInAmmo;
        }

        else if (item.itemName == "Shotgun Ammo")
        {
            int changeInAmmo = (int)(GameLogic.instance.shotgunAmmo - Mathf.Floor((float)GameLogic.instance.shotgunAmmo / maxShotgunAmmo) * maxShotgunAmmo);
            if (changeInAmmo == 0)
                changeInAmmo = GameLogic.instance.shotgunAmmo;
            GameLogic.instance.shotgunAmmo -= changeInAmmo;
            GameLogic.instance.storageShotgunAmmo += changeInAmmo;
        }

        else if (item.itemName == "Magnum Ammo")
        {
            int changeInAmmo = (int)(GameLogic.instance.magnumAmmo - Mathf.Floor((float)GameLogic.instance.magnumAmmo / maxMagnumAmmo) * maxMagnumAmmo);
            if (changeInAmmo == 0)
                changeInAmmo = GameLogic.instance.magnumAmmo;
            GameLogic.instance.magnumAmmo -= changeInAmmo;
            GameLogic.instance.storageMagnumAmmo += changeInAmmo;
        }

        storageItems.Add(item);
        storageSlots[storageIndex].name = item.itemName;
        storageSlots[storageIndex].image.sprite = item.sprite;
        storageSlots[storageIndex].gameObject.SetActive(true);
        RemoveFromInventory(item);

        storageIndex++;
        GameLogic.instance.UpdateAmmo();
    }

    public void Buy(InventoryItem item)
    {
        int price = GetBuyPrice(item);

        if (gold.total < price)
        {
            GameLogic.instance.AudioScript.PlayErrorSound();
            return;
        }

        if (item.itemName == "Pistol Ammo")
        {
            float oldNumOfPistolAmmo = Mathf.Ceil((float)GameLogic.instance.pistolAmmo / maxPistolAmmo);
            float newNumOfPistolAmmo = Mathf.Ceil((float)(GameLogic.instance.pistolAmmo + 10) / maxPistolAmmo);
            if (newNumOfPistolAmmo - oldNumOfPistolAmmo > 0 && items.Count >= 6)
            {
                GameLogic.instance.AudioScript.PlayErrorSound();
                return;
            }
            GameLogic.instance.pistolAmmo += 10;
            GameLogic.instance.UpdateAmmo();
            GameLogic.instance.AudioScript.PlayBuySound();
            gold.SpendGold(price);
            return;
        }
        else if (item.itemName == "AR Ammo")
        {
            float oldNumOfRifleAmmo = Mathf.Ceil((float)GameLogic.instance.rifleAmmo / maxRifleAmmo);
            float newNumOfRifleAmmo = Mathf.Ceil((float)(GameLogic.instance.rifleAmmo + 30) / maxRifleAmmo);

            if (newNumOfRifleAmmo - oldNumOfRifleAmmo > 0 && items.Count >= 6)
            {
                GameLogic.instance.AudioScript.PlayErrorSound();
                return;
            }

            GameLogic.instance.rifleAmmo += 30;
            GameLogic.instance.UpdateAmmo();
            GameLogic.instance.AudioScript.PlayBuySound();
            gold.SpendGold(price);
            return;
        }
        else if (item.itemName == "Shotgun Ammo")
        {
            float oldNumOfShotgunAmmo = Mathf.Ceil((float)GameLogic.instance.shotgunAmmo / maxShotgunAmmo);
            float newNumOfShotgunAmmo = Mathf.Ceil((float)(GameLogic.instance.shotgunAmmo + 5) / maxShotgunAmmo);

            if (newNumOfShotgunAmmo - oldNumOfShotgunAmmo > 0 && items.Count >= 6)
            {
                GameLogic.instance.AudioScript.PlayErrorSound();
                return;
            }

            GameLogic.instance.shotgunAmmo += 5;
            GameLogic.instance.UpdateAmmo();
            GameLogic.instance.AudioScript.PlayBuySound();
            gold.SpendGold(price);
            return;
        }
        else if (item.itemName == "Magnum Ammo")
        {
            float oldNumOfMagnumAmmo = Mathf.Ceil((float)GameLogic.instance.magnumAmmo / maxMagnumAmmo);
            float newNumOfMagnumAmmo = Mathf.Ceil((float)(GameLogic.instance.magnumAmmo + 3) / maxMagnumAmmo);

            if (newNumOfMagnumAmmo - oldNumOfMagnumAmmo > 0 && items.Count >= 6)
            {
                GameLogic.instance.AudioScript.PlayErrorSound();
                return;
            }

            GameLogic.instance.magnumAmmo += 3;
            GameLogic.instance.UpdateAmmo();
            GameLogic.instance.AudioScript.PlayBuySound();
            gold.SpendGold(price);
            return;
        }

        if (items.Count == maxInventoryCapacity)
        {
            GameLogic.instance.AudioScript.PlayErrorSound();
            return;
        }

        if ((item.itemName == "Assault Rifle" || item.itemName == "Shotgun") && !GameLogic.instance.currentWeapons.Contains(item.itemName) && !storageItems.Contains(item))
        {
            GameLogic.instance.currentWeapons.Add(item.itemName);
        }
        else if (item.itemName == "Hand Grenade" || item.itemName == "Flash Grenade")
        {
            GameLogic.instance.currentWeapons.Add(item.itemName);
        }
        else if (item.itemName == "Assault Rifle" || item.itemName == "Shotgun")
        {
            GameLogic.instance.AudioScript.PlayErrorSound();
            return;
        }

        gold.SpendGold(price);

        GameLogic.instance.AudioScript.PlayBuySound();

        AddToInventory(item);
    }

    public void Sell(Text name)
    {
        if (name.text == "Pistol" || name.text == "Assault Rifle" || name.text == "Shotgun" || name.text == "Magnum")
        {
            if (GameLogic.instance.conatinsOneWeapon())
            {
                GameLogic.instance.AudioScript.PlayErrorSound();
                return;
            }
        }

        InventoryItem item = null;
        int oldWeaponIndex = ThirdPersonController.instance._input.weaponIndex;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == name.text)
            {
                item = items[i];
                if (item.itemName == "Pistol" || item.itemName == "Assault Rifle" || item.itemName == "Shotgun" || item.itemName == "Magnum" || item.itemName == "Hand Grenade" || item.itemName == "Flash Grenade")
                {
                    int weaponRemovedIndex = GameLogic.instance.currentWeapons.IndexOf(item.itemName);
                    GameLogic.instance.currentWeapons.Remove(item.itemName);

                    if (weaponRemovedIndex == oldWeaponIndex)
                    {
                        if (ThirdPersonController.instance._input.weaponIndex > 0)
                        {
                            ThirdPersonController.instance._input.weaponIndex--;
                        }
                        ThirdPersonController.instance.GetComponent<Animator>().SetBool("isPuttingAway", true);
                    }

                    else if (weaponRemovedIndex < oldWeaponIndex)
                    {
                        ThirdPersonController.instance._input.weaponIndex--;
                    }

                    ThirdPersonController.instance.SwitchWeapon();
                }
                break;
            }
        }

        int price = GetSellPrice(item);
        gold.PickupGold(price);
        GameLogic.instance.AudioScript.PlayBuySound();
        RemoveFromInventory(item);
    }

    static int GetBuyPrice(InventoryItem item)
    {
        if (item.itemName == "Green Herb")
        {
            return 20;
        }
        if (item.itemName == "G+R Mixture")
        {
            return 60;
        }

        if (item.itemName == "Normal Gunpowder")
        {
            return 10;
        }

        if (item.itemName == "High-grade Gunpowder")
        {
            return 20;
        }

        if (item.itemName == "Hand Grenade")
        {
            return 40;
        }

        if (item.itemName == "Flash Grenade")
        {
            return 30;
        }

        if (item.itemName == "Assault Rifle")
        {
            return 100;
        }

        if (item.itemName == "Shotgun")
        {
            return 150;
        }
        if (item.itemName == "Pistol Ammo")
        {
            return 30;
        }
        if (item.itemName == "AR Ammo")
        {
            return 60;
        }
        if (item.itemName == "Shotgun Ammo")
        {
            return 50;
        }
        if (item.itemName == "Magnum Ammo")
        {
            return 80;
        }
        return 0;
    }


    static int GetSellPrice(InventoryItem item)
    {
        if (item.itemName == "Magnum")
        {
            return 100;
        }
        else if (item.itemName == "Pistol")
        {
            return 40;
        }
        else if (item.itemName == "G+G Mixture")
        {
            return 25;
        }
        else if (item.itemName == "G+R Mixture")
        {
            return 30;
        }
        else if (item.itemName == "R+R Mixture")
        {
            return 10;
        }
        else if (item.itemName == "Gold bar")
        {
            return 100;
        }
        else if (item.itemName == "Emerald")
        {
            return 60;
        }
        else if (item.itemName == "Diamond")
        {
            return 120;
        }
        else if (item.itemName == "Ruby")
        {
            return 90;
        }
        else if (item.itemName == "Gold Bars")
        {
            return 100;
        }
        return GetBuyPrice(item) / 2;
    }

}
