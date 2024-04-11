using System;
using System.Collections;
using System.Collections.Generic;
using Scipts;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using static ProjectUtilities;

[RequireComponent(typeof(SelectionManager))]
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private SerializedDictionary<KeyCode, Unit> units;

    private void Update()
    {
        foreach (var unit in units)
        {
            if (!Input.GetKeyDown(unit.Key)) continue;
            
            var position = ScreenPositionToGroundRaycast(mainCamera, Input.mousePosition);
            unit.Value.Spawn(selectionManager, position, Quaternion.Euler(0,0,0));
        }
    }
}