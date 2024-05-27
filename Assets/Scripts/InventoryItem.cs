using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public bool isEquippable;
    public bool isUsable;
    public bool isCraftable;
    public bool isDiscardable;
    public bool isStackable;
}
