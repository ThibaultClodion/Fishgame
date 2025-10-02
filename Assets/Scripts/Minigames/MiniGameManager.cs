using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private BaseMiniGame[] miniGames;
    [SerializeField] private Slider progressionSlider;

    private int currentMiniGameIndex = -1;

    public void StartRandomMiniGame()
    {
        int randomIndex = Random.Range(0, miniGames.Length);
        StartMiniGame(randomIndex);
    }

    private void StartMiniGame(int index)
    {
        progressionSlider.value = 0;
        progressionSlider.gameObject.SetActive(true);

        Debug.Log("Launching MiniGame "+miniGames[index]);

        miniGames[index].gameObject.SetActive(true);
        miniGames[index].OnAddToProgression += AddToProgression;
        miniGames[index].Initialize();
        currentMiniGameIndex = index;
    }

    private void EndMiniGame(bool isCompleted)
    {
        miniGames[currentMiniGameIndex].gameObject.SetActive(false);
        miniGames[currentMiniGameIndex].OnAddToProgression -= AddToProgression;
        currentMiniGameIndex = -1;

        GameManager.Instance.EndMiniGame(isCompleted);
    }

    private void AddToProgression(float value)
    {
        progressionSlider.value += value;

        if(progressionSlider.value >= 1f)
        {
            EndMiniGame(true);
        }
        else if(progressionSlider.value <= 0f)
        {
            EndMiniGame(false);
        }
    }
}
