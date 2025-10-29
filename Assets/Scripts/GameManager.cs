using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum PlayerState {
        IDLE,
        ANGLING,
        MINIGAME,
        INMENU
    };

    public PlayerState State {get; private set;}
    private PlayerState previousState;

    [SerializeField] private MiniGameManager miniGameManager;
    [SerializeField] private Angler angler;
    [SerializeField] private GameObject idleCanvas;

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
        StartIdle();
        InputManager.Instance.SouthButtonAction.performed += StartAngling;
    }

    private void StartIdle()
    {
        State = PlayerState.IDLE;
        idleCanvas.SetActive(true);
    }

    private void StopIdle()
    {
        idleCanvas.SetActive(false);
    }

    private void StartAngling(InputAction.CallbackContext ctx)
    {
        if (State != PlayerState.IDLE)
        {
            return;
        }

        StopIdle();     
        angler.gameObject.SetActive(true);
        State = PlayerState.ANGLING;
    }

    public void StopAngling()
    {
        if (State != PlayerState.ANGLING)
        {
            return;
        }

        angler.gameObject.SetActive(false);
        StartIdle();
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

    public void EndMiniGame(bool isCompleted, FishData fishData)
    {
        if (State != PlayerState.MINIGAME)
        {
            return;
        }

        miniGameManager.gameObject.SetActive(false);

        // TODO : Handle post-mini-game logic here (e.g., rewards, penalties)
        if (isCompleted)
        {
            Bestiary.NewCatch(fishData);
        }
        else
        {
            Debug.Log("Mini-game failed.");
        }

        StartIdle();
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

    public void StartMenu()
    {
        previousState = State;
        State = PlayerState.INMENU;
    }

    public void StopMenu()
    {
        State = previousState;
    }
}
