using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; 
using UnityEngine;
using UnityEngine.Rendering;

public class volumeupdown : MonoBehaviour
{
    public static float volume = 0.5f; // Initial default volume
    public Camera Settingcam; // Reference to the camera for volume settings
    public static GameObject parentObj; // Reference to the parent object of the sounds
    public GameObject parentObj2; // Another reference to a parent object (possibly for other purposes)
    public LayerMask interact; // Layer mask for interacting with objects


    void Start()
    {
        // finds the parent objects of the sounds and sets the initial volume
        parentObj = GameObject.Find("Sounds");
        setVolume(volume);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            if (Settingcam != null)
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Settingcam.ScreenPointToRay(mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, interact))
                {   
                    
                    if(hit.collider.gameObject!=null){
                        
                        switch (hit.collider.gameObject.name){
                            case "volumeDown": 
                                Debug.Log("volumjos");
                                volume-=0.05f; 
                                setVolume(volume); 
                                break;
                            case "volumeUp.kid":
                                volume+=0.05f; 
                                setVolume(volume); 
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

    // sets the volume for all audio sources method
    void setVolume(float volume){

        Transform[] audiosTransform = parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in audiosTransform){
            AudioSource auxSource = t.gameObject.GetComponent<AudioSource>();
            if(auxSource!=null){
                auxSource.volume = volume; 
            }
        }
        Transform[] audio2 = parentObj2.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in audio2){

            AudioSource auxSource = t.gameObject.GetComponent<AudioSource>();
            if(auxSource!=null){
                auxSource.volume = volume; 
            }
        }
    }
}
