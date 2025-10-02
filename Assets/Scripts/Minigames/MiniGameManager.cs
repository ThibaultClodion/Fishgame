using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private BaseMiniGame[] miniGames;
    [SerializeField] private Slider progressionSlider;

    private void Start()
    {
        StartMiniGame(0);
    }

    private void StartMiniGame(int index)
    {
        progressionSlider.value = 0;
        progressionSlider.gameObject.SetActive(true);

        miniGames[index].gameObject.SetActive(true);
        miniGames[index].OnAddToProgression += AddToProgression;
        miniGames[index].Initialize();
    }

    private void EndMiniGame(int index)
    {
        miniGames[index].gameObject.SetActive(false);
        miniGames[index].OnAddToProgression -= AddToProgression;
    }

    private void AddToProgression(float value)
    {
        progressionSlider.value += value;
    }
}
