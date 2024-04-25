using UnityEngine;

namespace Scipts.Interfaces
{
    public interface ISelectable
    {
        public bool Selected { get; }
        public bool OutlineEnabled { get; }

        public void EnableOutline();
        public void DisableOutline();

        public void Select();
        public void Deselect();
    }
}