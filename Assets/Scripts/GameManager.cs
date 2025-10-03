using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private MiniGameManager miniGameManager;
    [SerializeField] private Angler angler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        StartAngling();
    }

    private void StartAngling()
    {
        angler.gameObject.SetActive(true);
    }

    private void StartMiniGame()
    {
        miniGameManager.gameObject.SetActive(true);
        miniGameManager.StartRandomMiniGame();
    }

    public void EndMiniGame(bool isCompleted)
    {
        miniGameManager.gameObject.SetActive(false);

        // TODO : Handle post-mini-game logic here (e.g., rewards, penalties)
        if (isCompleted)
        {
            Debug.Log("Mini-game successfully completed!");
        }
        else
        {
            Debug.Log("Mini-game failed.");
        }
    }
}
