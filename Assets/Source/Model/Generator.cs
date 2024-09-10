using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Source.Model
{
    public class Generator
    {
        private const int CardsAmount = 40;

        private readonly int _minCombination = 2;
        private readonly int _maxCombination = 7;
        private readonly int _upperChance = 65;
        private readonly int _lowerChance = 35;
        private readonly int _rotateChance = 15;

        public int DeckSize => CardsAmount;

        public List<Combination> CreateCombinations()
        {
            List<Combination> combinations = new();

            int combinationLength;
            int maximumCombinationSize;
            int remainingCards = CardsAmount;
            Card card;

            while (remainingCards > 0)
            {
                maximumCombinationSize = Math.Min(remainingCards, _maxCombination) + 1;
                combinationLength = UnityEngine.Random.Range(_minCombination, maximumCombinationSize);

                card = CreateRandomCard();
                Combination combination = CreateCombination(combinationLength, card);
                combinations.Add(combination);

                remainingCards -= combination.Cards.Count();
            }

            return combinations;
        }

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

        private Combination CreateCombination(int size, Card firstCard)
        {
            List<Card> cards = new List<Card>();

            int seed = UnityEngine.Random.Range(0, _upperChance + _lowerChance);
            bool isUp = seed >= _lowerChance;
            Card card = firstCard;

            for (int i = size - 1; i > 0; i--)
            {
                card = CreatePreviousCard(card, isUp);
                cards.Add(card);
                seed = UnityEngine.Random.Range(0, _upperChance + _lowerChance);
                isUp = seed < _rotateChance ? !isUp : isUp;
            }

            return new(firstCard, cards);
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
}