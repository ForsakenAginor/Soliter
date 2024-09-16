using Assets.Source.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Model
{
    public class Game
    {
        private readonly List<Card> _solvedCards = new();
        private readonly Dictionary<IController, CardsPair> _cardsAtTable = new();
        private readonly Dictionary<IController, Card> _bankCards = new();
        private readonly Vector3 _basePosion;
        private readonly Dictionary<Card, ICardView> _cardViews = new();
        private readonly int _deckSize;
        private readonly List<Card> _openedCards = new();
        private readonly List<Combination> _combinations;

        private BaseCard _baseCard;

        public Game(List<List<IController>> cardControllers,
                    List<IController> bankCards,
                    List<Combination> combinations,
                    Vector3 basePosition)
        {
            if(cardControllers == null)
                throw new ArgumentNullException(nameof(cardControllers));

            if(bankCards == null)
                throw new ArgumentNullException(nameof(bankCards));

            if(combinations == null)
                throw new ArgumentNullException(nameof(combinations));

            _basePosion = basePosition;
            _combinations = combinations;
            _ = new GameEventHandler(this);

            InitializeBank(_combinations, bankCards);
            ShuffleCards(_combinations, cardControllers);
            _deckSize = _cardsAtTable.Count;
        }

        ~Game()
        {
            Unsubscribe();
        }

        public event Action PlayerWon;
        public event Action PlayerLosed;
        public event Action<Card> CardOpened;
        public event Action<Card> CardMovedToBase;
        public event Action<Card> CardMoveFailed;

        public IReadOnlyDictionary<Card, ICardView> CardViews => _cardViews;

        /// <summary>
        /// Restart current level
        /// </summary>
        /// <param name="cardControllers"></param>
        /// <param name="bankCards"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Load(List<List<IController>> cardControllers, List<IController> bankCards)
        {
            if (cardControllers == null)
                throw new ArgumentNullException(nameof(cardControllers));

            if (bankCards == null)
                throw new ArgumentNullException(nameof(bankCards));

            //Clear records about previous try
            Unsubscribe();
            _bankCards.Clear();
            _solvedCards.Clear();
            _openedCards.Clear();

            foreach (var combination in _combinations)
                _cardViews.Remove(combination.BankCard);

            // Reinitialize bank
            InitializeBank(_combinations, bankCards);

            //Reinitialize table
            foreach (var column in cardControllers)
            {
                for (int i = 0; i < column.Count; i++)
                {
                    column[i].OnClicked += OnCardClicked;

                    if (i == (column.Count - 1))
                    {
                        column[i].BecameActive();
                        _openedCards.Add(_cardsAtTable[column[i]].Card);
                        CardOpened?.Invoke(_cardsAtTable[column[i]].Card);
                    }
                }
            }
        }

        private void InitializeBank(List<Combination> combinations, List<IController> bankCards)
        {
            //Initialize cards in bank
            int lastCardIndex = combinations.Count - 1;

            for (int i = 0; i < combinations.Count; i++)
            {
                AddCardToDictionary(combinations[i].BankCard, bankCards[i], _basePosion);
                bankCards[i].OnClicked += OnBankCardClicked;
                _bankCards.Add(bankCards[i], combinations[i].BankCard);
            }

            //Show current based card
            CardOpened?.Invoke(combinations[lastCardIndex].BankCard);
            CardMovedToBase?.Invoke(combinations[lastCardIndex].BankCard);
            _baseCard = new(combinations[lastCardIndex].BankCard, bankCards[lastCardIndex]);

            //Made next bank card clickable
            bankCards[lastCardIndex - 1].BecameActive();

            // remove unused cards
            for (int i = combinations.Count; i < bankCards.Count; i++)
                bankCards[i].Disable();

            //remove current base card from bank
            bankCards[lastCardIndex].OnClicked -= OnBankCardClicked;
            _bankCards.Remove(bankCards[lastCardIndex]);
        }

        private void ShuffleCards(List<Combination> combinations, List<List<IController>> cardControllers)
        {
            //Create dictionary to monitoring columns fillness
            Dictionary<int, List<IController>> columnFillness = new()
            {
                { 0, new List<IController>()},
                { 1, new List<IController>()},
                { 2, new List<IController>()},
                { 3, new List<IController>()},
            };

            int columnSize = cardControllers[0].Count;
            int columnAmount = cardControllers.Count;
            int column;
            IController controller;
            CardsPair pair;

            foreach (var combination in combinations)
            {
                List<Card> cards = combination.Cards.ToList();

                //throw cards from combination to all columns
                for (int i = cards.Count - 1; i >= 0; i--)
                {
                    bool isAdded = false;

                    //until we find space for current card
                    while (isAdded == false)
                    {
                        //chose column
                        column = UnityEngine.Random.Range(0, columnAmount);

                        //if collumn didn't had any cards, then current card don't have descendant
                        if (cardControllers[column].Count == columnSize)
                        {
                            controller = InitController(cardControllers[column], cards[i]);
                            pair = new(cards[i]);
                            _cardsAtTable.Add(controller, pair);
                            columnFillness[column].Add(controller);
                            isAdded = true;
                        }
                        else
                        {
                            //add card on top of column
                            if (cardControllers[column].Count > 0)
                            {
                                controller = InitController(cardControllers[column], cards[i]);
                                pair = new(cards[i], columnFillness[column].Last());
                                _cardsAtTable.Add(controller, pair);
                                columnFillness[column].Add(controller);

                                //now, if column is full - show card on top
                                if (cardControllers[column].Count == 0)
                                {
                                    controller.BecameActive();
                                    _openedCards.Add(_cardsAtTable[controller].Card);
                                    CardOpened?.Invoke(_cardsAtTable[controller].Card);
                                }

                                isAdded = true;
                            }
                        }
                    }
                }
            }
        }

        private IController InitController(List<IController> controllers, Card card)
        {
            IController controller = controllers[0];
            AddCardToDictionary(card, controller, _basePosion);
            controller.OnClicked += OnCardClicked;
            controllers.Remove(controller);
            return controller;
        }

        private void AddCardToDictionary(Card card, IInitializer controller, Vector3 position)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            _cardViews.Add(card, controller.Init(card, position));
        }

        private void Unsubscribe()
        {
            foreach (var key in _cardsAtTable.Keys)
                key.OnClicked -= OnCardClicked;

            foreach (var key in _bankCards.Keys)
                key.OnClicked -= OnBankCardClicked;
        }

        private void OnBankCardClicked(IClickable clickable)
        {
            IController controller = clickable as IController;
            _baseCard.Disable();
            _baseCard = new(_bankCards[controller], controller);
            controller.BecameInactive();
            CardOpened?.Invoke(_bankCards[controller]);
            CardMovedToBase?.Invoke(_bankCards[controller]);
            controller.OnClicked -= OnBankCardClicked;
            _bankCards.Remove(controller);

            if (_bankCards.Count > 0)
                _bankCards.Keys.Last().BecameActive();

            ////check lose condition
            if (CheckLose())
                PlayerLosed?.Invoke();
        }

        private void OnCardClicked(IClickable clickable)
        {
            IController controller = clickable as IController;
            Card card = _cardsAtTable[controller].Card;

            if (card.CanMove(_baseCard.Card) == false)
            {
                CardMoveFailed?.Invoke(card);
                return;
            }

            controller.BecameInactive();
            CardMovedToBase?.Invoke(card);
            _baseCard.Disable();
            _baseCard = new(card, controller);
            _solvedCards.Add(card);
            _openedCards.Remove(card);

            IController descendant = _cardsAtTable[controller].DescendantCard;

            if (descendant != null)
            {
                descendant.BecameActive();
                _openedCards.Add(_cardsAtTable[descendant].Card);
                CardOpened?.Invoke(_cardsAtTable[descendant].Card);
            }

            //check win condition
            if (_deckSize == _solvedCards.Count)
            {
                PlayerWon?.Invoke();
                return;
            }

            //check lose condition
            if(CheckLose())
                PlayerLosed?.Invoke();
        }

        private bool CheckLose()
        {
            if(_bankCards.Count > 0)
                return false;

            foreach (Card card in _openedCards)
                if (card.CanMove(_baseCard.Card))
                    return false;

            return true;
        }

        private class CardsPair
        {
            private readonly Card _card;
            private readonly IController _descendantCard;

            /// <summary>
            /// class for cards at "table"
            /// </summary>
            /// <param name="card">current card</param>
            /// <param name="descendant">card under current card</param>
            /// <exception cref="ArgumentNullException"></exception>
            public CardsPair(Card card, IController descendant = null)
            {
                _card = card != null ? card : throw new ArgumentNullException(nameof(card));
                _descendantCard = descendant;
            }

            public Card Card => _card;

            public IController DescendantCard => _descendantCard;
        }

        private class BaseCard
        {
            private readonly Card _card;
            private readonly IDisabler _disabler;

            /// <summary>
            /// class for cards at base
            /// </summary>
            /// <param name="card">current "base" card</param>
            /// <param name="disabler">disable component at current "base" card</param>
            /// <exception cref="ArgumentNullException"></exception>
            public BaseCard(Card card, IDisabler disabler)
            {
                _card = card != null ? card : throw new ArgumentNullException(nameof(card));
                _disabler = disabler != null ? disabler : throw new ArgumentNullException(nameof(disabler));
            }

            public Card Card => _card;

            public void Disable() => _disabler.Disable();
        }
    }
}