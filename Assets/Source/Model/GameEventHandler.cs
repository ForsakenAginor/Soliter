using System;

namespace Assets.Source.Model
{
    public class GameEventHandler
    {
        private readonly Game _game;

        public GameEventHandler(Game game)
        {
            _game = game != null ? game : throw new ArgumentNullException(nameof(_game));
            _game.CardMovedToBase += OnCardMoved;
            _game.CardOpened += OnCardOpened;
            _game.CardMoveFailed += OnCardMoveFailed;
        }

        ~GameEventHandler()
        {
            _game.CardMovedToBase -= OnCardMoved;
            _game.CardOpened -= OnCardOpened;
            _game.CardMoveFailed -= OnCardMoveFailed;
        }

        private void OnCardMoveFailed(Card card)
        {
            _game.CardViews[card].PlayErrorEffect();
        }

        private void OnCardOpened(Card card)
        {
            _game.CardViews[card].BecameVisible();
        }

        private void OnCardMoved(Card card)
        {
            _game.CardViews[card].GoToBase();
        }
    }
}