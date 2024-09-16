using Assets.Source.Controller;
using Assets.Source.General;
using Assets.Source.Model;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private GameObject _losingScreen;
        [SerializeField] private GameObject _buttonPanel;
        [SerializeField] private Button[] _newGameButtons;
        [SerializeField] private Button[] _exitGameButtons;
        [SerializeField] private Button[] _restartGameButtons;

        private Game _game;

        private void Start()
        {
            Generator generator = new Generator();

            //Prepare gameobjects to use in model
            List<List<IController>> deck = new()
            {
                _firstColumn.Select(o => o as IController).ToList(),
                _secondColumn.Select(o => o as IController).ToList(),
                _thirdColumn.Select(o => o as IController).ToList(),
                _fourthColumn.Select(o => o as IController).ToList(),
            };

            List<IController> bank = _bank.Select(o => o as IController).ToList();

            _game = new Game(deck, bank, generator.CreateCombinations(), _base.position);
            _game.PlayerWon += OnPlayerWon;
            _game.PlayerLosed += OnPlayerLosed;

            foreach (var button in _newGameButtons)
                button.onClick.AddListener(OnNewGameButtonClick);

            foreach (var button in _exitGameButtons)
                button.onClick.AddListener(OnExitButtonClick);

            foreach (var button in _restartGameButtons)
                button.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnDestroy()
        {
            _game.PlayerWon -= OnPlayerWon;
            _game.PlayerLosed -= OnPlayerLosed;

            foreach (var button in _newGameButtons)
                button.onClick.RemoveListener(OnNewGameButtonClick);

            foreach (var button in _exitGameButtons)
                button.onClick.RemoveListener(OnExitButtonClick);

            foreach (var button in _restartGameButtons)
                button.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void OnRestartButtonClick()
        {
            foreach (var controller in _firstColumn)
                controller.Normalize();

            foreach (var controller in _secondColumn)
                controller.Normalize();

            foreach (var controller in _thirdColumn)
                controller.Normalize();

            foreach (var controller in _fourthColumn)
                controller.Normalize();

            foreach (var controller in _bank)
                controller.Normalize();

            //Prepare gameobjects to use in model
            List<List<IController>> deck = new()
            {
                _firstColumn.Select(o => o as IController).ToList(),
                _secondColumn.Select(o => o as IController).ToList(),
                _thirdColumn.Select(o => o as IController).ToList(),
                _fourthColumn.Select(o => o as IController).ToList(),
            };

            List<IController> bank = _bank.Select(o => o as IController).ToList();

            _game.Load(deck, bank);
            _buttonPanel.SetActive(true);
            _losingScreen.SetActive(false);
        }

        private void OnExitButtonClick()
        {
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }

        private void OnNewGameButtonClick()
        {
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }

        private void OnPlayerWon()
        {
            _victoryScreen.SetActive(true);
            _buttonPanel.SetActive(false);
        }

        private void OnPlayerLosed()
        {
            _losingScreen.SetActive(true);
            _buttonPanel.SetActive(false);
        }
    }
}