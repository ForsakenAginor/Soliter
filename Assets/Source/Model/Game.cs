using Assets.Source.Controller;
using Assets.Source.View;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Model
{
    public class Game
    {
        private readonly List<Card> _solvedCards = new();
        private readonly Dictionary<CardController, CardsPair> _dictionary = new();
        private readonly Dictionary<CardController, Card> _bankCards = new();
        private readonly Generator _generator;
        private readonly Vector3 _basePosion;

        private Card _baseCard;
        private GameObject _baseCardGameObject;

        public Game(List<List<CardController>> cardControllers, List<CardController> bankCards, Vector3 basePosition)
        {
            _generator = new Generator();
            _basePosion = basePosition;

            List<Combination> combinations = _generator.CreateCombinations();
            InitializeBank(combinations, bankCards);
            ShuffleCards(combinations, cardControllers);
        }

        ~Game()
        {
            foreach (var key in _dictionary.Keys)
                key.OnCardClick -= OnCardClicked;
        }

        public event Action PlayerWon;

        private void ShuffleCards(List<Combination> combinations, List<List<CardController>> cardControllers)
        {
            Dictionary<int, List<CardController>> columnFillness = new()
        {
            { 0, new List<CardController>()},
            { 1, new List<CardController>()},
            { 2, new List<CardController>()},
            { 3, new List<CardController>()},
        };

            int columnSize = cardControllers[0].Count;
            int columnAmount = cardControllers.Count;
            int column;
            CardController controller;
            CardsPair pair;

            foreach (var combination in combinations)
            {
                List<Card> cards = combination.Cards.ToList();

                for (int i = cards.Count - 1; i >= 0; i--)
                {
                    bool isAdded = false;

                    while (isAdded == false)
                    {
                        column = UnityEngine.Random.Range(0, columnAmount);

                        if (cardControllers[column].Count == columnSize)
                        {
                            controller = cardControllers[column][0];
                            controller.GetComponent<CardView>().Init(cards[i], _basePosion);
                            controller.OnCardClick += OnCardClicked;
                            cardControllers[column].Remove(controller);
                            pair = new(cards[i]);
                            _dictionary.Add(controller, pair);
                            columnFillness[column].Add(controller);
                            isAdded = true;
                        }
                        else
                        {
                            if (cardControllers[column].Count > 0)
                            {
                                controller = cardControllers[column][0];
                                controller.GetComponent<CardView>().Init(cards[i], _basePosion);
                                controller.OnCardClick += OnCardClicked;
                                cardControllers[column].Remove(controller);
                                pair = new(cards[i], columnFillness[column].Last());
                                _dictionary.Add(controller, pair);
                                columnFillness[column].Add(controller);

                                if (cardControllers[column].Count == 0)
                                {
                                    controller.BecameActive();
                                    controller.GetComponent<CardView>().BecameVisible();
                                }

                                isAdded = true;
                            }
                        }
                    }
                }
            }
        }

        private void InitializeBank(List<Combination> combinations, List<CardController> bankCards)
        {
            List<CardView> cardViews = bankCards.ToList().Select(o => o.GetComponent<CardView>()).ToList();
            int lastCardIndex = combinations.Count - 1;

            for (int i = 0; i < combinations.Count; i++)
            {
                cardViews[i].Init(combinations[i].BankCard, _basePosion);
                bankCards[i].OnCardClick += OnBankCardClicked;
                _bankCards.Add(bankCards[i], combinations[i].BankCard);
            }

            cardViews[lastCardIndex].BecameVisible();
            cardViews[lastCardIndex].GoToBase();
            _baseCardGameObject = cardViews[lastCardIndex].gameObject;
            _baseCard = combinations[lastCardIndex].BankCard;

            bankCards[lastCardIndex - 1].BecameActive();

            for (int i = combinations.Count; i < bankCards.Count; i++)
                bankCards[i].gameObject.SetActive(false);

            bankCards[lastCardIndex].OnCardClick -= OnBankCardClicked;
            _bankCards.Remove(bankCards[lastCardIndex]);
        }

        private void OnBankCardClicked(CardController controller)
        {
            _baseCardGameObject.SetActive(false);
            _baseCardGameObject = controller.gameObject;
            controller.BecameInactive();
            CardView cardView = controller.GetComponent<CardView>();
            cardView.BecameVisible();
            cardView.GoToBase();
            controller.OnCardClick -= OnBankCardClicked;
            _baseCard = _bankCards[controller];
            _bankCards.Remove(controller);
            _bankCards.Keys.Last().BecameActive();
        }

        private void OnCardClicked(CardController controller)
        {
            Card card = _dictionary[controller].Card;

            if (card.CanMove(_baseCard) == false)
            {
                controller.GetComponent<CardView>().PlayErrorEffect();
                return;
            }

            controller.BecameInactive();
            controller.GetComponent<CardView>().GoToBase();
            _baseCardGameObject.SetActive(false);
            _baseCardGameObject = controller.gameObject;
            _solvedCards.Add(card);
            _baseCard = card;

            CardController descendant = _dictionary[controller].DescendantCard;

            if (descendant != null)
            {
                _dictionary[controller].DescendantCard.BecameActive();
                _dictionary[controller].DescendantCard.GetComponent<CardView>().BecameVisible();
            }

            if (_generator.DeckSize == _solvedCards.Count)
                PlayerWon?.Invoke();
        }

        private class CardsPair
        {
            private readonly Card _card;
            private readonly CardController _descendantCard;

            public CardsPair(Card card, CardController descendant = null)
            {
                _card = card != null ? card : throw new ArgumentNullException(nameof(card));
                _descendantCard = descendant;
            }

            public Card Card => _card;

            public CardController DescendantCard => _descendantCard;
        }
    }
}