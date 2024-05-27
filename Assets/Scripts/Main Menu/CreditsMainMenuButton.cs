using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMainMenuButton : MonoBehaviour
{
    [SerializeField] private AudioSource FireSFX;
    [SerializeField] private GameObject Audio;

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            Audio.GetComponent<Audio>().StopMusic();
            Audio.GetComponent<Audio>().PlayLoopedMainMenuMusic();
            FireSFX.Play();
            gameObject.SetActive(false);
        }
    }
}
