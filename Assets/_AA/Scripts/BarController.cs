using System.Collections.Generic;
using UnityEngine;

// UI YONETICISI
public class BarController : MonoBehaviour
{
    [SerializeField] private List<BarUI> barList = new();
    private Dictionary<StatType, BarUI> bars = new();

    private void Awake()
    {
        bars = new Dictionary<StatType, BarUI>();
        foreach (var bar in barList)
        {
            if (bars.ContainsKey(bar.statType)) continue;
            bars.Add(bar.statType, bar);
        }
    }

    private void OnEnable()
    {
        // ARTIK KART KAYDIRILDIGINDA DEGIL, KINGDOM MANAGER "STAT DEGISTI" DEDIGINDE CALISIYOR
        GameEvents.StatChanged += OnStatChanged;
    }

    private void OnDisable()
    {
        GameEvents.StatChanged -= OnStatChanged;
    }

    private void OnStatChanged(StatType type, float normalizedValue)
    {
        // Eger o stat tipinde bir UI barimiz varsa, degerini (yuzdesini) guncelle
        if (bars.TryGetValue(type, out BarUI bar))
        {
            bar.SetFill(normalizedValue);
        }
    }
}