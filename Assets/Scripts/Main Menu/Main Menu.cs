using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using StarterAssets;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup MainMenuCanvas;
    [SerializeField] private CanvasGroup OptionsCanvas;

    [SerializeField] private GameObject MenuButtons;
    [SerializeField] private GameObject OptionButtons;

    [SerializeField] private Animator StartAnimator;
    [SerializeField] private Animator OptionsAnimator;
    [SerializeField] private Animator CreditsAnimator;
    [SerializeField] private Animator ExitAnimator;
    [SerializeField] private GameObject Credits;

    [SerializeField] private GameObject Audio;

    [SerializeField] private GameObject FirePlace;

    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject BackButton;

    private bool isFading = false;
    private float fadeTimeElapsed = 0f;
    private int fader = 0;

    public void StartGame()
    {
       SceneManager.LoadScene("Main Level Omar");
    }

    public void Options()
    {
        isFading = true;
        fader = 1;
        StartAnimator.Play("Hover Animation");
        OptionsAnimator.Play("Hover Animation");
        CreditsAnimator.Play("Hover Animation");
        ExitAnimator.Play("Hover Animation");
        MainMenuCanvas.DOFade(0, 0.25f);
        Audio.GetComponent<Audio>().PlayClickSound();
    }

    public void PlayCredits()
    {
        Credits.SetActive(true);
        Audio.GetComponent<Audio>().StopMusic();
        Audio.GetComponent<Audio>().PlayCreditsMusic();
        FirePlace.GetComponent<AudioSource>().Pause();
    }

    public void Back()
    {
        isFading = true;
        fader = 2;
        OptionsCanvas.DOFade(0, 0.25f);
        Audio.GetComponent<Audio>().PlayClickSound();
    }

    public void Exit()
    {
        Audio.GetComponent<Audio>().PlayClickSound();
        Application.Quit();
    }
    private void Update()
    {
        if(isFading)
        {
            fadeTimeElapsed += Time.deltaTime;
        }
        if(fadeTimeElapsed > 0.25f && isFading)
        {
            if (fader == 1)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                OptionButtons.SetActive(true);
                MenuButtons.SetActive(false);
                OptionsCanvas.DOFade(1, 0.35f);
                EventSystem.current.SetSelectedGameObject(BackButton);
            }
            else if (fader == 2)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                OptionButtons.SetActive(false);
                MenuButtons.SetActive(true);
                MainMenuCanvas.DOFade(1, 0.35f);
                EventSystem.current.SetSelectedGameObject(StartButton);
            }
            StartAnimator.Play("Unhover Animation");
            OptionsAnimator.Play("Unhover Animation");
            CreditsAnimator.Play("Unhover Animation");
            ExitAnimator.Play("Unhover Animation");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
