using UnityEngine;

public class SerialHandler : MonoBehaviour
{
	private SafeSerial _serial;

	// Common default serial device on a Windows machine
	[SerializeField] private string serialPort;
	[SerializeField] [Min(0)] private int baudrate;
	private const float OpenAttemptDelay = 5;
	private float _lastOpenAttemptTime = -OpenAttemptDelay;


	void Start()
	{
		_serial = new SafeSerial(serialPort, baudrate);
		// Guarantee that the newline is common across environments.
		_serial.NewLine = "\n";
		// Once configured, the serial communication must be opened.
		// Just like a file or a socket : the OS handles the hard work.
		OpenSerial();
	}

    // Try to open the configured serial port, rate limited by OpenAttemptDelay
    // Return if we successfully opened.
    private bool OpenSerial()
	{
		// Don't try to re-connect too often
		if (Time.time < _lastOpenAttemptTime + OpenAttemptDelay) return false;
		_lastOpenAttemptTime = Time.time;
		
		// Allow updating the port name in the inspector while running the game.
		_serial.PortName =  serialPort;
		if (!_serial.Open()) {
			return false;
		}

		Debug.Log($"Serial opened on {serialPort}");
		return true;
	}

    void Update()
    {
        if (!_serial.IsOpen && !OpenSerial()) return;
        if (_serial.BytesToRead <= 0) return;

        string message = _serial.ReadLine();
        if (string.IsNullOrEmpty(message) || message.Length < 3) return;

        // First byte is our header
        char header = message[0];

        // The following bytes are our value (Payload)
        // Warning: Arduino sends in Little-Endian (Low byte first)
        byte lowByte = (byte)message[1];
        byte highByte = (byte)message[2];
        int rawValue = (highByte << 8) | lowByte;

        switch (header)
        {
            case 'I':
                HandleIRInput(rawValue);
                break;

            default:
                Debug.LogWarning("Message with unknown header: " + header);
                break;
        }
    }

    private void HandleIRInput(int commandCode)
    {
        switch (commandCode)
        {
            case 0x45: Debug.Log("IR: POWER"); break;
            case 0x46: Debug.Log("IR: VOL+"); break;
            case 0x47: Debug.Log("IR: FUNC/STOP"); break;

            case 0x44: Debug.Log("IR: REWIND (Retour)"); break;
            case 0x40: Debug.Log("IR: PLAY/PAUSE"); break;
            case 0x43: Debug.Log("IR: FAST FORWARD (Avance)"); break;

            case 0x07: Debug.Log("IR: DOWN ARROW"); break;
            case 0x15: Debug.Log("IR: VOL-"); break;
            case 0x09: Debug.Log("IR: UP ARROW"); break;

            case 0x19: Debug.Log("IR: EQ"); break;
            case 0x0D: Debug.Log("IR: ST/REPT"); break;

            case 0x16: InputManager.Instance.TelecomandActions[0] = true; break;
            case 0x0C: InputManager.Instance.TelecomandActions[1] = true; break;
            case 0x18: InputManager.Instance.TelecomandActions[2] = true; break;
            case 0x5E: InputManager.Instance.TelecomandActions[3] = true; break;
            case 0x08: InputManager.Instance.TelecomandActions[4] = true; break;
            case 0x1C: InputManager.Instance.TelecomandActions[5] = true; break;
            case 0x5A: InputManager.Instance.TelecomandActions[6] = true; break;
            case 0x42: InputManager.Instance.TelecomandActions[7] = true; break;
            case 0x52: InputManager.Instance.TelecomandActions[8] = true; break;
            case 0x4A: InputManager.Instance.TelecomandActions[9] = true; break;

            default:
                Debug.LogWarning($"Touche IR inconnue reçue: 0x{commandCode:X2}");
                break;
        }
    }

    public void SendRawString(string data)
    {
        if (_serial != null && _serial.IsOpen)
        {
            _serial.WriteLine(data);
        }
    }

    private void OnDestroy()
	{
		_serial.Close();
    }
}
