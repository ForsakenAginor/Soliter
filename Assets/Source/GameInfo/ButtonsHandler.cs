using Assets.Source.General;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Source.GameInfo
{
    public class ButtonsHandler : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;

        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        }

        private void OnDestroy()
        {
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);            
        }

        private void OnMainMenuButtonClick()
        {
            SceneManager.LoadScene(Scenes.MainMenu.ToString());
        }
    }
}