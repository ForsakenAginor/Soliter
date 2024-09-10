using System;
using System.Collections.Generic;

namespace Assets.Source.Model
{
    public class Combination
    {
        private readonly Card _bankCard;
        private readonly List<Card> _cards;

        public Combination(Card bankCard, List<Card> cards)
        {
            _bankCard = bankCard != null ? bankCard : throw new ArgumentNullException(nameof(bankCard));
            _cards = cards != null ? cards : throw new ArgumentNullException(nameof(cards));
        }

        public Card BankCard => _bankCard;

        public IEnumerable<Card> Cards => _cards;
    }
}