using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject _infoPanel;
    [SerializeField] private GameObject _infoImage;
    [SerializeField] private TMP_Text _infoText;
    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }
    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool obj)
    {
        if (obj) 
        {
            _infoPanel.SetActive(true);
            _infoImage.SetActive(true);
            _infoText.text = "You win";
            Image image = _infoPanel.GetComponent<Image>();
            image.color = Color.yellow;
            Color tempColor = image.color;
            tempColor.a = 0.5f;
            image.color = tempColor;
        }
        else
        {
            _infoPanel.SetActive(true);
            _infoImage.SetActive(true);
            _infoText.text = "You lose";
            Image image = _infoPanel.GetComponent<Image>();
            image.color = Color.red;
            Color tempColor = image.color;
            tempColor.a = 0.5f;
            image.color = tempColor;
        }
    }
}
