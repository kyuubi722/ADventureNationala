using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject Tutorial;
    public Camera tutorialCamera;
    public GameObject Mainmenu;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Mainmenu.SetActive(true);
            mainCamera.enabled= true;
            Tutorial.SetActive(false);
            tutorialCamera.enabled= false;

        }
    }
}
