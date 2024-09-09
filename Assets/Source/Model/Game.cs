using System;
using System.Collections.Generic;
using System.Linq;

public class Game
{
    private Card _baseCard;
    private Card _topCard;
    private List<Card> _cards;
    private CardAtTable _topCardAtTable;

    public Game(CardAtTable[] cardAtTables)
    {
        if(cardAtTables == null)
            throw new ArgumentNullException(nameof(cardAtTables));

        Generator generator = new Generator();
        _cards = generator.Cards.ToList();

        if(cardAtTables.Length == 0 || cardAtTables.Length != _cards.Count)
            throw new ArgumentOutOfRangeException(nameof(cardAtTables));

        for (int i = 0; i < cardAtTables.Length; i++)
        {
            cardAtTables[i].Init(_cards[i]);
            cardAtTables[i].GetComponent<CardView>().Init(_cards[i]);
            cardAtTables[i].OnCardClick += OnCardClicked;
        }
        /*
        _baseCard = _cards.Last();
        _cards.Remove(_baseCard);
        _topCard = _cards.Last();
        _cards.Remove(_topCard);*/
    }

    private void OnCardClicked(Card card)
    {
        throw new NotImplementedException();
    }
}

