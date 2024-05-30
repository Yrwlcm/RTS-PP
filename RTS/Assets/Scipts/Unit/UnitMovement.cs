using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public bool ShouldTakeCommands;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LineRendererController lineRendererController;
    public bool IsStagg = false;
    private int staggTiming = 0;

    private Camera mainCamera;
    private Animator animator;
    private NavMeshAgent navAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (IsStagging()) return;
        if (navAgent.velocity.x == 0f && navAgent.velocity.y == 0f && navAgent.velocity.z == 0f) 
            animator?.SetBool("Walking", false);
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
        animator?.SetBool("Walking", true);
        animator?.SetBool("Attacking", false);
        navAgent.SetDestination(position);
        lineRendererController.DrawMovingLine(position);
    }

    private bool IsStagging()
    {
        if (IsStagg)
        {
            staggTiming++;
            if (staggTiming > 30)
            {
                IsStagg = false;
                return false;
            }
            return true;
        }
        
        return false;

    }
}