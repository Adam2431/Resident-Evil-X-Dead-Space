using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    public int total;
    public Text goldAmount;
    public Text inventoryGoldAmount;

    void Start()
    {
        goldAmount.text = total.ToString();
        inventoryGoldAmount.text = total.ToString();
    }
    public void PickupGold(int amount)
    {
        total += amount;
        goldAmount.text = total.ToString();
        inventoryGoldAmount.text = total.ToString();
    }

    public void SpendGold(int amount)
    {
        if (amount > total)
        {
            // invalid
            return;
        }
        total -= amount;
        goldAmount.text = total.ToString();
        inventoryGoldAmount.text = total.ToString();
    }
}
