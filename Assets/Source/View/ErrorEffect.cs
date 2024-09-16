using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Source.View
{
    public class ErrorEffect : MonoBehaviour
    {
        [SerializeField] private float _animationDuration = 0.2f;
        [SerializeField] private Color _animationColor = Color.red;
        [SerializeField] private Image _image;

        private bool _isPlaying = false;

        public void Play()
        {
            if (_isPlaying)
                return;

            int loops = 2;
            _image.DOColor(_animationColor, _animationDuration).SetLoops(loops, LoopType.Yoyo);
            StartCoroutine(StartTimer(_animationDuration * loops));
        }

        private IEnumerator StartTimer(float duration)
        {
            WaitForSeconds delay = new WaitForSeconds(duration);
            _isPlaying = true;
            yield return delay;
            _isPlaying = false;
        }
    }
}