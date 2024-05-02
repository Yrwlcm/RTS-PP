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

        private void Start()
        {
            GameObject = gameObject;
            SelectionManager.Instance.allUnits.Add(this);
            outline = GetComponent<Outline>();
            movement = GetComponent<UnitMovement>();
            
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
            movement.enabled = true;
            lineRenderer.enabled = true;
        }

        public void Deselect()
        {
            DisableOutline();
            Selected = false;
            movement.enabled = false;
            lineRenderer.enabled = false;
        }
    }
}