using System.Collections.Generic;
using UnityEngine;

// OYUNUN BEYNI (Model & Controller) - Tum mantik, hesaplamalar ve rastgelelik burada
public class KingdomManager : MonoBehaviour
{
    public static KingdomManager Instance { get; private set; }

    [Header("Starting Stat Bounds")]
    [SerializeField] private int minStartValue = 40;
    [SerializeField] private int maxStartValue = 60;

    [Header("Stat Limits")]
    [SerializeField] private int maxStatValue = 100; // Statlarin ulasabilecegi maksimum deger

    private Dictionary<StatType, int> _currentStats = new Dictionary<StatType, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeStats();
    }

    private void Start()
    {
        // Oyun ilk basladiginda tum arayuzu (UI) bu rastgele 40-60 arasi degerlerle dolduruyoruz
        foreach (var stat in _currentStats)
        {
            UpdateUI(stat.Key);
        }
    }

    private void OnEnable()
    {
        GameEvents.CardSwiped += OnCardSwiped;
    }

    private void OnDisable()
    {
        GameEvents.CardSwiped -= OnCardSwiped;
    }

    private void InitializeStats()
    {
        // Her stat icin verilen aralikta (Orn: 40-60) rastgele baslangic degeri belirliyoruz
        _currentStats[StatType.Cancer] = Random.Range(minStartValue, maxStartValue + 1);
        _currentStats[StatType.MentalHealth] = Random.Range(minStartValue, maxStartValue + 1);
        _currentStats[StatType.ImmuneSystem] = Random.Range(minStartValue, maxStartValue + 1);
        _currentStats[StatType.Wealth] = Random.Range(minStartValue, maxStartValue + 1);
        _currentStats[StatType.Honor] = Random.Range(minStartValue, maxStartValue + 1);
    }

    private void OnCardSwiped(SwipeDirection direction, CardSO cardData)
    {
        IReadOnlyList<StatEffect> effects = null;

        switch (direction)
        {
            case SwipeDirection.Right:
                effects = cardData.RightChoice.Effects;
                break;
            case SwipeDirection.Left:
                effects = cardData.LeftChoice.Effects;
                break;
            case SwipeDirection.Down:
                effects = cardData.DownChioce.Effects;
                break;
        }

        if (effects != null) ApplyEffects(effects);
    }

    private void ApplyEffects(IReadOnlyList<StatEffect> effects)
    {
        foreach (var effect in effects)
        {
            if (_currentStats.ContainsKey(effect.Stat))
            {
                // ONEMLI NOT: Senin CardSO icindeki statlar -3 ile +3 arasindaydi.
                // Biz sistemi 100 uzerinden (max 100) kurguladigimiz icin bu kucuk sayilari
                // 10 ile carparak (-30, +30) buyuk bir etki yaratmasini sagliyoruz.
                int changeAmount = effect.Amount * 10;

                _currentStats[effect.Stat] += changeAmount;

                // Degeri 0 ile 100 arasinda disari tasmayacak sekilde kelepcele (Clamp)
                _currentStats[effect.Stat] = Mathf.Clamp(_currentStats[effect.Stat], 0, maxStatValue);

                // Matematik bitti, arayuzu guncelle
                UpdateUI(effect.Stat);

                // Oyunun bitip bitmedigini kontrol et
                CheckGameOverConditions(effect.Stat, _currentStats[effect.Stat]);
            }
        }
    }

    private void UpdateUI(StatType type)
    {
        // Eger guncel stat 45 ise, 45 / 100f = 0.45f yuzdesini UI'a yolluyoruz.
        float normalizedValue = (float)_currentStats[type] / maxStatValue;
        GameEvents.StatChanged?.Invoke(type, normalizedValue);
    }

    private void CheckGameOverConditions(StatType type, int currentValue)
    {
        // BarUI'in icinde yazan eski kurallarini buraya, asil olmasi gereken yere tasidik
        if (type == StatType.Cancer)
        {
            if (currentValue <= 0)
            {
                Debug.Log("Kanser tamamen bitti, kazandin!");
                GameEvents.GameOver?.Invoke(true); // You Win
            }
            else if (currentValue >= maxStatValue)
            {
                Debug.Log("Kanser vucudu ele gecirdi, kaybettin!");
                GameEvents.GameOver?.Invoke(false); // You Lose
            }
        }
        else // Diger statlar
        {
            if (currentValue <= 0)
            {
                Debug.Log($"Oyun bitti! Tuketilen stat: {type}");
                GameEvents.GameOver?.Invoke(false); // You Lose
            }
        }
    }
}