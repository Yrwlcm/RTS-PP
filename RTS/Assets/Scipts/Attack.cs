using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static CameraUtilities;

public class Attack : MonoBehaviour
{
    public bool ShouldTakeCommands;

    [SerializeField] private Transform attackPoint;
    [SerializeField] [CanBeNull] private Transform projectilePrefab;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackCooldown = 1f;
    
    [SerializeField] private UnitMovement unitMovement;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LineRendererController lineRendererController;

    private Camera mainCamera;
    private float lastAttackTime;
    private Transform target;

    private void Start()
    {
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
        
        lineRendererController.DrawAttackingLine(target.position);
    }

    public void SetTarget()
    {
        if (!Input.GetMouseButtonDown((int)MouseButton.Right)) return;

        var isHit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 100_000,
            LayerMask.GetMask("Unit"));

        target = isHit ? hit.transform : null;
    }

    private void Shoot(Transform target)
    {
        var projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var projectile = projectileInstance.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetTarget(target);
        }
    }
}