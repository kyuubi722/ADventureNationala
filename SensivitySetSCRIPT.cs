using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensivitySetSCRIPT : MonoBehaviour
{
    public float sensivity=300;
    public Camera Settingcam; // Referință la camera pentru setările de volum
    public static GameObject parentObj; // Referință la obiectul părinte al sunetelor
    public LayerMask interact; // Masca stratului pentru interacțiunea cu obiectele
    void Start()
    {
        setsens(sensivity);
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetMouseButtonDown(0))
        {
        if (Settingcam != null)
            {
                // Determină poziția cursorului mouse-ului și trasează un raycast către cursor
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Settingcam.ScreenPointToRay(mousePosition);
                RaycastHit hit;
                // Verifică dacă raycast-ul lovește un obiect în stratul interacțiunii
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, interact))
                {   
                    // Verifică dacă obiectul lovit nu este nul
                    if(hit.collider.gameObject!=null){
                        // Verifică numele obiectului lovit și ajustează volumul în funcție de acesta
                        switch (hit.collider.gameObject.name){
                            case "sensDown": // Dacă este butonul pentru volum mai mic
                            sensivity-=10;
                            setsens(sensivity);
                                Debug.Log("volumjos");
                                break;
                            case "sensUp": // Dacă este butonul pentru volum mai mare
                            sensivity+=10;
                            setsens(sensivity);
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Main camera reference is null!"); // Afiseaza eroare daca referinta la camera principala este nula
            }
        }
}

    void setsens(float snes){
        CameraMovement.sens = snes;
    }
    }

