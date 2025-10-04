using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum PlayerState {
        IDLE,
        ANGLING,
        MINIGAME
    };

    public PlayerState State {get; private set;}

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
        State = PlayerState.IDLE;
        StartAngling();
    }

    private void StartAngling()
    {
        if (State == PlayerState.ANGLING)
        {
            return;
        }
        angler.gameObject.SetActive(true);
        State = PlayerState.ANGLING;
    }

    private void StopAngling()
    {
        if (State != PlayerState.ANGLING)
        {
            return;
        }
        angler.gameObject.SetActive(false);
        State = PlayerState.IDLE;
    }

    private void StartMiniGame(FishData data)
    {
        if (State == PlayerState.MINIGAME)
        {
            return;
        }
        miniGameManager.gameObject.SetActive(true);
        miniGameManager.StartRandomMiniGame(data);
        State = PlayerState.MINIGAME;
    }

    public void EndMiniGame(bool isCompleted)
    {
        if (State != PlayerState.MINIGAME)
        {
            return;
        }

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

        // Go back to Angling
        StartAngling();
    }

    public void CatchFish(FishData data)
    {
        // We can only catch fish when Angling
        if (State != PlayerState.ANGLING)
        {
            return;
        }

        // Stop angling
        StopAngling();

        // Start a Random Minigame
        StartMiniGame(data);
    }
}
