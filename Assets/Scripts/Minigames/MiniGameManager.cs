using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private BaseMiniGame[] miniGames;
    [SerializeField] private Slider sucessSlider;

    private void Update()
    {
        // sucessSlider.value = Mathf.Lerp(0, 1, Time.deltaTime * 0.2f + sucessSlider.value);
    }
}
