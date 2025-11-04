using System.Collections;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private float startTime;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float particleEmissionRate;

    public void Reset()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.SetActive(false);
    }

    public void StartRotation(float seconds, float waitTime)
    {
        StartCoroutine(RotateOverTime(seconds, waitTime));
    }

    private IEnumerator RotateOverTime(float seconds, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (particles != null) {
            var particleEmission = particles.emission;
            particleEmission.rateOverTime = FXManager.Instance.particlesEnabled ? particleEmissionRate : 0.0f;
        }
        startTime = Time.time;
        float elapsed = 0f;

        while (elapsed < seconds)
        {
            float angle = (elapsed / seconds) * 360f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            elapsed = Time.time - startTime;
            yield return null;
        }

        if (particles != null) {
            var particleEmission = particles.emission;
            particleEmission.rateOverTime = 0.0f;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.SetActive(false);
    }
}
