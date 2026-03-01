using UnityEngine;
using UnityEngine.UI;

// SADECE GÖRSELLİĞİ YÖNETEN SINIF (Clean Code - View)
// Artik burada matematik veya GameOver mantigi YOK. Tamamen arindirdik. 
public class BarUI : MonoBehaviour
{
    public StatType statType;
    [SerializeField] private Image fillImage;
    private const float MIN_VISIBLE_FILL = 0.05f;

    // Gelen 0.0 ile 1.0 arasindaki yuzdelik degeri direkt ekrana yansitiyoruz.
    public void SetFill(float normalizedValue)
    {
        float newValue = Mathf.Clamp01(normalizedValue);

        // Barin tamamen bos gorunmemesi icin minik bir gorunurluk payi
        if (newValue > 0f && newValue < MIN_VISIBLE_FILL)
            newValue = MIN_VISIBLE_FILL;

        fillImage.fillAmount = newValue;
    }
}