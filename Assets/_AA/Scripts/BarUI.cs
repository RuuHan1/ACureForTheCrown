using TMPro;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    public StatType statType;
    [SerializeField] private UnityEngine.UI.Image fillImage; // Slider veya Image bileşeni
    private void Start()
    {
        fillImage.fillAmount = 0;
        UpdateValue(5);
    }
    public void UpdateValue(int amount)
    {
        float newValue = Mathf.Clamp01(fillImage.fillAmount + (amount / 10f));
        fillImage.fillAmount = newValue;

        if (statType == StatType.Cancer && newValue <= 0f)
        {
            GameEvents.GameOver?.Invoke(true); // Kazandın
            Debug.Log("True");
            return; // Fonksiyondan çık
        }
        if (statType == StatType.Cancer && newValue >= 1f)
        {
            GameEvents.GameOver?.Invoke(false); // Kaybettin
            Debug.Log("False");

            return;
        }

        if (statType != StatType.Cancer && newValue <= 0f)
        {
            GameEvents.GameOver?.Invoke(false); // Kaybettin
            Debug.Log("False");

            return;
        }
    }
}
