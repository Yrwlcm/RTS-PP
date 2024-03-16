using System.Collections.Generic;
using Scipts.Interfaces;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    public readonly HashSet<ISelectable> allUnits = new();
    private readonly HashSet<ISelectable> selectedUnits = new();
    private readonly HashSet<ISelectable> unitsInBox = new();

    public LayerMask clickable;

    private Camera mainCamera;
    [SerializeField] private RectTransform boxVisual;
    private Rect selectionBox;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private const float DragTreshold = 50;

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
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick();
        }

        if (Input.GetMouseButton(0))
        {
            OnMouseHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseRelease();
        }
    }

    private void OnMouseClick()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            DeselectAll();
        }

        startPosition = Input.mousePosition;
        selectionBox = new Rect();
    }

    private void OnMouseHold()
    {
        if (boxVisual.rect.size.magnitude > DragTreshold)
        {
            foreach (var unit in allUnits)
            {
                var unitScreenPosition = mainCamera.WorldToScreenPoint(unit.GameObject.transform.position);
                if (selectionBox.Contains(unitScreenPosition) && unitsInBox.Add(unit))
                {
                    unit.EnableOutline();
                }
                else if (!selectionBox.Contains(unitScreenPosition) && !selectedUnits.Contains(unit) &&
                         unitsInBox.Remove(unit))
                {
                    unit.DisableOutline();
                }
            }
        }

        endPosition = Input.mousePosition;
        DrawVisual();
        DrawSelection();
    }

    private void OnMouseRelease()
    {
        if (boxVisual.rect.size.magnitude < DragTreshold)
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, clickable))
            {
                var selectable = hit.collider.gameObject.GetComponent<ISelectable>();

                if (selectable.Selected)
                    DeselectAndRemember(selectable);
                else
                    SelectAndRemember(selectable);
            }
        }
        else
        {
            foreach (var unit in unitsInBox)
                SelectAndRemember(unit);
        }

        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void SelectAndRemember(ISelectable unit)
    {
        unit.Select();
        selectedUnits.Add(unit);
    }

    private void DeselectAndRemember(ISelectable selectable)
    {
        selectable.Deselect();
        selectedUnits.Remove(selectable);
    }

    private void DeselectAll()
    {
        foreach (var selectedUnit in selectedUnits)
        {
            selectedUnit.Deselect();
        }

        selectedUnits.Clear();
        unitsInBox.Clear();
    }

    private void DrawVisual()
    {
        var boxStart = startPosition;
        var boxEnd = endPosition;

        var boxCenter = (boxStart + boxEnd) / 2;

        boxVisual.position = boxCenter;

        var boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        selectionBox.xMin = Mathf.Min(Input.mousePosition.x, startPosition.x);
        selectionBox.xMax = Mathf.Max(Input.mousePosition.x, startPosition.x);

        selectionBox.yMin = Mathf.Min(Input.mousePosition.y, startPosition.y);
        selectionBox.yMax = Mathf.Max(Input.mousePosition.y, startPosition.y);
    }
}