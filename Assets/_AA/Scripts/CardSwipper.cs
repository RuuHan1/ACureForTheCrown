using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening; // Gerekli DOTween kutuphanesi eklendi

// Bu scriptin calismasi icin objede kesinlikle "Card" componenti olmasini zorunlu kilar.
// Bu sayede baska bir objeye yanlislikla atarsan Unity seni uyarir. (Defensive Programming)
[RequireComponent(typeof(Card))]
public class CardSwipper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Card _card;

    // Baslangic pozisyonu ve rotasyonunu tutmak icin degiskenler
    private Vector2 _startPosition;
    private Quaternion _startRotation;

    [Header("Swipe Settings")]
    [SerializeField] private float swipeThreshold = 150f; // Saga veya sola kaydirma sayilmasi icin gereken mesafe
    [SerializeField] private float rotationMultiplier = 15f; // Suruklerken kartin ne kadar donecegi (Tinder hissi icin)

    [Header("Animation Settings")]
    [SerializeField] private float snapBackDuration = 0.3f; // Merkeze donus hizi
    [SerializeField] private float flyOffDuration = 0.4f; // Ekrandan cikis hizi
    [SerializeField] private float flyOffDistance = 1000f; // Ekrandan cikarken gidecegi mesafe

    private bool _isAnimating = false; // Animasyon sirasinda mudehaleyi engellemek icin state kontrolu

    private void Start()
    {
        _card = GetComponent<Card>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isAnimating) return; // Kart animasyon halindeyse suruklemeye izin verme

        // Eger onceden kalan bir tween varsa (cok hizli tiklamalarda) durdur
        transform.DOKill();

        // Surukleme basladiginda kartin ilk konumunu ve acisini kaydediyoruz.
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isAnimating) return; // Kart animasyon halindeyse suruklemeye izin verme

        // Sadece X ekseninde hareket etmesini sagliyoruz. Y ve Z eksenleri sabit kaliyor.
        Vector3 currentPosition = transform.localPosition;
        currentPosition.x += eventData.delta.x;
        transform.localPosition = currentPosition;

        // Daha iyi bir oyun hissi (Game Feel) icin karti X eksenindeki hareketine gore hafifce donduruyoruz.
        float differenceX = transform.localPosition.x - _startPosition.x;
        float rotationZ = -(differenceX / swipeThreshold) * rotationMultiplier;
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isAnimating) return; // Kart animasyon halindeyse islem yapma

        // Surukleme bittiginde (mouse/parmak kaldirildiginda) kartin ne kadar kaydirildigini hesapla.
        float differenceX = transform.localPosition.x - _startPosition.x;

        // Eger sag tarafa belirledigimiz sinirdan (threshold) fazla kaydirildiysa
        if (differenceX > swipeThreshold)
        {
            SwipeRight();
        }
        // Eger sol tarafa belirledigimiz sinirdan (threshold) fazla kaydirildiysa
        else if (differenceX < -swipeThreshold)
        {
            SwipeLeft();
        }
        // Yeterince kaydirilmadiysa karti eski yerine geri dondur.
        else
        {
            ResetCardPosition();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Sadece tiklayarak test etmek istersen burayi aktif tutabilirsin,
        // ancak normal bir swipe oyununda tiklama genelde swipe islevini tetiklemez.
        // Eger sadece surukleme istiyorsan icini bosaltabilirsin.
    }

    private void ResetCardPosition()
    {
        // DOTween ile yumusak (smooth) bir sekilde eski konumuna ve acisina dondur.
        // Ease.OutBack kartin yerine otururken hafifce esnemesini saglar (Game Feel)
        transform.DOLocalMove(_startPosition, snapBackDuration).SetEase(Ease.OutBack);
        transform.DOLocalRotateQuaternion(_startRotation, snapBackDuration).SetEase(Ease.OutBack);
    }

    private void SwipeRight()
    {
        Debug.Log("Swiped right");
        _isAnimating = true;

        // Karti saga dogru firlat, animasyon bitince event'i tetikle ve objeyi yok et
        transform.DOLocalMoveX(_startPosition.x + flyOffDistance, flyOffDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                GameEvents.CardSwiped?.Invoke(true, _card.cardData);
                Destroy(gameObject);
            });
    }

    private void SwipeLeft()
    {
        Debug.Log("Swiped left");
        _isAnimating = true;

        // Karti sola dogru firlat, animasyon bitince event'i tetikle ve objeyi yok et
        transform.DOLocalMoveX(_startPosition.x - flyOffDistance, flyOffDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                GameEvents.CardSwiped?.Invoke(false, _card.cardData);
                Destroy(gameObject);
            });
    }
}