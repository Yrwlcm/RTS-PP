using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent navAgent;
    public LayerMask ground;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ground))
            {
                navAgent.SetDestination(hit.point);
            }
        }
    }
}
