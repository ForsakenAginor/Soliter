using System;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private CardController[] _cards;
    [SerializeField] private GameObject _victoryScreen;

    private Game _game;

    private void Awake()
    {
        _game = new Game(_cards);
        _game.PlayerWon += OnPlayerWon;
    }

    private void OnDestroy()
    {
        _game.PlayerWon -= OnPlayerWon;
    }

    private void OnPlayerWon()
    {
        _victoryScreen.SetActive(true);
    }
}
