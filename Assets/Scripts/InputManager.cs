using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections.Generic;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputAction RightJoystick { get; private set; }
    public InputAction LeftJoystick { get; private set; }

    public InputAction NorthButtonAction { get; private set; }
    public InputAction EastButtonAction { get; private set; }
    public InputAction SouthButtonAction { get; private set; }
    public InputAction WestButtonAction { get; private set; }
    public InputAction SelectButtonAction { get; private set; }
    public InputAction StartButtonAction { get; private set; }

    public InputAction RightTrigger { get; private set; }

    // Array of all buttons, used for QTE
    public InputAction[] ButtonActions { get; private set; }

    private PlayerInput playerInput;

    // Can either poll the variable
    public bool UsingKeyboard {get; set;}
    // Or use the event
    public delegate void OnInputModeChangeEventDelegate(bool usingKeyboard);
    public event OnInputModeChangeEventDelegate OnInputModeChangeEvent;

    // Keyboard & gamepad sprite
    [System.Serializable]
    public struct KeyImage {
        public Sprite keyboard;
        public Sprite gamepad;
    };

    public enum KeyType {
        RightJoystick,
        LeftJoystick,
        NorthButton,
        EastButton,
        SouthButton,
        WestButton,
        SelectButton,
        StartButton,
        RightTrigger
    };

    public Dictionary<KeyType, KeyImage> KeyImages { get; private set; }
    public Dictionary<InputAction, KeyType> KeyTypes { get; private set; }

    // Hack to get a "dictionary" working in the inspector... gotta love Unity
    [System.Serializable]
    private struct KeyInfo {
        public KeyType type;
        public KeyImage images;
    }
    [SerializeField]
    private KeyInfo[] keyInfos;

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
        playerInput = GetComponent<PlayerInput>();

        RightJoystick = playerInput.actions["RightJoystick"];
        LeftJoystick = playerInput.actions["LeftJoystick"];

        NorthButtonAction = playerInput.actions["NorthButton"];
        EastButtonAction = playerInput.actions["EastButton"];
        SouthButtonAction = playerInput.actions["SouthButton"];
        WestButtonAction = playerInput.actions["WestButton"];
        SelectButtonAction = playerInput.actions["SelectButton"];
        StartButtonAction = playerInput.actions["StartButton"];
        RightTrigger = playerInput.actions["RightTrigger"];

        // Fill the array
        ButtonActions = new InputAction[4];
        ButtonActions[0] = NorthButtonAction;
        ButtonActions[1] = EastButtonAction;
        ButtonActions[2] = SouthButtonAction;
        ButtonActions[3] = WestButtonAction;

        // Add Action to input mapping
        KeyTypes = new Dictionary<InputAction, KeyType>();
        KeyTypes.Add(RightJoystick, KeyType.RightJoystick);
        KeyTypes.Add(LeftJoystick, KeyType.LeftJoystick);
        KeyTypes.Add(NorthButtonAction, KeyType.NorthButton);
        KeyTypes.Add(EastButtonAction, KeyType.EastButton);
        KeyTypes.Add(SouthButtonAction, KeyType.SouthButton);
        KeyTypes.Add(WestButtonAction, KeyType.WestButton);
        KeyTypes.Add(SelectButtonAction, KeyType.SelectButton);
        KeyTypes.Add(StartButtonAction, KeyType.StartButton);
        KeyTypes.Add(RightTrigger, KeyType.RightTrigger);

        KeyImages = new Dictionary<KeyType, KeyImage>();
        // Unity doodoo
        for (int i=0;i<keyInfos.Length;i++)
            KeyImages.Add(keyInfos[i].type, keyInfos[i].images);
    }

    private void Update()
    {
        bool newUsingKeyboard = playerInput.currentControlScheme == "Keyboard&Mouse";
        if (UsingKeyboard != newUsingKeyboard) {
            UsingKeyboard = newUsingKeyboard;
            if (OnInputModeChangeEvent != null)
                OnInputModeChangeEvent(newUsingKeyboard);
        }
    }
}
