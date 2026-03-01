using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardSO> cardDatas; // Ana liste (bozulmaz)
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private RectTransform _cardPanel;

    private List<CardSO> _activeCards = new List<CardSO>(); // Tüketilen liste

    private void OnEnable()
    {
        GameEvents.CardSwiped += OnCardSwiped;
        GameEvents.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameEvents.CardSwiped -= OnCardSwiped;
        GameEvents.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        RefillDeck();
        GenerateCard();
    }

    private void OnCardSwiped(SwipeDirection direction, CardSO sO)
    {
        GenerateCard();
    }

    private void GenerateCard()
    {
        // Havuz boşsa yeniden doldur
        if (_activeCards.Count == 0)
        {
            RefillDeck();
        }

        GameObject gameObj = Instantiate(_cardPrefab, _cardPanel);

        RectTransform rt = gameObj.GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero; // Düzeltildi
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;

        Card card = gameObj.GetComponent<Card>();

        // Havuzdan rastgele kart seç
        int dice = Random.Range(0, _activeCards.Count);
        CardSO selectedCard = _activeCards[dice];

        // Seçilen kartı havuzdan çıkar
        _activeCards.RemoveAt(dice);

        card.Setup(selectedCard);
    }

    private void RefillDeck()
    {
        _activeCards.Clear();
        _activeCards.AddRange(cardDatas);
    }
}