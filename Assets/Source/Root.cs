using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private CardAtTable[] _cards;

    private void Awake()
    {
        Game game = new Game(_cards);
    }
}
