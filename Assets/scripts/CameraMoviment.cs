using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoviment : MonoBehaviour
{
    public FixedJoystick joystick;
    public GameObject player;
    [SerializeField]
    private float cameraSpeed;
    private float xMax, yMin;
    private Vector3 dragOrigin;
    private bool isDragging = false;
    private bool isCameraLocked = false;

    private void Awake()
    {
        cameraSpeed = 5f;
    }

    private void Update()
    {
        GetInput();
    }
    private void GetInput()
    {
        if (joystick.Horizontal != 0f || joystick.Vertical != 0f)
        {
            isCameraLocked = true;
        }
        else
        {
            isCameraLocked = false;
        }

        if (isCameraLocked)
        {
            // Camera follows the user
            transform.position = new Vector3(
                Mathf.Clamp(player.transform.position.x, 0, xMax),
                Mathf.Clamp(player.transform.position.y, yMin, 0),
                -10
            );
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                dragOrigin = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // Handle camera movement while dragging
            if (isDragging)
            {
                Vector3 currentMousePos = Input.mousePosition;
                Vector3 dragDirection = currentMousePos - dragOrigin;
                transform.Translate(-dragDirection * cameraSpeed * Time.deltaTime);
                dragOrigin = currentMousePos;
            }
        }
        transform.position = new Vector3(
          Mathf.Clamp(transform.position.x, 0, xMax),
          Mathf.Clamp(transform.position.y, yMin, 0),
          -10
      );

    }

    public void SetLimits(Vector3 maxTile)
    {
        Vector3 wp=Camera.main.ViewportToWorldPoint(new Vector3(1,0));
        xMax = maxTile.x - wp.x;
        yMin = maxTile.y - wp.y;

    }

    public void setCamaraPosition(Vector3 position)
    {
        transform.position = position;
    }
}
