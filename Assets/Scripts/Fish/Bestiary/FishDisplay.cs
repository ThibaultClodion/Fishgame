using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishNameText;
    [SerializeField] private Image fishImage;
    [SerializeField] private TextMeshProUGUI fishDescriptionText;
    [SerializeField] private TextMeshProUGUI fishMaxSizeText;
    [SerializeField] private TextMeshProUGUI fishMinSizeText;
    [SerializeField] private TextMeshProUGUI fishNbCatchedText;

    public void Initialize(FishType fishType)
    {
        BestiaryEntry bestiaryEntry = Bestiary.GetBestiaryEntry(fishType);

        if(bestiaryEntry == null)
        {
            fishNameText.text = "";
            fishImage.sprite = null;
            // TODO : fishDescriptionText.text = "";
            fishMaxSizeText.text = "";
            fishMinSizeText.text = "";
            fishNbCatchedText.text = "";
        }
        else
        {
            fishNameText.text = fishType.name;
            fishImage.sprite = fishType.sprite;
            // TODO : fishDescriptionText.text = fishType.description;
            fishMaxSizeText.text = $"Max Size: {bestiaryEntry.MaxSize} cm";
            fishMinSizeText.text = $"Min Size: {bestiaryEntry.MinSize} cm";
            fishNbCatchedText.text = $"Caught: {bestiaryEntry.NbCatched}";
        }
    }
}
