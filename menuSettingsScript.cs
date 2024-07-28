using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuSettingsScript : MonoBehaviour
{
    public LayerMask interactableLayer;
    public Camera menucam;
    void Start()
    {
        menucam = GameObject.Find("MainMenuCam").GetComponent<Camera>();
    }

    //updates the languages
    void Update()
    {
        if(gameObject.transform.localPosition== new Vector3(-32f, 1.31f, 16.01f)){
            executeShopCommands();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameObject shopPannel = GameObject.Find("settingsWindowmenu");
            shopPannel.transform.localPosition = new Vector3(0, 0, 0);
            WorkCamScript.aWindowIsOpened = false;
            MainMenuCamScript.openedMenu=false;
        }
    }
      void executeShopCommands(){
         if (Input.GetMouseButtonDown(0))
        {
            Ray ray = menucam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                switch (hit.collider.gameObject.name){
                    
                    
                    case "LbRO":{
                        Debug.Log("romana");
                        Debug.Log("true");
                        break;
                    }
                    case "LbEN":{
                        Debug.Log("tailandeza");
                        Debug.Log("false");
                        break;
                    }
                }
            }
        }
    }
}
