using UnityEngine;

namespace Assets.Source.View
{
    public class CardOpener : MonoBehaviour
    {
        [SerializeField] private GameObject _face;
        [SerializeField] private GameObject _back;

        public void Open()
        {
            _face.SetActive(true);
            _back.SetActive(false);
        }

        public void Close()
        {
            _face.SetActive(false);
            _back.SetActive(true);
        }
    }
}