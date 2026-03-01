using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Card))]
public class CardSwipper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Card _card;

    private Vector3 _centerPosition;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    [Header("Swipe Settings")]
    [SerializeField] private float swipeThreshold = 150f;
    [SerializeField] private float rotationMultiplier = 15f;

    [Header("Animation Settings")]
    [SerializeField] private float snapBackDuration = 0.3f;
    [SerializeField] private float flyOffDuration = 0.4f;
    [SerializeField] private float flyOffDistance = 1000f;

    [Header("Idle Floating Animation")]
    [Tooltip("Kart bosta dururken ne kadar asagi inip cikacak")]
    [SerializeField] private float idleFloatAmount = 15f;
    [Tooltip("Bir asagi-yukari salinim ne kadar surecek")]
    [SerializeField] private float idleFloatDuration = 1.5f;

    private bool _isAnimating = false;

    private void Start()
    {
        _card = GetComponent<Card>();
        _centerPosition = transform.localPosition;

        StartIdleAnimation();
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;

        // Butondan gelen Hapse At sinyalini dinlemeye basla
        GameEvents.ImprisonButtonClicked += OnImprisonButtonClicked;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
        GameEvents.ImprisonButtonClicked -= OnImprisonButtonClicked;
    }

    // Butona basildigi sinyali gelince tetiklenen metot
    private void OnImprisonButtonClicked()
    {
        // Eger kart zaten baska bir yere firlatiliyorsa cakisip bug olmamasi icin iptal et
        if (_isAnimating) return;

        // Surtunme veya mouse kullanmadan direkt SwipeDown (Asagi atma) fonksiyonunu calistir
        _startPosition = transform.localPosition; // Nerede duruyorsa oradan asagi ucsun
        SwipeDown();
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    private void OnGameOver(bool isWin)
    {
        _isAnimating = true;
        transform.DOKill();
    }

    private void StartIdleAnimation()
    {
        transform.DOKill();

        transform.DOLocalMoveY(_centerPosition.y - idleFloatAmount, idleFloatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        transform.DOKill();

        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        transform.localScale = Vector3.one;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        Vector3 currentPosition = transform.localPosition;
        currentPosition.x += eventData.delta.x;
        currentPosition.y += eventData.delta.y;
        transform.localPosition = currentPosition;

        float differenceX = transform.localPosition.x - _startPosition.x;
        float rotationZ = -(differenceX / swipeThreshold) * rotationMultiplier;
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        float differenceX = transform.localPosition.x - _startPosition.x;
        float differenceY = transform.localPosition.y - _startPosition.y;

        // Artik asagi fiziksel kaydirma devre disi, sadece saga ve sola kaydirma var.
        // Hapse atma islemi sadece UI butonundan yapilacak.
        if (differenceX > swipeThreshold)
        {
            SwipeRight();
        }
        else if (differenceX < -swipeThreshold)
        {
            SwipeLeft();
        }
        else
        {
            ResetCardPosition();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Bos
    }

    private void ResetCardPosition()
    {
        transform.DOKill();

        transform.DOLocalMove(_centerPosition, snapBackDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => StartIdleAnimation());

        transform.DOLocalRotateQuaternion(Quaternion.identity, snapBackDuration)
            .SetEase(Ease.OutBack);
    }

    private void SwipeRight()
    {
        _isAnimating = true;
        PlaySwipeSound();

        transform.DOLocalMoveX(_startPosition.x + flyOffDistance, flyOffDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                GameEvents.CardSwiped?.Invoke(SwipeDirection.Right, _card.cardData);
                Destroy(gameObject);
            });
    }

    private void SwipeLeft()
    {
        _isAnimating = true;
        PlaySwipeSound();

        transform.DOLocalMoveX(_startPosition.x - flyOffDistance, flyOffDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                GameEvents.CardSwiped?.Invoke(SwipeDirection.Left, _card.cardData);
                Destroy(gameObject);
            });
    }

    private void SwipeDown()
    {
        _isAnimating = true;

        // DEĞİŞTİRİLDİ: Standart kaydirma sesi yerine hapse atma sesini caliyoruz
        PlayImprisonSound();

        Debug.Log("Card Imprisoned via Button! (Karakter butona basilarak hapse atildi)");

        // Karti asagi (zindana) firlat
        transform.DOLocalMoveY(_startPosition.y - flyOffDistance, flyOffDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                GameEvents.CardSwiped?.Invoke(SwipeDirection.Down, _card.cardData);
                Destroy(gameObject);
            });
    }

    private void PlaySwipeSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.swipeSound, true);
        }
    }

    // YENI EKLENDI: Sadece hapse atma (Imprison) eylemi icin ozel ses metodu
    private void PlayImprisonSound()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.imprisonSound != null)
        {
            AudioManager.Instance.PlayPunishmentSFX(AudioManager.Instance.imprisonSound);
        }
    }
}