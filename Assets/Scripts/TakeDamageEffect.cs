using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class TakeDamageEffect : MonoBehaviour

{

    [SerializeField] private Volume volume;
    Vignette vignette;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);

        vignette.active = false;
    }

    public System.Collections.IEnumerator TakeDamage()
    {
        vignette.active = true;
        float intensity = 0.4f;

        vignette.intensity.value = intensity;
        yield return new WaitForSeconds(2.5f);

        while(intensity > 0)
        {
            intensity -= 0.005f;
            vignette.intensity.value = intensity;
            yield return null;
        }
        vignette.active = false;
    }
}
