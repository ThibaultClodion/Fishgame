using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Image image;
    private FishType fishType;
    private FishDisplay fishDisplay;

    public void Initialize(FishType fishType, FishDisplay fishDisplay)
    {
        this.fishType = fishType;
        this.fishDisplay = fishDisplay;
        image.sprite = fishType.sprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        fishDisplay.Initialize(fishType);
    }
}
