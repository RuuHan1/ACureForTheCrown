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
    private TMP_Text _priceText;

    public void Setup(CardSO cardData)
    {
        this.cardData = cardData;
        _fgImage.sprite = cardData.ArtWork;
        _titleText.text = cardData.Title;
        _suggestionText.text = cardData.Suggestion;
        _priceText.text = "";
        foreach (var effect in cardData.RightChoice.Effects)
        {
            if (effect.Stat == StatType.Wealth)
            {
                _priceText.text =( effect.Amount *10).ToString();
                break; // İlgili stat bulunduktan sonra döngüye devam etmeye gerek yok
            }
        }
    }
}
