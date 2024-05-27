using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image[] bars;
    public float health;
    public int maxHealth;
    float lerpSpeed;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        health = 8;
        maxHealth = 8;
        lerpSpeed = 0.5f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        health = player.GetComponent<ThirdPersonController>().health;
        HealthBarFiller();
    }

    public void DecreaseHealthBar(int healthPoints)
    {
        health -= healthPoints;
    }

    public void IncreaseHealthBar(int healthPoints)
    {
        health += healthPoints;
    }

    public void HealthBarFiller()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            if (i >= health)
            {
                bars[i].enabled = false;
            }
            else
            {
                bars[i].enabled = true;
            }
            bars[i].color = Color.Lerp(Color.red, Color.green, health / maxHealth);
        }
    }
}
