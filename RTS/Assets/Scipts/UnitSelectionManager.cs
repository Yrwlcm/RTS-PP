using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

// Singleton класс, но из-за специфики юнити приходится делать фокусы с instanc-ами
public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }

    public readonly HashSet<GameObject> allUnits = new();
    public readonly HashSet<GameObject> selectedUnits = new();
    public readonly HashSet<GameObject> unitsInBox = new();

    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;

    private Camera mainCamera;
    [SerializeField] private RectTransform boxVisual;
    private Rect selectionBox;
    private Vector2 startPosition;
    private Vector2 endPosition;

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

        startPosition = Vector2.zero;
        endPosition = Vector2.zero;

        DrawVisual();
    }

    private void Update()
    {
        var holdShift = Input.GetKey(KeyCode.LeftShift);

        // When Clicked
        if (Input.GetMouseButtonDown(0))
        {
            if (!holdShift)
            {
                DeselectAll();
            }

            startPosition = Input.mousePosition;

            // For selection the Units
            selectionBox = new Rect();
        }

        // When Dragging
        if (Input.GetMouseButton(0))
        {
            if (boxVisual.rect.size.magnitude > 0)
            {
                // if (!holdShift)
                // {
                //     DeselectAll();
                // }

                SelectUnits();
            }
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        // When Releasing
        if (Input.GetMouseButtonUp(0))
        {
            if (boxVisual.rect.size.magnitude == 0)
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                var hitClickable = Physics.Raycast(ray, out var hit, Mathf.Infinity, clickable);
                if (hitClickable)
                {
                    ToggleUnitSelection(hit.collider.gameObject);
                }
            }
            unitsInBox.Clear();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    public void ToggleUnitSelection(GameObject unit)
    {
        if (selectedUnits.Contains(unit))
            SetUnitSelection(unit, false);
        else
        {
            SetUnitSelection(unit, true);
        }
    }

    public void SetUnitSelection(GameObject unit, bool selected)
    {
        if (selected && selectedUnits.Contains(unit))
            return;
        if (!selected && !selectedUnits.Contains(unit))
            return;

        if (selected)
            selectedUnits.Add(unit);
        else
            selectedUnits.Remove(unit);

        EnableUnitMovement(unit, selected);
        EnableOutline(unit, selected);
    }

    public void DeselectAll()
    {
        foreach (var selectedUnit in selectedUnits)
        {
            EnableUnitMovement(selectedUnit, false);
            EnableOutline(selectedUnit, false);
        }

        selectedUnits.Clear();
    }

    private void EnableOutline(GameObject unit, bool enable)
    {
        unit.GetComponent<Outline>().enabled = enable;
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void DrawVisual()
    {
        // Calculate the starting and ending positions of the selection box.
        var boxStart = startPosition;
        var boxEnd = endPosition;

        // Calculate the center of the selection box.
        var boxCenter = (boxStart + boxEnd) / 2;

        // Set the position of the visual selection box based on its center.
        boxVisual.position = boxCenter;

        // Calculate the size of the selection box in both width and height.
        var boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        // Set the size of the visual selection box based on its calculated size.
        boxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }


        if (Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    private void SelectUnits()
    {
        foreach (var unit in allUnits)
        {
            if (selectionBox.Contains(mainCamera.WorldToScreenPoint(unit.transform.position))
                && !unitsInBox.Contains(unit))
            {
                ToggleUnitSelection(unit);
                unitsInBox.Add(unit);
            }
        }
    }
}