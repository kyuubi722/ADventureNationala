using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class checkForEscape : MonoBehaviour
{
    public GameObject joc;
    public GameObject mc;
    public GameObject house;
    public GameObject menu;
    public GameObject MainMenuCam;
    public GameObject Text;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            joc.SetActive(false);
            house.SetActive(false);
            menu.SetActive(true);
            MainMenuCam.SetActive(true);
            Text.SetActive(false);
            mc.SetActive(false);
        }
    }
}
