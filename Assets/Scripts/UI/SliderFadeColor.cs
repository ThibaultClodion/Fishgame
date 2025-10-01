using UnityEngine;
using UnityEngine.UI;

public class SliderFadeColor : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image targetImage;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    public void SliderChangeValue()
    {
        targetImage.color = Color.Lerp(startColor, endColor, slider.value);
    }
}
