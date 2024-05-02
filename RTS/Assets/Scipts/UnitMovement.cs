using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LineRenderer lineRenderer;
    
    private NavMeshAgent navAgent;
    private void Start()
    {
        mainCamera = Camera.main;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var groundedTransfortm = transform.position;
        groundedTransfortm.y = 0.5f;
        lineRenderer.SetPosition(0, groundedTransfortm);
        
        if (!Input.GetMouseButtonDown((int)MouseButton.Right)) return;

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ground))
        {
            navAgent.SetDestination(hit.point);
            lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z));
        }
    }
}