using System.Collections.Generic;
using Scipts.Interfaces;
using UnityEngine;
using static CameraUtilities;

public class SelectionManager : MonoBehaviour
{
    public static Dictionary<int, SelectionManager> Instances { get; } = new();

    public readonly HashSet<ISelectable> allUnits = new();
    private readonly HashSet<ISelectable> selectedUnits = new();
    private readonly HashSet<ISelectable> unitsInBox = new();

    public HashSet<ISelectable> SelectedUnit => selectedUnits;
    public int TeamId => teamId;
    public bool IsPlayerManager => isPlayerManager;


    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform boxVisual;
    [SerializeField] private LayerMask clickable;
    [SerializeField] private int teamId;
    [SerializeField] private bool isPlayerManager;

    private Rect selectionBox;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private const float DragTreshold = 50;

    public void RemoveUnit(ISelectable unit)
    {
        allUnits.Remove(unit);
        selectedUnits.Remove(unit);
        unitsInBox.Remove(unit);
    }

    private void Awake()
    {
        if (Instances.TryGetValue(TeamId, out var instance) && Instances[TeamId] != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instances.TryAdd(TeamId, this);
        }
    }

    private void Start()
    {
        if (!isPlayerManager)
            return;
        
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;

        DrawVisual();
    }

    private void Update()
    {
        if (!isPlayerManager)
            return;

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

        startPosition = ScreenPositionToGroundRaycast(mainCamera, Input.mousePosition);
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

        endPosition = ScreenPositionToGroundRaycast(mainCamera, Input.mousePosition);
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
        var boxStart = mainCamera.WorldToScreenPoint(startPosition);
        var boxEnd = mainCamera.WorldToScreenPoint(endPosition);

        var boxCenter = (boxStart + boxEnd) / 2;

        boxVisual.position = boxCenter;

        var boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        selectionBox.xMin = Mathf.Min(Input.mousePosition.x, mainCamera.WorldToScreenPoint(startPosition).x);
        selectionBox.xMax = Mathf.Max(Input.mousePosition.x, mainCamera.WorldToScreenPoint(startPosition).x);

        selectionBox.yMin = Mathf.Min(Input.mousePosition.y, mainCamera.WorldToScreenPoint(startPosition).y);
        selectionBox.yMax = Mathf.Max(Input.mousePosition.y, mainCamera.WorldToScreenPoint(startPosition).y);
    }
}