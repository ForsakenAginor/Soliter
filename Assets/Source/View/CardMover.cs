using DG.Tweening;
using UnityEngine;

namespace Assets.Source.View
{
    public class CardMover : MonoBehaviour
    {
        [SerializeField] private float _animationDuration;

        private Tween _tween;

        public void Move(Vector3 position)
        {
            _tween = transform.DOMove(position, _animationDuration);
        }

        public void CancelMove()
        {
            _tween.Kill();
        }
    }
}