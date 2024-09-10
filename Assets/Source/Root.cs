using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private CardController[] _firstColumn;
    [SerializeField] private CardController[] _secondColumn;
    [SerializeField] private CardController[] _thirdColumn;
    [SerializeField] private CardController[] _fourthColumn;
    [SerializeField] private GameObject _victoryScreen;

    private Game _game;

    private void Awake()
    {
        _game = new Game(_firstColumn);
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
