using UnityEngine;

public class LCDDisplayer : MonoBehaviour
{
    public static LCDDisplayer Instance { get; private set; }

    [SerializeField] private SerialHandler serialHandler;

    private void Awake()
    {
        // Configuration du Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Clear();
    }

    public void DisplayText(int column, int row, string text)
    {
        if (serialHandler != null)
        {
            string message = string.Format("LCD:{0},{1},{2}", column, row, text);
            serialHandler.SendRawString(message);
        }
        else
        {
            Debug.LogWarning("SerialHandler non assigné au LCDDisplayer !");
        }
    }

    public void Clear()
    {
        serialHandler.SendRawString("LCD:0,0,                "); // 16 espaces
        serialHandler.SendRawString("LCD:0,1,                ");
    }
}