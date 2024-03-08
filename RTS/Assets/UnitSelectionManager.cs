using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Singleton класс, но из-за специфики юнити приходится делать фокусы с instanc-ами
public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    public readonly HashSet<GameObject> allUnits = new();
    public readonly HashSet<GameObject> selectedUnits = new();

    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;

    private Camera mainCamera;

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown((int)MouseButton.Left)) return;

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            DeselectAll();
        }

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, clickable))
        {
            ClickSelect(hit.collider.gameObject);
        }
    }

    private void ClickSelect(GameObject unit)
    {
        var isSelected = selectedUnits.Add(unit);
        if (!isSelected)
            selectedUnits.Remove(unit);
        
        //Debug.Log($"{(isSelected ? "Selected" : "Deselected")} a unit", unit);
        EnableUnitMovement(unit, isSelected);
        EnableOutline(unit, isSelected);
    }

    private void EnableOutline(GameObject unit, bool enable)
    {
        unit.GetComponent<Outline>().enabled = enable;
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void DeselectAll()
    {
        foreach (var selectedUnit in selectedUnits)
        {
            EnableUnitMovement(selectedUnit, false);
            EnableOutline(selectedUnit, false);
        }

        selectedUnits.Clear();
    }
}