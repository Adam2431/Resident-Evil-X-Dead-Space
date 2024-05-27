using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadePages : MonoBehaviour
{
    [SerializeField] private CanvasGroup buyPageCanvas;
    [SerializeField] private CanvasGroup sellPageCanvas;
    [SerializeField] private CanvasGroup storePageCanvas;

    [SerializeField] private GameObject sellPage;
    [SerializeField] private GameObject buyPage;
    [SerializeField] private GameObject storePage;

    [SerializeField] private Animator buyPageAnimator;
    [SerializeField] private Animator sellPageAnimator;
    [SerializeField] private Animator storePageAnimator;

    [SerializeField] private List<GameObject> infoList;

    private bool isFading = false;
    private float fadeTimeElapsed = 0f;
    private int fader = 0;

    public void SellPage()
    {
        isFading = true;
        fader = 1;
        buyPageAnimator.Play("Fade Out");
        buyPageCanvas.DOFade(0, 0.25f);
        storePageAnimator.Play("Fade Out");
        storePageCanvas.DOFade(0, 0.25f);
    }

    public void BuyPage()
    {
        isFading = true;
        fader = 2;
        sellPageAnimator.Play("Fade Out");
        sellPageCanvas.DOFade(0, 0.25f);
        storePageAnimator.Play("Fade Out");
        storePageCanvas.DOFade(0, 0.25f);
    }

    public void StorePage()
    {
        isFading = true;
        fader = 3;
        buyPageAnimator.Play("Fade Out");
        buyPageCanvas.DOFade(0, 0.25f);
        sellPageAnimator.Play("Fade Out");
        sellPageCanvas.DOFade(0, 0.25f);
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
                sellPage.SetActive(true);
                buyPage.SetActive(false);
                storePage.SetActive(false);

                buyPageCanvas.DOFade(1, 0.35f);
                storePageCanvas.DOFade(1, 0.35f);
            }
            else if (fader == 2)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                sellPage.SetActive(false);
                buyPage.SetActive(true);
                storePage.SetActive(false);

                sellPageCanvas.DOFade(1, 0.35f);
                storePageCanvas.DOFade(1, 0.35f);
            }
            else if (fader == 3)
            {
                isFading = false;
                fadeTimeElapsed = 0f;
                sellPage.SetActive(false);
                buyPage.SetActive(false);
                storePage.SetActive(true);

                sellPageCanvas.DOFade(1, 0.35f);
                buyPageCanvas.DOFade(1, 0.35f);
            }

            buyPageAnimator.Play("Fade In");
            sellPageAnimator.Play("Fade In");
            storePageAnimator.Play("Fade In");

            foreach (var info in infoList)
            {
                info.SetActive(false);
            }
        }
    }
}
