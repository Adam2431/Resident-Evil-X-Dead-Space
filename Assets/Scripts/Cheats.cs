using StarterAssets;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private float timeBetweenCharacters = 0.2f;
    private float timeSinceLastCharacter = 0.2f;
    private string cheatCode = "GAZZAR";
    private string codeSoFar = "";

    public bool infiniteAmmoActive = false;
    public static Cheats instance;

    void Update()
    {
        if (instance == null)
        {
            instance = this;
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            codeSoFar += "G";
            timeSinceLastCharacter = timeBetweenCharacters;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            codeSoFar += "A";
            timeSinceLastCharacter = timeBetweenCharacters;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            codeSoFar += "Z";
            timeSinceLastCharacter = timeBetweenCharacters;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            codeSoFar += "R";
            timeSinceLastCharacter = timeBetweenCharacters;
        }

        if(timeSinceLastCharacter <= 0)
        {
            if (codeSoFar.Equals(cheatCode))
            {
                GameLogic.instance.AudioScript.PlayCheatSound();
                GetComponent<Gold>().PickupGold(1000);
                GameLogic.instance.Player.GetComponent<ThirdPersonController>().health = 8;
                infiniteAmmoActive = !infiniteAmmoActive;
            }
            codeSoFar = "";
            timeSinceLastCharacter = timeBetweenCharacters;
        }
        else
        {
            timeSinceLastCharacter -= Time.deltaTime;
        }
    }
}
