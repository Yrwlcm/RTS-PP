using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private Vector3? target;
    private bool isMoving;
    private bool isAttacking;
    
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material walkingPath;
    [SerializeField] private Material attackPath;

    private void Start()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }
    
    private void Update()
    {
        if (isAttacking)
        {
            lineRenderer.material = attackPath;
        }
        else if (isMoving)
        {
            lineRenderer.material = walkingPath;
        }
        lineRenderer.SetPosition(0, transform.position);
        if (target is not null)
        {
            lineRenderer.SetPosition(1, target.Value);
        }
    }

    public void DrawMovingLine(Vector3 position)
    {
        isAttacking = false;
        isMoving = true;
        target = position;
    }

    public void DrawAttackingLine(Vector3 position)
    {
        isMoving = false;
        isAttacking = true;
        target = position;
    }
}
