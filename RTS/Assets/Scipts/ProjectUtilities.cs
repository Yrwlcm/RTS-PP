using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectUtilities
{
    public static Vector3 ScreenPositionToGroundRaycast(Camera mainCamera, Vector3 screenPosition)
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(screenPosition), out var hit, 100_000, LayerMask.GetMask("Ground"));

        return hit.point;
    }
}
