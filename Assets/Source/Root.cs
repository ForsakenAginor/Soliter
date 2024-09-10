using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Root : MonoBehaviour
{
    [SerializeField] private CardController[] _firstColumn;
    [SerializeField] private CardController[] _secondColumn;
    [SerializeField] private CardController[] _thirdColumn;
    [SerializeField] private CardController[] _fourthColumn;

    [Header("UI")]
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitGameButton;

    private Game _game;

    private void Awake()
    {
        List<CardController[]> cards = new()
        {
            _firstColumn,
            _secondColumn,
            _thirdColumn,
            _fourthColumn
        };
        _game = new Game(cards);
        _game.PlayerWon += OnPlayerWon;
        _newGameButton.onClick.AddListener(OnNewGameButtonClick);
        _exitGameButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDestroy()
    {
        _game.PlayerWon -= OnPlayerWon;
        _newGameButton.onClick.RemoveListener(OnNewGameButtonClick);
        _exitGameButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnNewGameButtonClick()
    {
        SceneManager.LoadScene(Scenes.GameScene.ToString());
    }

    private void OnPlayerWon()
    {
        _victoryScreen.SetActive(true);
    }
}
