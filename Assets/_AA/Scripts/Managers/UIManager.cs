using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Sahneyi yeniden yuklemek icin
using DG.Tweening; // Animasyonlar icin

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _infoImage;
    [SerializeField] private TMP_Text _infoText;

    // Caching yerine manuel atama icin SerializeField eklendi
    [Tooltip("Arka plani karartacak olan Image bileseni")]
    [SerializeField] private Image _panelImage;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;

    private void Awake()
    {
        // Buton tiklanma eventini kod uzerinden bagliyoruz
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(RestartGame);
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
        _infoPanel.SetActive(true);
        _infoImage.SetActive(true);

        // 1. Popup Animasyonu: _infoImage objesini kucukten buyuge yaylanarak (OutBack) ekrana cikar
        _infoImage.transform.localScale = Vector3.zero;
        _infoImage.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f);

        // 2. Kazanma ve Kaybetme Durumu Icin Yazi ve Renk Ayarlari
        Color targetColor;

        if (isWin)
        {
            _infoText.text = "You win";
            // Kazanma durumu icin altinsi/sari bir renk belirliyoruz
            targetColor = new Color(1f, 0.8f, 0f);
        }
        else
        {
            _infoText.text = "You lose";
            // Kaybetme durumu icin kirmizi renk belirliyoruz
            targetColor = Color.red;
        }

        // 3. Arka Plan Animasyonu: DOTween ile pruzsuzce karartma/renklendirme
        if (_panelImage != null)
        {
            // Renkleri ata ama once Alpha'yi (saydamligi) sifir yap ki yavasca karararak gelsin
            targetColor.a = 0f;
            _panelImage.color = targetColor;

            // 1 saniye icerisinde yavasca %50 seffafliga (0.5f) ulas (Fade In)
            _panelImage.DOFade(0.5f, 1f);
        }
    }

    private void RestartGame()
    {
        // Sahneyi yeniden yuklerken arkada takili kalan animasyonlari temizle (Memory Leak onlemi)
        DOTween.KillAll();

        // Aktif sahneyi (Mevcut leveli) bastan yukler
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}