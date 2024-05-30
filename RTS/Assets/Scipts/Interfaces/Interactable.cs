using UnityEngine;

namespace Scipts.Interfaces
{
    public class Interactable : MonoBehaviour
    {
        public bool isInteracting = false;
        [SerializeField] private Outline outline;

        public virtual void Awake()
        {
            outline.enabled = false;
        }

        public virtual void OnInteractEnter()
        {
            ShowHighlight();
            isInteracting = true;
        }

        public virtual void OnInteractExit()
        {
            HideHighlight();
            isInteracting = false;
        }

        public virtual void ShowHighlight()
        {
            outline.enabled = true;
        }

        public virtual void HideHighlight()
        {
            outline.enabled = false;
        }

    }
}