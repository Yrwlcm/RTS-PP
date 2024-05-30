using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Unit = Scipts.Unit;

public class Attack : MonoBehaviour
{
    public bool ShouldTakeCommands;

    [SerializeField] private Transform attackPoint;
    [SerializeField] [CanBeNull] private Transform projectilePrefab;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackCooldown = 1f;

    [SerializeField] private Unit unit;
    [SerializeField] private UnitMovement unitMovement;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LineRendererController lineRendererController;

    private Animator animator;
    private Camera mainCamera;
    private float lastAttackTime;
    private Transform target;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (ShouldTakeCommands)
            SetTarget();

        if (target == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            unitMovement.MoveTo(target.position);
        }
        else if (Time.time > lastAttackTime + attackCooldown)
        {
            unitMovement.MoveTo(transform.position);
            Shoot(target);
            lastAttackTime = Time.time;
        }
        else
        {
            unitMovement.MoveTo(transform.position);
        }

        lineRendererController.DrawAttackingLine(target.position);
    }

    public void SetTarget()
    {
        if (!Input.GetMouseButtonDown((int)MouseButton.Right)) return;

        var isHit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 100_000,
            LayerMask.GetMask("Unit"));

        if (!isHit)
        {
            target = null;
            return;
        }
        
        target = hit.transform.GetComponent<Unit>().Team == unit.Team
            ? null
            : hit.transform;
    }

    private void Shoot(Transform target)
    {
        animator?.SetBool("Attacking", true);
        var projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var projectile = projectileInstance.GetComponent<Projectile>();
        projectile.damage = attackDamage;
        if (projectile != null)
        {
            projectile.SetTarget(target);
        }
    }
}