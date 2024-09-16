using Assets.Source.General;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Source.MainMenu
{
    public class ButtonsHandler : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _infoGameButton;

        private void Awake()
        {
            _exitButton.onClick.AddListener(OnExitButtonClick);
            _newGameButton.onClick.AddListener(OnNewGameButtonClick);
            _infoGameButton.onClick.AddListener(OnInfoButtonClick);
        }

        private void OnDestroy()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
            _newGameButton.onClick.RemoveListener(OnNewGameButtonClick);
            _infoGameButton.onClick.RemoveListener(OnInfoButtonClick);
        }

        private void OnInfoButtonClick()
        {
            SceneManager.LoadScene(Scenes.GameInfo.ToString());
        }

        private void OnNewGameButtonClick()
        {
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}