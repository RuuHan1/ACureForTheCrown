using UnityEngine;
using UnityEngine.UI;

public class CancerVisualizer : MonoBehaviour
{
    [SerializeField] private Image cancerImageDisplay; 
    [SerializeField] private Sprite[] cancerSprites; // 3 adet resim (0: Hafif, 1: Orta, 2: Ağır)

    private void OnEnable() => GameEvents.CancerStageChanged += UpdateCancerSprite;
    private void OnDisable() => GameEvents.CancerStageChanged -= UpdateCancerSprite;

    private void UpdateCancerSprite(int stageIndex)
    {
        if (stageIndex >= 0 && stageIndex < cancerSprites.Length)
        {
            cancerImageDisplay.sprite = cancerSprites[stageIndex];
        }
    }
}
