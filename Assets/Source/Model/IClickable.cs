using System;

namespace Assets.Source.Model
{
    public interface IClickable
    {
        public event Action<IClickable> OnClicked;
    }
}