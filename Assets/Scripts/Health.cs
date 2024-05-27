using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    
    static int health;
    // Start is called before the first frame update
    void Start()
    {
        health = 8;
        
    }
    public static void IncreaseHealth(int add)
    {
        health += add;
        if (health > 8)
            health = 8;
    }

    public static void DecreaseHealth(int sub)
    {
        health -= sub;
        if (health <= 0)
        {
            health = 0;
            GameOver();
        }
    }

    private static void GameOver()
    {
        // go to game over screen
        throw new NotImplementedException();
    }

    void Update()
    {
        
    }
}
