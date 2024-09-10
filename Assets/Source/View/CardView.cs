using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _suitTextField;
    [SerializeField] private TextMeshProUGUI _valueTextField;
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _back;
    [SerializeField] private float _animationDuration = 1.0f;

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
}
