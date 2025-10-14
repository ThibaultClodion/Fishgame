using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private BaseMiniGame[] miniGames;
    [SerializeField] private Slider progressionSlider;
    [SerializeField] private TMP_Text fishNameText;

    private int currentMiniGameIndex = -1;
    private FishData currentFishData;

    private RealRandom random = new RealRandom(1);

    public void StartRandomMiniGame(FishData data)
    {
        int randomIndex = this.random.Range(0, miniGames.Length);
        currentFishData = data;

        StartMiniGame(randomIndex);
    }

    private void StartMiniGame(int index)
    {
        progressionSlider.value = 0;
        fishNameText.text = currentFishData.Name;

        Debug.Log("Launching MiniGame "+miniGames[index]);

        miniGames[index].gameObject.SetActive(true);
        miniGames[index].AddToProgressionCallback = AddToProgression;
        miniGames[index].Initialize(currentFishData);
        currentMiniGameIndex = index;
    }

    private void EndMiniGame(bool isCompleted)
    {
        miniGames[currentMiniGameIndex].Disable();
        miniGames[currentMiniGameIndex].gameObject.SetActive(false);
        miniGames[currentMiniGameIndex].AddToProgressionCallback = null;
        currentMiniGameIndex = -1;

        GameManager.Instance.EndMiniGame(isCompleted, currentFishData);
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
