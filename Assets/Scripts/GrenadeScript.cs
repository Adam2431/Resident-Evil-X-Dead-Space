using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.HighDefinition;
using StarterAssets;

public class GrenadeScript : MonoBehaviour
{

    [SerializeField] private GameObject GrenadeExplosion;

    private GameObject Audio;

    public bool isFlashbang;

    void Start()
    {
        StartCoroutine(ExplodeAfterDelay(2.5f));
        Audio = GameObject.Find("Audio");
    }

    private bool CheckVisibility()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        var point = transform.position;

        foreach (var plane in planes)
        {
            //return (plane.GetDistanceToPoint(point) > 0);
            if (plane.GetDistanceToPoint(point) > 0)
            {
                Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    return hit.transform.gameObject == gameObject;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6);
        foreach (var hitCollider in hitColliders)
        {
            if (gameObject.CompareTag("Grenade"))
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    hitCollider.GetComponent<Enemy>().GetHit(4, null, new Vector3(0, 0, 0), Quaternion.identity);
                    hitCollider.GetComponent<Enemy>().isGrappling = false;
                    hitCollider.GetComponent<Enemy>().EnemyAnimator.SetBool("isHitting", false);
                    hitCollider.GetComponent<Enemy>().EnemyAnimator.SetBool("isKnockedDown", true);
                    hitCollider.GetComponent<Enemy>().EnemyAnimator.SetBool("isGrappling", false);
                    hitCollider.GetComponent<Enemy>().player.GetComponent<Animator>().SetBool("isGrappled", false);
                    ThirdPersonController.instance.isBeingGrappled = false;
                    GameLogic.instance.StrugglingCanvas.SetActive(false);
                }
                else if (hitCollider.CompareTag("Jack"))
                {
                    hitCollider.GetComponent<Jack>().GetHit(15, null, new Vector3(0, 0, 0), Quaternion.identity);
                }
                else if (hitCollider.CompareTag("Player"))
                {
                    ThirdPersonController.instance.GetHit(4);
                }
            }
            else if (hitCollider.CompareTag("Enemy") && gameObject.CompareTag("Flash"))
            {
                hitCollider.gameObject.SendMessage("GetFlashed", SendMessageOptions.DontRequireReceiver);
            }
        }



        if (isFlashbang)
        {
            Audio.GetComponent<Audio>().PlayFashbangSound();
            if ((CheckVisibility() && Vector3.Distance(Camera.main.transform.position, gameObject.transform.position) < 20) || Vector3.Distance(Camera.main.transform.position, gameObject.transform.position) < 5)
            {
                BlindnessEffect.activeInstance.GoBlind(GrenadeExplosion);
                Audio.GetComponent<Audio>().PlayEarRingingSound();
            }
            else
            {
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

        else
        {
            Instantiate(GrenadeExplosion, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
            Audio.GetComponent<Audio>().PlayHandgrenadeSound();
        }
        Destroy(gameObject);
    }
}
