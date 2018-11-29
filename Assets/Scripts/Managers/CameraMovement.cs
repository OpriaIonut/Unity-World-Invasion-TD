using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float movementSpeed = 100f;
    public float zoomSpeed = 100f;
    public Vector3 minZoomPos;
    public Vector3 maxZoomPos;

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            Zoom(scrollInput);
        }

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Move(verticalInput, horizontalInput);
        }
    }

    private void Move(float vertical, float horizontal)
    {
        Vector3 newPos = transform.position;

        if(vertical != 0)
        {
            newPos.z += vertical * movementSpeed * Time.deltaTime;
        }
        if(horizontal != 0)
        {
            newPos.x += horizontal * movementSpeed * Time.deltaTime;
        }

        transform.position = newPos;
    }

    private void Zoom(float input)
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y - input * zoomSpeed, transform.position.z);

        if (transform.position.y >= minZoomPos.y && transform.position.y <= maxPos.y)
        {
            transform.position = newPos;
        }
        else if (transform.position.y < minZoomPos.y && input < 0)
        {
            transform.position = newPos;
        }
        else if (transform.position.y > maxPos.y && input > 0)
        {
            transform.position = newPos;
        }
    }
}
