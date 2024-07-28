using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class settingScript : MonoBehaviour
{
    public LayerMask interactableLayer; // layer for pbjects you can interact with

    void Start()
    {

    }

    void Update()
    {
        // checks if object has correct position
        if(gameObject.transform.localPosition== new Vector3(14.76f, 19.82f,0.51f)){
            executeShopCommands();
        }
    }

    // running the store commands method
    void executeShopCommands(){
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // raycast towards mouse cursor
            RaycastHit hit;

            // checks if raycast hits objects
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                // checks the name of the objects
                switch (hit.collider.gameObject.name){
                    case "butonX.settings": 
                        GameObject xbutton = GameObject.Find("butonX.settings");
                        if(xbutton!=null){
                            GameObject shopPannel = GameObject.Find("settingsWindow");
                            shopPannel.transform.localPosition = new Vector3(10, 20, 45); 
                            WorkCamScript.aWindowIsOpened = false;
                        }
                        break;
                    case "volumeDown": 
                        volumeupdown.volume-=0.05f; 
                        setVolume(volumeupdown.volume); 
                        break;
                    case "volumeUp.kid": 
                        volumeupdown.volume+=0.05f; 
                        setVolume(volumeupdown.volume); 
                        break;
                }
            }
        }
    }

    // setting the volume for every audio method
    void setVolume(float volume){
        Debug.Log("exist");
        Transform[] audiosTransform = volumeupdown.parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in audiosTransform){
            // gets AudioSource component
            AudioSource auxSource = t.gameObject.GetComponent<AudioSource>();
            if(auxSource!=null){
                auxSource.volume = volume; // sets volume
            }
        }
    }
}
