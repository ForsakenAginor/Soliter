using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardView))]
public class CardAtTable : MonoBehaviour, IPointerClickHandler
{
    private Card _card;
    private bool _isActive;

    public event Action<Card> OnCardClick;

    public Card Card => _card;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardClick?.Invoke(_card);
    }

    public void Init(Card card)
    {
        _card = card != null ? card : throw new ArgumentNullException(nameof(card));
    }
}
