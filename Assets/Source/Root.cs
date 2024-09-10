using Assets.Source.Controller;
using Assets.Source.General;
using Assets.Source.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Source
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private List<CardController> _firstColumn;
        [SerializeField] private List<CardController> _secondColumn;
        [SerializeField] private List<CardController> _thirdColumn;
        [SerializeField] private List<CardController> _fourthColumn;
        [SerializeField] private List<CardController> _bank;
        [SerializeField] private Transform _base;

        [Header("UI")]
        [SerializeField] private GameObject _victoryScreen;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitGameButton;

        private Game _game;

        private void Awake()
        {
            List<List<CardController>> deck = new()
        {
            _firstColumn,
            _secondColumn,
            _thirdColumn,
            _fourthColumn
        };

            _game = new Game(deck, _bank, _base.position);
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
}