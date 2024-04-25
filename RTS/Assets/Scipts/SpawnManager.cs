using System;
using System.Collections;
using System.Collections.Generic;
using Scipts;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.Netcode;
using static ProjectUtilities;

[RequireComponent(typeof(SelectionManager))]
public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private SerializedDictionary<KeyCode, Unit> units;

    private void Update()
    {
        if (!IsOwner)
            return;

        foreach (var bindUnit in units)
        {
            if (!Input.GetKeyDown(bindUnit.Key)) continue;

            var position = ScreenPositionToGroundRaycast(mainCamera, Input.mousePosition);

            SpawnUnitServerRPC(bindUnit.Key, OwnerClientId, position, Quaternion.Euler(0, 0, 0),
                selectionManager);
        }
    }

    [ServerRpc]
    public void SpawnUnitServerRPC(KeyCode unitCode, ulong ownnerId, Vector3 position, Quaternion rotation,
        NetworkBehaviourReference selectionManagerReference)
    {
        var unit = units[unitCode];
        var instance = Instantiate(unit, position, rotation);
        instance.NetworkObject.SpawnWithOwnership(ownnerId);
        SetUnitOwnershipForAllClientRPC(instance, selectionManagerReference);
    }

    [ClientRpc]
    public void SetUnitOwnershipForAllClientRPC(NetworkBehaviourReference unitReference,
        NetworkBehaviourReference selectionManagerReference)
    {
        unitReference.TryGet(out Unit unit);
        selectionManagerReference.TryGet(out SelectionManager selectionManager);
        unit.SetSelectionManager(selectionManager);
    }
}