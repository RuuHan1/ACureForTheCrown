using UnityEngine;
using UnityEngine.UI;
public class BarUI : MonoBehaviour
{
    public StatType statType;
    [SerializeField] private Image fillImage; // Slider veya Image bileşeni
    private const float MIN_VISIBLE_FILL = 0.05f;
    private void Start()
    {
        fillImage.fillAmount = 0;
        UpdateValue(5);
    }
    public void UpdateValue(int amount)
    {
        float rawValue = fillImage.fillAmount + (amount / 10f);
        float newValue = Mathf.Clamp01(rawValue);

        if (newValue > 0f && newValue < MIN_VISIBLE_FILL)
            newValue = MIN_VISIBLE_FILL;

        fillImage.fillAmount = newValue;

        if (statType == StatType.Cancer && rawValue <= 0f)
        {
            GameEvents.GameOver?.Invoke(true);
            return;
        }

        if (statType == StatType.Cancer && rawValue >= 1f)
        {
            GameEvents.GameOver?.Invoke(false);
            return;
        }

        if (statType != StatType.Cancer && rawValue <= 0f)
        {
            GameEvents.GameOver?.Invoke(false);
        }
    }
}
