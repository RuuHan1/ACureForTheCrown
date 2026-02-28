using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Card))]
public class CardSwipper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Card _card;

    private Vector3 _centerPosition; // Kartin ilk dogdugu kusursuz merkez (0,0,0)
    private Vector2 _startPosition; // Oyuncunun karti tuttugu anki pozisyon
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

        // Kartin dogdugu anki pozisyonunu merkez kabul ediyoruz
        _centerPosition = transform.localPosition;

        StartIdleAnimation();
    }

    private void StartIdleAnimation()
    {
        // Kartin merkezin ustune cikmasini istemedigimiz icin, 
        // merkezden 'idleFloatAmount' kadar asagiya inip tekrar merkeze cikmasini (Yoyo) sagliyoruz.
        // Ease.InOutSine secimi nefes alip verme (organik salinim) hissi yaratir.
        transform.DOLocalMoveY(_centerPosition.y - idleFloatAmount, idleFloatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        // Oyuncu karta dokundugu an salinim (floating) animasyonunu oldururuz
        transform.DOKill();

        // Kartin o an salinimda kaldigi yeri baslangic pozisyonu sayiyoruz
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        Vector3 currentPosition = transform.localPosition;
        currentPosition.x += eventData.delta.x;
        currentPosition.y += eventData.delta.y;

        // KARTIN YUKARI GITMESINI ENGELLEME: (Salinim sirasinda bug olmamasi icin _centerPosition baz alindi)
        currentPosition.y = Mathf.Min(currentPosition.y, _centerPosition.y);

        transform.localPosition = currentPosition;

        // Donme efekti
        float differenceX = transform.localPosition.x - _startPosition.x;
        float rotationZ = -(differenceX / swipeThreshold) * rotationMultiplier;
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        float differenceX = transform.localPosition.x - _startPosition.x;
        float differenceY = transform.localPosition.y - _startPosition.y;

        if (Mathf.Abs(differenceX) > Mathf.Abs(differenceY))
        {
            if (differenceX > swipeThreshold) SwipeRight();
            else if (differenceX < -swipeThreshold) SwipeLeft();
            else ResetCardPosition();
        }
        else
        {
            if (differenceY < -swipeThreshold) SwipeDown();
            else ResetCardPosition();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Tiklama islevi bos birakildi
    }

    private void ResetCardPosition()
    {
        // Oyuncu karti birakip vazgectiyse, karti dogdugu orijinal merkeze (_centerPosition) geri dondur.
        transform.DOLocalMove(_centerPosition, snapBackDuration).SetEase(Ease.OutBack)
            .OnComplete(StartIdleAnimation); // Kart tam yerine oturdugunda (OnComplete) tekrar süzülmeye (nefes almaya) baslasin.

        transform.DOLocalRotateQuaternion(Quaternion.identity, snapBackDuration).SetEase(Ease.OutBack);
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
        PlaySwipeSound();

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
}