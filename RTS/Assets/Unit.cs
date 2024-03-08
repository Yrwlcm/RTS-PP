using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private void Start()
    {
        UnitSelectionManager.Instance.allUnits.Add(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnits.Remove(gameObject);
    }
}
