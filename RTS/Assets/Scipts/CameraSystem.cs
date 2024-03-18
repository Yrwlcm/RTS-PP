using UnityEngine;


public class CameraSystem : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 35;
    [SerializeField] private int rotateSpeed = 100;
    [SerializeField] private int edgeScrollingBorder = 20;

    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;

    void Update()
    {
        var inputDirection = new Vector3();

        if (!dragPanMoveActive)
        {
            inputDirection = CheckInputDirection();
            inputDirection = CheckEdgeScrollDirection();
        }
        else
        {
            inputDirection = CheckClickDragDirection();
        }

        var moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        var rotateDirection = CheckRotateDirection();

        transform.eulerAngles += new Vector3(0, rotateDirection * rotateSpeed * Time.deltaTime, 0);
    }

    private Vector3 CheckClickDragDirection()
    {
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
            dragPanMoveActive = true;
        }

        var inputDirection = new Vector3();
        var mouseMoveDelta = (Vector2)Input.mousePosition - lastMousePosition;

        inputDirection.x = mouseMoveDelta.x;
        inputDirection.z = mouseMoveDelta.y;

        lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(2))
            dragPanMoveActive = false;

        return new Vector3(-mouseMoveDelta.y, mouseMoveDelta.x, 0);
    }

    private int CheckRotateDirection()
    {
        var rotateDirection = 0;
        if (Input.GetKey(KeyCode.Q)) rotateDirection = 1;
        if (Input.GetKey(KeyCode.E)) rotateDirection = -1;
        return rotateDirection;
    }

    private Vector3 CheckInputDirection()
    {
        var inputDirection = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow)) inputDirection.z = 1;
        if (Input.GetKey(KeyCode.DownArrow)) inputDirection.z = -1;
        if (Input.GetKey(KeyCode.LeftArrow)) inputDirection.x = -1;
        if (Input.GetKey(KeyCode.RightArrow)) inputDirection.x = 1;
        return inputDirection;
    }

    private Vector3 CheckEdgeScrollDirection()
    {
        var inputDirection = new Vector3();
        if (Input.mousePosition.x < edgeScrollingBorder) inputDirection.x = -1;
        if (Input.mousePosition.y < edgeScrollingBorder) inputDirection.z = -1;
        if (Input.mousePosition.x > Screen.width - edgeScrollingBorder) inputDirection.x = +1;
        if (Input.mousePosition.y > Screen.height - edgeScrollingBorder) inputDirection.z = +1;
        return inputDirection;
    }
}