using System;
using System.Collections.Generic;

public class Generator
{
    private const int CardsAmount = 11;

    private readonly int _minCombination = 2;
    private readonly int _maxCombination = 7;
    private readonly int _upperChance = 65;
    private readonly int _lowerChance = 35;
    private readonly int _rotateChance = 15;
    private readonly List<Card> _cards = new List<Card>();

    public Generator()
    {
        int combination;
        int remainingSlots;
        int maximumCombinationSize;
        Card card;

        while (_cards.Count < CardsAmount)
        {
            maximumCombinationSize = Math.Min(CardsAmount - _cards.Count, _maxCombination) +1;
            combination = UnityEngine.Random.Range(_minCombination, maximumCombinationSize);
            remainingSlots = CardsAmount - _cards.Count - combination;

            while (remainingSlots < _minCombination && remainingSlots > 0)
            {
                combination = UnityEngine.Random.Range(_minCombination, maximumCombinationSize);
                remainingSlots = CardsAmount - _cards.Count - combination;
            }

            card = CreateRandomCard();
            _cards.Add(card);
            CreateCombination(combination, card);
        }
    }

    public IEnumerable<Card> Cards => _cards;


    private Card CreateRandomCard()
    {
        Values value = (Values)UnityEngine.Random.Range(1, Enum.GetValues(typeof(Values)).Length);
        Suits suit = (Suits)UnityEngine.Random.Range(1, Enum.GetValues(typeof(Suits)).Length);
        Card card = new Card(suit, value);
        return card;
    }

    private Card CreateRandomCard(Values value)
    {
        Suits suit = (Suits)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Suits)).Length);
        Card card = new Card(suit, value);
        return card;
    }

    private void CreateCombination(int size, Card firstCard)
    {
        int seed = UnityEngine.Random.Range(0, _upperChance + _lowerChance);
        bool isUp = seed >= _lowerChance;
        Card card = firstCard;

        for (int i = size - 1; i > 0; i--)
        {
            card = CreatePreviousCard(card, isUp);
            _cards.Add(card);
            seed = UnityEngine.Random.Range(0, _upperChance + _lowerChance);
            isUp = seed < _rotateChance ? !isUp : isUp;
        }
    }

    private Card CreatePreviousCard(Card card, bool isUp)
    {
        if (isUp)
        {
            if (card.Value == Values.King)
                return CreateRandomCard(Values.Ace);
            else
                return CreateRandomCard((Values)((int)card.Value + 1));
        }

        if (card.Value == Values.Ace)
            return CreateRandomCard(Values.King);

        return CreateRandomCard((Values)((int)card.Value - 1));
    }
}