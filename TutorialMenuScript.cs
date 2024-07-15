using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenuScript : MonoBehaviour
{
   public LayerMask interactableLayer;
    public Camera menucam;
    void Start()
    {
        menucam = GameObject.Find("MainMenuCam").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.localPosition== new Vector3(-32f, 1.31f, 16.01f)){
            executeShopCommands();
        }
    }
      void executeShopCommands(){
         if (Input.GetKeyDown(KeyCode.Escape))
        {
                MainMenuCamScript.openedMenu=false;
                GameObject shopPannel = GameObject.Find("TutorialWindowMenu");
                shopPannel.transform.localPosition = new Vector3(0, 0, 0);
                         
        }
    }
}
