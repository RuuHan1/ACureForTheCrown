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
        // 0-1 arası normalize edilmiş değer hesapla
        float newValue = Mathf.Clamp01(fillImage.fillAmount + (amount / 10f));
        fillImage.fillAmount = newValue;
    }
}
