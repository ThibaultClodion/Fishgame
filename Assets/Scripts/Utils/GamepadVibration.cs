using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadVibration : MonoBehaviour
{
    public static GamepadVibration Instance { get; private set; }
    [HideInInspector] public bool CanVibrate = true;
    private bool isVibrating = false;


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

    public void EnableVibration(bool enable)
    {
        CanVibrate = enable;
    }

    public void Vibration(float lowFrequency, float highFrequency, float duration)
    {
        if (CanVibrate && Gamepad.current != null && isVibrating == false)
        {
            isVibrating = true;
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(StopVibration(duration));
        }
    }

    private IEnumerator StopVibration(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (Gamepad.current != null)
        {
            isVibrating = false;
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}
