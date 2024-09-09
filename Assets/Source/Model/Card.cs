using System;

public class Card
{
    private readonly Suits _suit;
    private readonly Values _value;

    public Card(Suits suit, Values value)
    {
        _suit = suit;
        _value = value;
    }

    public Values Value => _value;

    public Suits Suit => _suit;

    public bool CanMove(Card target)
    {
        if (MathF.Abs((int)Value - (int)target.Value) == 1)
            return true;

        if (Value == Values.Ace && target.Value == Values.King)
            return true;

        if (target.Value == Values.Ace && Value == Values.King)
            return true;

        return false;
    }
}