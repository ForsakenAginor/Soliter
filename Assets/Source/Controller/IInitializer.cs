using Assets.Source.Model;
using UnityEngine;

namespace Assets.Source.Controller
{
    public interface IInitializer
    {
        public ICardView Init(Card card, Vector3 basePosition);
    }
}