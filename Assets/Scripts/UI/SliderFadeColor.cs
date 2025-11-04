using UnityEngine;
using UnityEngine.UI;

public class SliderFadeColor : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image targetImage;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [SerializeField] private Transform worldObjectToMove;
    [SerializeField] private Vector3 worldObjectOffset;
    [SerializeField] private ParticleSystem particleSystemToUse;
    [SerializeField] private float particleCoeff;

    private float oldValue;

    public void Awake() {
        oldValue = slider.value;
    }

    public void SliderChangeValue()
    {
        targetImage.color = Color.Lerp(startColor, endColor, slider.value);
        if (worldObjectToMove != null) {
            Vector3 canvasPos = slider.handleRect.transform.position;
            worldObjectToMove.position = Camera.main.ScreenToWorldPoint(canvasPos) + worldObjectOffset;
        }   
    }

    public void Update()
    {
        if (particleSystemToUse != null) {
            var emission = particleSystemToUse.emission;
            emission.rateOverTime = FXManager.Instance.particlesEnabled ? Mathf.Max(((slider.value - oldValue) / Time.deltaTime) * particleCoeff, 0.0f) : 0.0f;
        }
        oldValue = slider.value;
    }

    public void OnDisable() {
        if (particleSystemToUse != null) {
            var emission = particleSystemToUse.emission;
            emission.rateOverTime = 0.0f;
        }
    }
}
