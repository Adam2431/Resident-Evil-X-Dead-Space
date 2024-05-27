using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CreditsButtonScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
