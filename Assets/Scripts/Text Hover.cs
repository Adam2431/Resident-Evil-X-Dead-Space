using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    [SerializeField] Animator StartAnimator;
    [SerializeField] Animator OptionsAnimator;
    [SerializeField] Animator CreditsAnimator;
    [SerializeField] Animator ExitAnimator;

    [SerializeField] private GameObject Audio;

    int CurrentHover;
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        Audio.GetComponent<Audio>().PlayHoverSound();
        if(gameObject.name == "Start Game")
        {
            StartAnimator.Play("Hover Animation");
            if(CurrentHover == 2)
            {
                OptionsAnimator.Play("Unhover Animation");
            }
            else if(CurrentHover == 3)
            {
                CreditsAnimator.Play("Unhover Animation");
            }
            else if(CurrentHover == 4)
            {
                ExitAnimator.Play("Unhover Animation");
            }
            CurrentHover = 1;
        }
        else if (gameObject.name == "Options")
        {
            OptionsAnimator.Play("Hover Animation");
            if (CurrentHover == 1)
            {
                StartAnimator.Play("Unhover Animation");
            }
            else if (CurrentHover == 3)
            {
                CreditsAnimator.Play("Unhover Animation");
            }
            else if (CurrentHover == 4)
            {
                ExitAnimator.Play("Unhover Animation");
            }
            CurrentHover = 2;
        }
        else if (gameObject.name == "Exit")
        {
            ExitAnimator.Play("Hover Animation");
            if (CurrentHover == 1)
            {
                StartAnimator.Play("Unhover Animation");
            }
            else if (CurrentHover == 2)
            {
                OptionsAnimator.Play("Unhover Animation");
            }
            else if (CurrentHover == 3)
            {
                CreditsAnimator.Play("Unhover Animation");
            }
            CurrentHover = 4;
        }
        else if (gameObject.name == "Credits")
        {
            CreditsAnimator.Play("Hover Animation");
            if (CurrentHover == 1)
            {
                StartAnimator.Play("Unhover Animation");
            }
            else if (CurrentHover == 2)
            {
                OptionsAnimator.Play("Unhover Animation");
            }
            else if (CurrentHover == 4)
            {
                ExitAnimator.Play("Unhover Animation");
            }
            CurrentHover = 3;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (gameObject.name == "Start Game")
        {
            StartAnimator.Play("Unhover Animation");
        }
        else if (gameObject.name == "Options")
        {
            OptionsAnimator.Play("Unhover Animation");
        }
        else if(gameObject.name == "Credits")
        {
            CreditsAnimator.Play("Unhover Animation");
        }
        else if (gameObject.name == "Exit")
        {
            ExitAnimator.Play("Unhover Animation");
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Audio.GetComponent<Audio>().PlayHoverSound();
    }
}
