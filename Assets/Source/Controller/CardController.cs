using Assets.Source.View;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Controller
{
    [RequireComponent(typeof(CardView))]
    public class CardController : MonoBehaviour, IPointerClickHandler
    {
        private bool _isActive;

        public event Action<CardController> OnCardClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isActive)
                OnCardClick?.Invoke(this);
        }

        public void BecameActive() => _isActive = true;

        public void BecameInactive() => _isActive = false;
    }
}