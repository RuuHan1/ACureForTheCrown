using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Card))]
public class CardSwipper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Card _card;

    private Vector2 _startPosition;
    private Quaternion _startRotation;

    [Header("Swipe Settings")]
    [SerializeField] private float swipeThreshold = 150f;
    [SerializeField] private float rotationMultiplier = 15f;

    [Header("Animation Settings")]
    [SerializeField] private float snapBackDuration = 0.3f;
    [SerializeField] private float flyOffDuration = 0.4f;
    [SerializeField] private float flyOffDistance = 1000f;

    private bool _isAnimating = false;

    private void Start()
    {
        _card = GetComponent<Card>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        transform.DOKill();

        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        Vector3 currentPosition = transform.localPosition;
        currentPosition.x += eventData.delta.x;
        transform.localPosition = currentPosition;

        float differenceX = transform.localPosition.x - _startPosition.x;
        float rotationZ = -(differenceX / swipeThreshold) * rotationMultiplier;
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isAnimating) return;

        float differenceX = transform.localPosition.x - _startPosition.x;

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
        // Tiklama islevi bos birakildi
    }

    private void ResetCardPosition()
    {
        transform.DOLocalMove(_startPosition, snapBackDuration).SetEase(Ease.OutBack);
        transform.DOLocalRotateQuaternion(_startRotation, snapBackDuration).SetEase(Ease.OutBack);
    }

    private void SwipeRight()
    {
        _isAnimating = true;

        // TEKLI SESI CAL VE PITCH RASTGELELIGINI AKTIF ET
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.swipeSound, true);
        }

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

        // TEKLI SESI CAL VE PITCH RASTGELELIGINI AKTIF ET
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.swipeSound, true);
        }

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
        GameEvents.CardSwiped?.Invoke(SwipeDirection.Down, _card.cardData);
    }
}