using Scipts.Interfaces;
using UnityEngine;

namespace Scipts
{
    public class Unit : MonoBehaviour, ISelectable
    {
        public GameObject GameObject { get; private set; }
        public bool Selected { get; private set; }
        public bool OutlineEnabled { get; private set; }

        private Outline outline;
        private UnitMovement movement;
        private SelectionManager selectionManager;

        private void Awake()
        {
            GameObject = gameObject;
            outline = GetComponent<Outline>();
            movement = GetComponent<UnitMovement>();
        }

        private void OnDestroy()
        {
            selectionManager.allUnits.Remove(this);
        }

        public void Spawn(SelectionManager bindingSelectionManager, Vector3 position, Quaternion rotation)
        {
            var instance = Instantiate(this, position, rotation);
            instance.selectionManager = bindingSelectionManager;
            instance.selectionManager.allUnits.Add(instance);
        }

        public void EnableOutline()
        {
            OutlineEnabled = true;
            outline.enabled = true;
        }

        public void DisableOutline()
        {
            OutlineEnabled = false;
            outline.enabled = false;
        }

        public void Select()
        {
            EnableOutline();
            Selected = true;
            movement.enabled = true;
        }

        public void Deselect()
        {
            DisableOutline();
            Selected = false;
            movement.enabled = false;
        }
    }
}