using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    // Catch information
    [SerializeField] private UIAnimation catchStar;
    private Fish currentFish;
    private float outWaterTime = 1f;
    private float spinTime = 2.5f;

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
        InputManager.Instance.LeftJoystick.started += SwitchAngling;
        InputManager.Instance.LeftJoystick.performed += SwitchAngling;
        InputManager.Instance.LeftJoystick.canceled += SwitchAngling;
        InputManager.Instance.StartButtonAction.performed += OpenMainMenu;
        OpenMainMenu(new InputAction.CallbackContext());
    }

    private void StartIdle()
    {
        State = PlayerState.IDLE;
        idleCanvas.SetActive(true);
        angler.CancelAngling();
    }

    private void StopIdle()
    {
        idleCanvas.SetActive(false);
    }

    private void SwitchAngling(InputAction.CallbackContext ctx)
    {
        Vector2 stickPosition = ctx.ReadValue<Vector2>();
        bool aimingDown = Vector2.Dot(stickPosition, Vector2.down) > 0.0f;
        if (State == PlayerState.IDLE && aimingDown)
        {
            StartAngling();
        }
        else if (State == PlayerState.ANGLING && !aimingDown)
        {
            StopAngling();
        }
    }

    private void StartAngling()
    {
        StopIdle();     
        angler.gameObject.SetActive(true);
        State = PlayerState.ANGLING;
    }

    public void StopAngling()
    {
        angler.gameObject.SetActive(false);
        StartIdle();
    }

    private void StartMiniGame()
    {
        if (State == PlayerState.MINIGAME)
        {
            return;
        }

        miniGameManager.gameObject.SetActive(true);
        miniGameManager.StartRandomMiniGame(currentFish);
        State = PlayerState.MINIGAME;
    }

    public void EndMiniGame(bool isCompleted)
    {
        if (State != PlayerState.MINIGAME)
        {
            return;
        }

        miniGameManager.gameObject.SetActive(false);

        if (isCompleted)
        {
            FishData fishData = currentFish.Catch(catchStar.transform.position, outWaterTime, spinTime);
            Bestiary.NewCatch(currentFish.Data);

            if (fishData != null)
            {
                catchStar.gameObject.SetActive(true);
                catchStar.StartRotation(spinTime, outWaterTime);
            }
        }
        else
        {
            currentFish.UnHook();
            currentFish = null;
        }

        StartIdle();
    }

    public void HookFish(Fish fish)
    {
        // We can only catch fish when Angling
        if (State != PlayerState.ANGLING)
        {
            return;
        }

        // Stop angling
        angler.gameObject.SetActive(false);

        // Reset catch animation
        catchStar.Reset();
        if(currentFish != null) currentFish.Finish();

        // Start minigame
        currentFish = fish;
        StartMiniGame();
    }

    private void OpenMainMenu(InputAction.CallbackContext ctx)
    {
        if(!SceneManager.GetSceneByName("MainMenu").isLoaded && State == PlayerState.IDLE)
        {
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            StartMenu();

            InputManager.Instance.StartButtonAction.performed -= OpenMainMenu;
            InputManager.Instance.StartButtonAction.performed += CloseMainMenu;
            InputManager.Instance.EastButtonAction.performed += CloseMainMenu;
        }
    }

    public void CloseMainMenu(InputAction.CallbackContext ctx)
    {
        if(SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            StopMenu();

            InputManager.Instance.StartButtonAction.performed += OpenMainMenu;
            InputManager.Instance.StartButtonAction.performed -= CloseMainMenu;
            InputManager.Instance.EastButtonAction.performed -= CloseMainMenu;
        }
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
