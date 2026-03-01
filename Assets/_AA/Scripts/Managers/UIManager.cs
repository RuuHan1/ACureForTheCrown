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

    [Header("Texts")]
    // YENI: Oyunun sonucunu "You Win" / "You Lose" olarak gosterecek baslik metni
    [SerializeField] private TMP_Text _endingHeader;
    [SerializeField] private TMP_Text _infoText; // Hikaye detayini gosterecek metin

    [Header("Images")]
    [Tooltip("Arka plani karartacak/renklendirecek olan Image bileseni")]
    [SerializeField] private Image _panelImage;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _imprisonButton;

    // PERFORMAS TAVSİYESİ (Caching)
    private Image _infoImageComponent;

    private void Awake()
    {
        // Buton tiklanma eventini kod uzerinden bagliyoruz
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(RestartGame);
        }

        // Imprison butonunu GameEvents sinyaline bagliyoruz
        if (_imprisonButton != null)
        {
            _imprisonButton.onClick.AddListener(() => GameEvents.ImprisonButtonClicked?.Invoke());
        }

        // infoImage uzerindeki Image bilesenini hafizaya al
        if (_infoImage != null)
        {
            _infoImageComponent = _infoImage.GetComponent<Image>();
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

    private void OnGameOver(bool isWin, string message)
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

        // 2. Kazanma ve Kaybetme Durumu Icin Renk Ayarlari ve Baslik Metni
        Color targetColor;
        Color popupBackgroundColor;

        if (isWin)
        {
            // YENI: Basligi ayarla
            if (_endingHeader != null) _endingHeader.text = "You Win";

            targetColor = new Color(161f / 255f, 144f / 255f, 37f / 255f);
            popupBackgroundColor = new Color(1f, 0.9f, 0.5f);
        }
        else
        {
            // YENI: Basligi ayarla
            if (_endingHeader != null) _endingHeader.text = "You Lose";

            targetColor = Color.red;
            popupBackgroundColor = new Color(1f, 0.7f, 0.7f);
        }

        // Ekrana KingdomManager'dan gelen hikayeli aciklama metnini basiyoruz
        if (_infoText != null) _infoText.text = message;

        // infoImage'in (ortadaki panelin) arka plan rengini degistir
        if (_infoImageComponent != null)
        {
            _infoImageComponent.color = popupBackgroundColor;
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