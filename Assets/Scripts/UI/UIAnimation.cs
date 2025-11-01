using System.Collections;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private float rotationStart;

    public void StartRotation(float seconds, float waitTime)
    {
        StartCoroutine(RotateOverTime(seconds, waitTime));
    }

    private IEnumerator RotateOverTime(float seconds, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rotationStart = Time.time;
        float elapsed = 0f;

        while (elapsed < seconds)
        {
            float angle = (elapsed / seconds) * 360f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            elapsed = Time.time - rotationStart;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.SetActive(false);
    }
}
