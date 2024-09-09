using System;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _suitTextField;
    [SerializeField] private TextMeshProUGUI _valueTextField;

    public void Init(Card card)
    {
        if (card == null)
            throw new ArgumentNullException(nameof(card));

        _suitTextField.text = card.Suit.ToString();
        _valueTextField.text = card.Value.ToString();
    }
}
