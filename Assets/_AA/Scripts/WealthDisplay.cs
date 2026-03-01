using TMPro;
using UnityEngine;

public class WealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wealthText;

    // KingdomManager içindeki maxStatValue (100) ile eşleşmeli
     private int maxWealthValue = 100;

    private void OnEnable()
    {
        GameEvents.StatChanged += OnStatChanged;
    }

    private void OnDisable()
    {
        GameEvents.StatChanged -= OnStatChanged;
    }

    private void OnStatChanged(StatType type, float normalizedValue)
    {
        if (type == StatType.Wealth)
        {
            // KingdomManager'dan 0.0 ile 1.0 arasında bir değer (normalizedValue) geliyor.
            // Gerçek tam sayıyı elde etmek için maksimum değer ile çarpıyoruz.
            int currentWealth = Mathf.RoundToInt(normalizedValue * maxWealthValue);

            // Ekrana yazdır
            wealthText.text = currentWealth.ToString();
        }
    }
}
