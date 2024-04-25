using Scipts.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace Scipts
{
    public class Unit : NetworkBehaviour, ISelectable
    {
        public bool Selected { get; private set; }
        public bool OutlineEnabled { get; private set; }

        private Outline outline;
        private UnitMovement movement;

        [SerializeField] private SelectionManager selectionManager;

        public override void OnNetworkSpawn()
        {
            outline = GetComponent<Outline>();
            movement = GetComponent<UnitMovement>();
        }

        public override void OnDestroy()
        {
            selectionManager.allUnits.Remove(this);
        }
        
        public void SetSelectionManager(SelectionManager selectionManager)
        {
            this.selectionManager = selectionManager;
            this.selectionManager.allUnits.Add(this);
        }

        public void EnableOutline()
        {
            if (!IsOwner)
                return;

            OutlineEnabled = true;
            outline.enabled = true;
        }

        public void DisableOutline()
        {
            if (!IsOwner)
                return;

            OutlineEnabled = false;
            outline.enabled = false;
        }

        public void Select()
        {
            if (!IsOwner)
                return;

            EnableOutline();
            Selected = true;
            movement.enabled = true;
        }
        
        public void Deselect()
        {
            if (!IsOwner)
                return;

            DisableOutline();
            Selected = false;
            movement.enabled = false;
        }
    }
}