using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtilities
{
    //ScreenToWorldPoint у mainCamera не работает, почему - не знаю
    //Загуглить не получилось, при этом все остальные функции которые конвертируют ворлд в скрин и наоборот работают
    //Поэтому пришлось писать этот метод
    public static Vector3 ScreenPositionToGroundRaycast(Camera mainCamera, Vector3 screenPosition)
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(screenPosition), out var hit, 100_000, LayerMask.GetMask("Ground"));

        return hit.point;
    }
}