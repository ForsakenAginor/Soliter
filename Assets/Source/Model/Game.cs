using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    private readonly List<Card> _cards = new();
    private readonly List<Card> _solvedCards = new();
    private readonly List<CardController> _cardsControllers = new();
    private readonly List<CardView> _cardViews = new();
    private readonly Generator _generator;

    private Card _baseCard;

    public Game(List<CardController[]> cardsAtTable)
    {
        if (cardsAtTable == null)
            throw new ArgumentNullException(nameof(cardsAtTable));

        _generator = new Generator();

        foreach (var column in cardsAtTable)
            if (column.Length == 0 || column.Length != _generator.ColumnSize)
                throw new ArgumentOutOfRangeException(nameof(cardsAtTable));

        InitializeCards(cardsAtTable);
    }

    ~Game()
    {
        int lastCardIndex = _cardsControllers.Count - 1;

        for (int i = 0; i < lastCardIndex; i++)
        {
            try
            {
                _cardsControllers[i].OnCardClick -= OnCardClicked;
                _cardsControllers[i].OnCardClick -= OnBankCardClicked;
            }
            catch
            {
                continue;
            }
        }
    }

    public event Action PlayerWon;

    private void InitializeCards(List<CardController[]> cardsAtTable)
    {
        List<int> topDeckCardIndexes = new();
        List<int> bankCardIndexes = new();

        foreach (var column in cardsAtTable)
        {
            _cards.AddRange(_generator.CreateColumn());
            _cardsControllers.AddRange(column);
            _cardViews.AddRange(column.ToList().Select(o => o.GetComponent<CardView>()));
            topDeckCardIndexes.Add(_cards.Count - 2);
            bankCardIndexes.Add(_cards.Count - 1);
        }

        int lastCardIndex = _cardsControllers.Count - 1;
        Vector3 basePosition = _cardsControllers[lastCardIndex].transform.position;

        for (int i = 0; i < lastCardIndex; i++)
        {
            _cardViews[i].Init(_cards[i], basePosition);
            _cardsControllers[i].OnCardClick += OnCardClicked;
        }

        foreach (var topDeckCardIndex in topDeckCardIndexes)
        {
            _cardViews[topDeckCardIndex].BecameVisible();
            _cardsControllers[topDeckCardIndex].BecameActive();
        }

        for (int i = 0; i < bankCardIndexes.Count - 1; i++)
        {
            _cardsControllers[bankCardIndexes[i]].OnCardClick -= OnCardClicked;
            _cardsControllers[bankCardIndexes[i]].OnCardClick += OnBankCardClicked;
            _cardsControllers[bankCardIndexes[i]].BecameActive();
            _solvedCards.Add(_cards[bankCardIndexes[i]]);
        }

        _cardViews[lastCardIndex].Init(_cards[lastCardIndex], basePosition);
        _cardViews[lastCardIndex].BecameVisible();

        _baseCard = _cards[lastCardIndex];
        _solvedCards.Add(_baseCard);
    }

    private void OnBankCardClicked(CardController controller)
    {
        int index = _cardsControllers.IndexOf(controller);
        controller.BecameInactive();
        _cardViews[index].GoToBase();
        _cardViews[index].BecameVisible();
        _cardViews[_cards.IndexOf(_baseCard)].gameObject.SetActive(false);
        _baseCard = _cards[index];
    }

    private void OnCardClicked(CardController controller)
    {
        int index = _cardsControllers.IndexOf(controller);

        if (_cards[index].CanMove(_baseCard))
        {
            controller.BecameInactive();
            _cardViews[index].GoToBase();
            _cardViews[_cards.IndexOf(_baseCard)].gameObject.SetActive(false);
            _baseCard = _cards[index];
            _solvedCards.Add(_baseCard);

            if (index > 0)
            {
                index--;

                if (_solvedCards.Contains(_cards[index]) == false)
                {
                    _cardViews[index].BecameVisible();
                    _cardsControllers[index].BecameActive();
                }
            }

            if (_cards.Count == _solvedCards.Count)
                PlayerWon?.Invoke();
        }
    }
}