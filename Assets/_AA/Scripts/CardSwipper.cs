using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSwipper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    private Card _card;
    
    
    private void Start()
    {
        _card = this.GetComponent<Card>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SwipeRight();
    }

    private void SwipeRight()
    {
        

        Debug.Log("swiped right");
        GameEvents.CardSwiped?.Invoke(true,_card.cardData);
        Destroy(gameObject);

        
    }

    private void SwipeLeft()
    {
        GameEvents.CardSwiped?.Invoke(false,_card.cardData);
        Destroy(gameObject);
    }
}
