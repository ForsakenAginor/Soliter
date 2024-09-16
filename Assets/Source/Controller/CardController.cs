using Assets.Source.Model;
using Assets.Source.View;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Controller
{
    [RequireComponent(typeof(ViewManager))]
    public class CardController : MonoBehaviour, IPointerClickHandler, IController
    {
        private bool _isActive;
        private Vector3 _startPosition;
        private ViewManager _viewManager;

        public event Action<IClickable> OnClicked;

        public void Awake()
        {
            _startPosition = transform.position;
            _viewManager = GetComponent<ViewManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isActive)
                OnClicked?.Invoke(this);
        }

        public void BecameActive() => _isActive = true;

        public void BecameInactive() => _isActive = false;

        public void Disable() => gameObject.SetActive(false);

        public ICardView Init(Card card, Vector3 basePosition)
        {
            ViewManager view = GetComponent<ViewManager>();
            view.Init(card, basePosition);
            return view;
        }

        public void Normalize()
        {
            gameObject.SetActive(true);
            transform.position = _startPosition;
            _viewManager.BecameInvisible();
            _viewManager.CancelMove();
        }
    }
}