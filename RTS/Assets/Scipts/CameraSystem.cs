using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private int MoveSpeed = 50;

    void Update()
    {
        var inputDirection = new Vector3();

        if (Input.GetKey(KeyCode.UpArrow)) inputDirection.z = 1;
        if (Input.GetKey(KeyCode.DownArrow)) inputDirection.z = -1;
        if (Input.GetKey(KeyCode.LeftArrow)) inputDirection.x = -1;
        if (Input.GetKey(KeyCode.RightArrow)) inputDirection.x = 1;

        var moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

        transform.position += moveDirection * MoveSpeed * Time.deltaTime;
    }
}