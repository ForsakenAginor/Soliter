using Assets.Source.Model;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Source.View
{
    [RequireComponent(typeof(CardMover))]
    [RequireComponent(typeof(ErrorEffect))]
    [RequireComponent(typeof(CardOpener))]
    public class ViewManager : MonoBehaviour, ICardView
    {
        [SerializeField] private TextMeshProUGUI _suitTextField;
        [SerializeField] private TextMeshProUGUI _valueTextField;

        private Vector3 _basePosition;
        private CardOpener _opener;
        private CardMover _mover;
        private ErrorEffect _errorEffect;

        private void Awake()
        {
            _errorEffect = GetComponent<ErrorEffect>();
            _opener = GetComponent<CardOpener>();
            _mover = GetComponent<CardMover>();
        }

        public void Init(Card card, Vector3 basePosition)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            _suitTextField.text = card.Suit.ToString();
            _valueTextField.text = card.Value.ToString();
            _basePosition = basePosition;
        }

        public void BecameVisible() => _opener.Open();

        public void BecameInvisible() => _opener.Close();

        public void GoToBase() => _mover.Move(_basePosition);

        public void PlayErrorEffect() => _errorEffect.Play();

        public void CancelMove() => _mover.CancelMove();        
    }
}