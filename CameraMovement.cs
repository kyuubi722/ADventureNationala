using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static float sens; // Sensibilitatea camerei
    private float xRot; // Roțirea pe axa X
    private float yRot; // Roțirea pe axa Y
    public GameObject orientation; // Obiectul pentru orientare

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Blocarea cursorului în centru
        Cursor.visible = true; // Facem cursorul vizibil
    }

    // Script simplu pentru mișcarea camerei
    void Update()
    {
        // Obține mișcarea mouse-ului în pixeli pe secundă și aplică sensibilitatea
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        // Actualizează rotația pe axele X și Y
        yRot += mouseX;
        xRot -= mouseY;

        // Limităm rotația pe axa X între anumite valori pentru a evita mișcările neobișnuite ale camerei
        xRot = Mathf.Clamp(xRot, -65f, 65f);

        // Aplică rotația camerei în funcție de mișcarea mouse-ului
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
    }

    // Actualizează orientarea în funcție de mișcarea mouse-ului în FixedUpdate pentru mișcări uniforme
    void FixedUpdate()
    {
        orientation.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
