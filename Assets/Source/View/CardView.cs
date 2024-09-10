using Assets.Source.Model;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.View
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _suitTextField;
        [SerializeField] private TextMeshProUGUI _valueTextField;
        [SerializeField] private GameObject _face;
        [SerializeField] private GameObject _back;
        [SerializeField] private float _animationDuration = 1.0f;
        [SerializeField] private Image _background;

        private Vector3 _basePosition;

        public bool IsVisible => _face.activeSelf;

        public void Init(Card card, Vector3 basePosition)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            _suitTextField.text = card.Suit.ToString();
            _valueTextField.text = card.Value.ToString();
            _basePosition = basePosition;
        }

        public void BecameVisible()
        {
            _face.SetActive(true);
            _back.SetActive(false);
        }

        public void GoToBase()
        {
            transform.DOMove(_basePosition, _animationDuration);
        }

        public void PlayErrorEffect()
        {
            Color color = Color.red;
            int loops = 2;
            float duration = 0.2f;
            _background.DOColor(color, duration).SetLoops(loops, LoopType.Yoyo);
        }
    }
}