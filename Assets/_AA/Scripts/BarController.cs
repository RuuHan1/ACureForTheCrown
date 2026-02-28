using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    [SerializeField] private List<BarUI> barList = new();
    private Dictionary<StatType, BarUI> bars = new();

    private void Awake()
    {
        bars = new Dictionary<StatType, BarUI>();
        foreach (var bar in barList)
        {
            bars.Add(bar.statType, bar);
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
    private void OnCardSwiped(SwipeDirection direction, CardSO sO)
    {
        
        if (direction == SwipeDirection.Right)
        {
            foreach(StatEffect effect in sO.RightChoice.Effects)
            {
                if (bars.TryGetValue(effect.Stat, out BarUI bar))
                {
                    bar.UpdateValue(effect.Amount);
                }
            }
        }
        else if (direction == SwipeDirection.Left)
        {
            foreach (StatEffect effect in sO.LeftChoice.Effects)
            {
                if (bars.TryGetValue(effect.Stat, out BarUI bar))
                {
                    bar.UpdateValue(effect.Amount);
                }
            }
        }
        else
        {
            foreach (StatEffect effect in sO.DownChioce.Effects)
            {
                if (bars.TryGetValue(effect.Stat, out BarUI bar))
                {
                    bar.UpdateValue(effect.Amount);
                }
            }
        }
    }
}


