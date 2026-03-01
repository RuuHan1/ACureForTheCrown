using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [HideInInspector] public CardSO cardData;

    [Header("UI elemnts")]
    [SerializeField] private Image _bgImage;
    [SerializeField] private Image _fgImage;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _suggestionText;


    public void Setup(CardSO cardData)
    {
        this.cardData = cardData;
        _fgImage.sprite = cardData.ArtWork;
        _titleText.text = cardData.Title;
        _suggestionText.text = cardData.Suggestion;
    
    }
}
