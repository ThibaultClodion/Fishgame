using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip miniGameSuccessSFX;
    [SerializeField] private AudioClip miniGameFailureSFX;

    [Header("References")]
    [SerializeField] private BaseMiniGame[] miniGames;
    [SerializeField] private GameObject commonContainer;
    [SerializeField] private Slider progressionSlider;
    [SerializeField] private TMP_Text fishNameText;

    private int currentMiniGameIndex = -1;
    private Fish currentFish;

    private RealRandom random = new RealRandom(1);

    public void StartRandomMiniGame(Fish fish)
    {
        int randomIndex = this.random.Range(0, miniGames.Length);
        currentFish = fish;


        StartMiniGame(randomIndex);
    }

    private void StartMiniGame(int index)
    {
        progressionSlider.value = 0;
        fishNameText.text = currentFish.Data.Name;
        commonContainer.SetActive(true);

        miniGames[index].gameObject.SetActive(true);
        miniGames[index].AddToProgressionCallback = AddToProgression;
        miniGames[index].Initialize(currentFish.Data);
        currentMiniGameIndex = index;
    }

    private void EndMiniGame(bool isCompleted)
    {
        miniGames[currentMiniGameIndex].Disable();
        miniGames[currentMiniGameIndex].gameObject.SetActive(false);
        miniGames[currentMiniGameIndex].AddToProgressionCallback = null;
        currentMiniGameIndex = -1;
        commonContainer.SetActive(true);

        if(isCompleted)
        {
            AudioManager.Instance.PlaySFX(miniGameSuccessSFX);
        }
        else
        {
            AudioManager.Instance.PlaySFX(miniGameFailureSFX);
        }

        GameManager.Instance.EndMiniGame(isCompleted);
    }

    // Returns if the minigame ended or not
    private bool AddToProgression(float value)
    {
        progressionSlider.value += value;

        if(progressionSlider.value >= 1f)
        {
            EndMiniGame(true);
            return true;
        }
        else if(progressionSlider.value <= 0f)
        {
            EndMiniGame(false);
            return true;
        }

        return false;
    }
}
