using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputAction RightJoystick { get; private set; }
    public InputAction LeftJoystick { get; private set; }

    public InputAction NorthButtonAction { get; private set; }
    public InputAction EastButtonAction { get; private set; }
    public InputAction SouthButtonAction { get; private set; }
    public InputAction WestButtonAction { get; private set; }


    [SerializeField] private PlayerInput playerInput;

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
        RightJoystick = playerInput.actions["RightJoystick"];
        LeftJoystick = playerInput.actions["LeftJoystick"];

        NorthButtonAction = playerInput.actions["NorthButton"];
        EastButtonAction = playerInput.actions["EastButton"];
        SouthButtonAction = playerInput.actions["SouthButton"];
        WestButtonAction = playerInput.actions["WestButton"];
    }
}
