using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardSO> cardDatas;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private RectTransform _cardPanel;
    private void Awake()
    {
        GenerateCard();
    }

    private void OnEnable()
    {
        GameEvents.CardSwiped += OnCardSwiped;
    }

    private void OnCardSwiped(SwipeDirection direction, CardSO sO)
    {
        GenerateCard();
    }

    private void GenerateCard()
    {
        GameObject gameObj = Instantiate(_cardPrefab, _cardPanel);

        RectTransform rt = gameObj.GetComponent<RectTransform>();
        rt.localPosition = Vector3.one; 
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;

        Card card = gameObj.GetComponent<Card>();
        int dice = Random.Range(0, cardDatas.Count);
        card.Setup(cardDatas[dice]);
    }


}
