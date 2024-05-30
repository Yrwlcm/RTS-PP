using UnityEngine;
using Scipts.Interfaces;

namespace Scipts
{
    public class IBuilding : Interactable
    {
        public PlayerActions actions;
        public GameObject spawnMarker = null;
        public GameObject spawnMarkerGraphics = null;
        public float maxMarkerDistance = 10f;

        public virtual void OnInteractEnter()
        {
            ActionFrame.instance.SetActionButtons(actions, spawnMarker);
            spawnMarkerGraphics.SetActive(true);
            base.OnInteractEnter();
        }

        public virtual void OnInteractExit()
        {
            ActionFrame.instance.ClearActions();
            spawnMarkerGraphics.SetActive(false);
            base.OnInteractExit();
        }

        public void SetSpawnMarkerLocation()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                spawnMarker.transform.position = hit.point;
            }
        }
    }
}
