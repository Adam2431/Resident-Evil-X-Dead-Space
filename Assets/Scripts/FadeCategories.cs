using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeCategories : MonoBehaviour
{
    [SerializeField] private CanvasGroup weaponsCanvas;
    [SerializeField] private CanvasGroup ammoCanvas;
    [SerializeField] private CanvasGroup itemsCanvas;

    [SerializeField] private GameObject weapons;
    [SerializeField] private GameObject ammo;
    [SerializeField] private GameObject items;

    [SerializeField] private Animator weaponsAnimator;
    [SerializeField] private Animator ammoAnimator;
    [SerializeField] private Animator itemsAnimator;

    [SerializeField] private GameObject WeaponsGradient;
    [SerializeField] private GameObject AmmoGradient;
    [SerializeField] private GameObject ItemsGradient;


    [SerializeField] private List<GameObject> infoList;

    private bool isFading = false;
    private float fadeTimeElapsed = 0f;
    private int fader = 0;

    public void WeaponsPage()
    {
        isFading = true;
        fader = 1;
        ammoAnimator.Play("Fade Out");
        ammoCanvas.DOFade(0, 0.25f);
        itemsAnimator.Play("Fade Out");
        itemsCanvas.DOFade(0, 0.25f);
    }

    public void AmmoPage()
    {
        isFading = true;
        fader = 2;
        weaponsAnimator.Play("Fade Out");
        weaponsCanvas.DOFade(0, 0.25f);
        itemsAnimator.Play("Fade Out");
        itemsCanvas.DOFade(0, 0.25f);
    }

    public void ItemsPage()
    {
        isFading = true;
        fader = 3;
        weaponsAnimator.Play("Fade Out");
        weaponsCanvas.DOFade(0, 0.25f);
        ammoAnimator.Play("Fade Out");
        ammoCanvas.DOFade(0, 0.25f);
    }

    private void Start()
    {
        WeaponsGradient.SetActive(true);
    }

    private void Update()
    {
        if (isFading)
        {
            fadeTimeElapsed += Time.deltaTime;
        }
        if (fadeTimeElapsed > 0.25f && isFading)
        {
            if (fader == 1)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                weapons.SetActive(true);
                ammo.SetActive(false);
                items.SetActive(false);
                ammoCanvas.DOFade(1, 0.35f);
                itemsCanvas.DOFade(1, 0.35f);
                WeaponsGradient.SetActive(true);
                AmmoGradient.SetActive(false);
                ItemsGradient.SetActive(false);
            }
            else if (fader == 2)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                weapons.SetActive(false);
                ammo.SetActive(true);
                items.SetActive(false);
                weaponsCanvas.DOFade(1, 0.35f);
                itemsCanvas.DOFade(1, 0.35f);
                WeaponsGradient.SetActive(false);
                AmmoGradient.SetActive(true);
                ItemsGradient.SetActive(false);
            }
            else if (fader == 3)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                weapons.SetActive(false);
                ammo.SetActive(false);
                items.SetActive(true);
                weaponsCanvas.DOFade(1, 0.35f);
                ammoCanvas.DOFade(1, 0.35f);
                WeaponsGradient.SetActive(false);
                AmmoGradient.SetActive(false);
                ItemsGradient.SetActive(true);
            }
            weaponsAnimator.Play("Fade In");
            ammoAnimator.Play("Fade In");
            itemsAnimator.Play("Fade In");

            foreach (var info in infoList)
            {
                info.SetActive(false);
            }
        }
    }
}
