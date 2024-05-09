using Scipts.Interfaces;
using UnityEngine;

namespace Scipts
{
    public class Unit : MonoBehaviour, ISelectable
    {
        public GameObject GameObject { get; private set; }
        public bool Selected { get; private set; }
        public bool OutlineEnabled { get; private set; }

        [SerializeField] private LineRenderer lineRenderer;
        
        private Outline outline;
        private UnitMovement movement;
        private Attack attack;

        private void Start()
        {
            GameObject = gameObject;
            SelectionManager.Instance.allUnits.Add(this);
            outline = GetComponent<Outline>();
            movement = GetComponent<UnitMovement>();
            attack = GetComponent<Attack>();
            
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        private void OnDestroy()
        {
            SelectionManager.Instance.allUnits.Remove(this);
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
            movement.ShouldTakeCommands = true;
            attack.ShouldTakeCommands = true;
            lineRenderer.enabled = true;
        }

        public void Deselect()
        {
            DisableOutline();
            Selected = false;
            movement.ShouldTakeCommands = false;
            attack.ShouldTakeCommands = false;
            lineRenderer.enabled = false;
        }
    }
}