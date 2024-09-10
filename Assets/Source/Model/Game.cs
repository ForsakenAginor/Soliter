using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    private Card _baseCard;
    private List<Card> _cards;
    private List<CardController> _cardsControllers;
    private List<CardView> _cardViews;

    public Game(CardController[] cardAtTables)
    {
        if(cardAtTables == null)
            throw new ArgumentNullException(nameof(cardAtTables));

        Generator generator = new Generator();
        _cards = generator.CreateColumn();

        if(cardAtTables.Length == 0 || cardAtTables.Length != _cards.Count)
            throw new ArgumentOutOfRangeException(nameof(cardAtTables));

        _cardsControllers = cardAtTables.ToList();
        _cardViews = _cardsControllers.ToList().Select(o => o.GetComponent<CardView>()).ToList();

        int lastCardIndex = _cardsControllers.Count - 1;
        int previousCardIndex = lastCardIndex - 1;
        Vector3 basePosition = _cardsControllers[lastCardIndex].transform.position;

        for (int i = 0; i < lastCardIndex; i++)
        {
            _cardViews[i].Init(_cards[i], basePosition);
            _cardsControllers[i].OnCardClick += OnCardClicked;
        }

        _cardViews[previousCardIndex].BecameVisible();
        _cardsControllers[previousCardIndex].BecameActive();
;
        _cardViews[lastCardIndex].Init(_cards[lastCardIndex], basePosition);
        _cardViews[lastCardIndex].BecameVisible();
        
        _baseCard = _cards[lastCardIndex];
    }

    public event Action PlayerWon;

    private void OnCardClicked(CardController controller)
    {
        int index = _cardsControllers.IndexOf(controller);

        if (_cards[index].CanMove(_baseCard))
        {
            controller.BecameInactive();
            _cardViews[index].GoToBase();
            _cardViews[_cards.IndexOf(_baseCard)].gameObject.SetActive(false);
            _baseCard = _cards[index];

            if(index > 0)
            {
                index--;
                _cardViews[index].BecameVisible();
                _cardsControllers[index].BecameActive();
            }
            else
            {
                PlayerWon?.Invoke();
            }
        }
    }
}

