using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class BlindnessEffect : MonoBehaviour
{
    [SerializeField] private Image img;

    [SerializeField] private Animator anim;

    private int width, height;

    public bool imageDone;


    public static BlindnessEffect activeInstance;
    void Start()
    {
        activeInstance = this;

        anim = GetComponent<Animator>();
        width = Screen.width;
        height = Screen.height;

        imageDone = false;
    }

    void Update()
    {
        anim = GetComponent<Animator>();
        width = Screen.width;
        height = Screen.height;
        activeInstance = this;
    }
    public void GoBlind(GameObject GrenadeExplosion)
    {
        StartCoroutine(BlindnessEffectCoroutine(GrenadeExplosion));
    }

    private IEnumerator BlindnessEffectCoroutine(GameObject GrenadeExplosion)
    {
        yield return new WaitForEndOfFrame();

        Texture2D tex = new(width, height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        img.sprite = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100);
        imageDone = true;
        anim.SetTrigger("GoBlind");

        GameObject ExplosionInstantiated = Instantiate(GrenadeExplosion, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        HDAdditionalLightData PointLight = ExplosionInstantiated.GetComponentInChildren<HDAdditionalLightData>();

        float time = 2.5f;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / time);
            PointLight.intensity = Mathf.Lerp(7000000, 0, t);
            yield return null;
        }
    }
}
