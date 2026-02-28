using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/New Card")]
public class CardSO : ScriptableObject
{
    [Header("Visual")]
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public string Suggestion { get; private set; }
    [field: SerializeField] public Sprite ArtWork { get; private set; }


    [Header("Choices")]
    [SerializeField] private SwipeChoice rightChoice;
    [SerializeField] private SwipeChoice leftChoice;
    [SerializeField] private SwipeChoice downChoice;


    public SwipeChoice LeftChoice => leftChoice;
    public SwipeChoice RightChoice => rightChoice;

    public SwipeChoice DownChioce => downChoice;

}

[System.Serializable]
public struct SwipeChoice
{
    [SerializeField] private List<StatEffect> effects;
    public IReadOnlyList<StatEffect> Effects => effects;


}

[System.Serializable]
public struct StatEffect
{
    public StatType Stat;
    [Range(-3,3)]
    public int Amount;
}
public enum StatType
{
    Cancer,
    MentalHealth,
    ImmuneSystem,
    Wealth,
    Honor,
}
public enum SwipeDirection
{
    Right,
    Left,
    Down
}