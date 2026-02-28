using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _infoImage;
    [SerializeField] private TMP_Text _infoText;

    [Tooltip("Arka plani karartacak/renklendirecek olan Image bileseni")]
    [SerializeField] private Image _panelImage;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;

    // YENI: Oyun ici Hapse At butonu buraya eklendi
    [SerializeField] private Button _imprisonButton;

    private void Awake()
    {
        // Buton tiklanma eventini kod uzerinden bagliyoruz
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(RestartGame);
        }

        // YENI: Imprison butonunu GameEvents sinyaline bagliyoruz
        if (_imprisonButton != null)
        {
            _imprisonButton.onClick.AddListener(() => GameEvents.ImprisonButtonClicked?.Invoke());
        }

        // Oyun basinda Game Over ekranini gizle
        _infoPanel.SetActive(false);
        _infoImage.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool isWin)
    {
        // Oyun bittiginde arkaplandaki Hapse At butonuna basilmasini engellemek icin kapatiyoruz
        if (_imprisonButton != null)
        {
            _imprisonButton.gameObject.SetActive(false);
        }

        _infoPanel.SetActive(true);
        _infoImage.SetActive(true);

        // 1. Popup Animasyonu
        _infoImage.transform.localScale = Vector3.zero;
        _infoImage.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f);

        // 2. Kazanma ve Kaybetme Durumu Icin Yazi ve Renk Ayarlari
        Color targetColor;

        if (isWin)
        {
            _infoText.text = "You win";
            targetColor = new Color(161f, 144f, 37f);
        }
        else
        {
            _infoText.text = "You lose";
            targetColor = Color.red;
        }

        // 3. Arka Plan Animasyonu
        if (_panelImage != null)
        {
            targetColor.a = 0f;
            _panelImage.color = targetColor;
            _panelImage.DOFade(0.5f, 1f);
        }
    }

    private void RestartGame()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}