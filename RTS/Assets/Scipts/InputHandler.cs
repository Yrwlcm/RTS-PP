using Scipts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scipts
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;
        public Transform selectedBuilding = null;
        public LayerMask interactableLayer = new LayerMask();

        [SerializeField] private Camera mainCamera;

        private RaycastHit hit;

        private Vector3 mousePos;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Handle();
            }
        }

        private void Deselect()
        {
            if (selectedBuilding) 
            {
                selectedBuilding.gameObject.GetComponent<IBuilding>().OnInteractExit();
                selectedBuilding = null;
            }
        }

        public void Handle()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100_000, interactableLayer))
                {
                    if (addedBuilding(hit.transform))
                    {
                        
                    }
                }
                else
                {
                    Deselect();
                }
            }

            if (Input.GetMouseButtonDown(1) && selectedBuilding != null)
            {
                selectedBuilding.gameObject.GetComponent<IBuilding>().SetSpawnMarkerLocation();
            }
        }

        private IBuilding addedBuilding(Transform tf)
        {
            Deselect();
            IBuilding iBuilding = tf.GetComponent<IBuilding>();
            if (iBuilding) 
            {
                selectedBuilding = iBuilding.gameObject.transform;
                iBuilding.OnInteractEnter();

                return iBuilding;
            }
            else
            {
                return null;
            }
        }
    }
}
