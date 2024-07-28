using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensivitySetSCRIPT : MonoBehaviour
{
    //sets sensivity (flat is 300)//
    public float sensivity=300;
    public Camera Settingcam; // volume settigs camera reference
    public static GameObject parentObj; // parent object of sounds reference
    public LayerMask interact; // layer for objects intaraction
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
                // gets the mouse cursor and draws a raycast towards it
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Settingcam.ScreenPointToRay(mousePosition);
                RaycastHit hit;
                // checks if raycast hits an objects
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, interact))
                {   

                    if(hit.collider.gameObject!=null){
                        
                        switch (hit.collider.gameObject.name){
                            case "sensDown": 
                            sensivity-=10;
                            setsens(sensivity);
                                Debug.Log("volumjos");
                                break;
                            case "sensUp": 
                            sensivity+=10;
                            setsens(sensivity);
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Main camera reference is null!"); 
            }
        }
}

    void setsens(float snes){
        CameraMovement.sens = snes;
    }
    }

