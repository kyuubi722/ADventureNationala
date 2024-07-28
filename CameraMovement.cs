using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static float sens; // camera sensibility  
    private float xRot; // x axis rotation
    private float yRot; // y axis rotation
    public GameObject orientation; // orientation object

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // blocks the cursor in the middle
        Cursor.visible = true; // makes the cursor visible
    }

    // camera movement script
    void Update()
    {
        // gets mouse movement in pixels per seconds and applies the sensibility
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        //activates rotation
        yRot += mouseX;
        xRot -= mouseY;

        // limits the rotation on the x axis to reduce unusual camera movement
        xRot = Mathf.Clamp(xRot, -65f, 65f);

        // applies camera rotation based on mouse movement
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
    }

    // updates the orientation based on mouse movement
    void FixedUpdate()
    {
        orientation.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
