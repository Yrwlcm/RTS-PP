using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public bool ShouldTakeCommands;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LineRendererController lineRendererController;

    private Camera mainCamera;
    private NavMeshAgent navAgent;

    private void Start()
    {
        mainCamera = Camera.main;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!ShouldTakeCommands) return;

        if (!Input.GetMouseButtonDown((int)MouseButton.Right))
            return;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ground))
            MoveTo(hit.point);
    }

    public void MoveTo(Vector3 position)
    {
        navAgent.SetDestination(position);
        lineRendererController.DrawMovingLine(position);
    }
}