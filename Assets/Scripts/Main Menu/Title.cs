using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{

    [SerializeField] private CanvasGroup TitleCanvas;
    [SerializeField] private CanvasGroup TempCanvas;
    [SerializeField] private CanvasGroup MainMenuCanvas;

    [SerializeField] private GameObject MainMenuObjects;

    [SerializeField] private GameObject Audio;

    [SerializeField] private GameObject StartGame;

    private float fadeTimeElapsed = 0f;
    private bool isFading = false;

    private bool faded = false;
    public void Fader()
    {
        TitleCanvas.DOFade(0, 2);
        isFading = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        TitleCanvas.DOFade(1, 2);
        Audio.GetComponent<Audio>().PlayAwakeSound();
        MainMenuObjects.SetActive(false);
        Audio.GetComponent<Audio>().PlayLoopedMainMenuMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFading)
        {
            fadeTimeElapsed += Time.deltaTime;
        }
        if(fadeTimeElapsed > 3 && !faded)
        {
            TempCanvas.DOFade(0, 5);
            MainMenuObjects.SetActive(true);
            MainMenuCanvas.DOFade(1, 5);
            EventSystem.current.SetSelectedGameObject(StartGame);
            faded = true;
        }
        if (Input.anyKeyDown && !isFading) { 
            Fader();
            Audio.GetComponent<Audio>().PlayStartSound();
        }
    }
}
