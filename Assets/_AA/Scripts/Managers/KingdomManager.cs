using System.Collections.Generic;
using UnityEngine;

// THE BRAIN OF THE GAME (Model & Controller) - All logic, calculations, and randomness are handled here
public class KingdomManager : MonoBehaviour
{
    public static KingdomManager Instance { get; private set; }

    [Header("Starting Stat Bounds")]
    [SerializeField] private int minStartValue = 40;
    [SerializeField] private int maxStartValue = 60;

    [Header("Stat Limits")]
    [SerializeField] private int maxStatValue = 100; // The maximum value stats can reach

    private Dictionary<StatType, int> _currentStats = new Dictionary<StatType, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeStats();
    }

    private void Start()
    {
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
                int changeAmount = effect.Amount * 10;
                _currentStats[effect.Stat] += changeAmount;
                _currentStats[effect.Stat] = Mathf.Clamp(_currentStats[effect.Stat], 0, maxStatValue);

                UpdateUI(effect.Stat);
                CheckGameOverConditions(effect.Stat, _currentStats[effect.Stat]);
                if (effect.Stat == StatType.Cancer)
                {
                    int stage = CalculateCancerStage(_currentStats[StatType.Cancer]);
                    GameEvents.CancerStageChanged?.Invoke(stage);
                }
            }
        }
    }

    private void UpdateUI(StatType type)
    {
        float normalizedValue = (float)_currentStats[type] / maxStatValue;
        GameEvents.StatChanged?.Invoke(type, normalizedValue);
    }

    private void CheckGameOverConditions(StatType type, int currentValue)
    {
        if (type == StatType.Cancer)
        {
            if (currentValue <= 0)
            {
                string winMessage = "The cancer has been completely eradicated! The princess is saved, and the kingdom breathes a sigh of relief.";
                GameEvents.GameOver?.Invoke(true, winMessage);
            }
            else if (currentValue >= maxStatValue)
            {
                string loseMessage = "The cancer has consumed her entire body... The princess has passed away, and the kingdom is in mourning.";
                GameEvents.GameOver?.Invoke(false, loseMessage);
            }
        }
        else
        {
            // Generate story texts when other stats are depleted
            if (currentValue <= 0)
            {
                string loseMessage = "Game over!"; // Default

                switch (type)
                {
                    case StatType.MentalHealth:
                        loseMessage = "The princess's mental health has completely collapsed. Having lost the strength to rule, she is now trapped in darkness.";
                        break;
                    case StatType.ImmuneSystem:
                        loseMessage = "Her immune system has failed. Even a simple illness spreading through the palace was enough to defeat the princess...";
                        break;
                    case StatType.Wealth:
                        loseMessage = "The treasury is completely empty! Without wages, the soldiers rebelled and the palace was looted.";
                        break;
                    case StatType.Honor:
                        loseMessage = "The kingdom's honor has been trampled. The people no longer see you as a leader; you have been overthrown!";
                        break;
                }

                GameEvents.GameOver?.Invoke(false, loseMessage);
            }
        }
    }
    private int CalculateCancerStage(int value)
    {
        if (value <= 30) return 0;      // 0-30 arası (Hafif)
        if (value <= 70) return 1;      // 31-70 arası (Orta)
        return 2;                       // 71-100 arası (Ağır)
    }
}