using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Unity.Mathematics;

public class InteractionHandler : MonoBehaviour
{
    public static string UserName;
    public static string UserMail;
    public static int UserID;
    public LayerMask interactablelayer;
    // public and private variables declarations
    public LayerMask bed; //bed layer
    public string saveFilePath;
    private bool lightactive = true;
    public LayerMask clickableObj;
    public static string motherString;
    public Camera transitioncam; // transition camera
    public GameObject restOfThings; // remaining objects
    public static bool CanInteract = true; // flag for allowing interactions
    [SerializeField] private LayerMask computerLayer; // pc layer
    [SerializeField] public AudioSource usadeschis; // audio source for opening
    [SerializeField] public AudioSource usaclose; // audio source for closing
    public GameObject player; // player reference
    public GameObject transitionscreen; // transition screen
    public static bool canReset=false; // flag for allowing reset
    public LayerMask changetrack; // layer for allowing to change the song
    private GameObject whiteDot; // white dot 
    public LayerMask radioOnOff; //radio layer
    public GameObject joculPrincipal; // the main game
    public GameObject playerCam; // player's camera
    public Transform RaycastOrigin; // rays origin
    public Animator radioAnimator; // radio animation
    private GameObject interactButton; // intaraction button
    public GameObject light;
    public Animator chairanim;
    public  GameObject gameCam; // game camera
    public RunningGameDB runnginGameNew; // reference to another script
    private GameObject albumparentobj; // parent object for album covers
    private GameObject plantparentobj; // parent object for plants
    private GameObject decoparentobj; // parent object for decorations
    public static bool radioState=false; // radio state
    public bool opened = false; // Flag for opening
    RaycastHit hit; // information regarding the hit object

    void Start(){
        transitioncam.gameObject.SetActive(false);
        whiteDot = GameObject.Find("whiteDot");
        joculPrincipal.SetActive(false); 
        interactButton = GameObject.Find("Interact"); 
        interactButton.SetActive(false); 
        albumparentobj = GameObject.Find("albumCovers"); 
        decoparentobj = GameObject.Find("Decoratiuni");
        plantparentobj = GameObject.Find("Plants"); 
         if (albumparentobj == null) {
        Debug.LogError("Failed to find 'albumCovers' GameObject."); 
    }
    if (decoparentobj == null) {
        Debug.LogError("Failed to find 'Decoratiuni' GameObject."); 
    }
    }
    void handleLight(bool condition){
        AudioSource sunet = GameObject.Find("LightSwitch").GetComponent<AudioSource>();
                GameObject sswitch = GameObject.Find("SwitchForAnim");
                if (condition)
                {
                    sswitch.transform.rotation = Quaternion.Euler(0, -90, 0); // Set rotation to 90 degrees on the X-axis
                } 
                    else
                {
                    sswitch.transform.rotation = Quaternion.Euler(90, -90, 0); // Set rotation to 0 degrees on the X-axis
                }
                sunet.Play();
                light.SetActive(condition);
    }
    void Update()
    { 
        
        // activates the white dot of player is active
        if(player.activeSelf)whiteDot.SetActive(true);  

        if (CanInteract && Input.GetKeyDown(KeyCode.E))
        {
            TryInteract(); 
        }
        checkRange(); 
    }
//checks clicking on certain objects to perform actions//
    void TryInteract()
    {
        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, interactablelayer)){
            if(hit.collider.name == "intrerupator"){
                lightactive=!lightactive;
                handleLight(lightactive);
            }
        }
        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, interactablelayer)){
            if(hit.collider.name == "Chair"){
                chairanim.SetBool("Triggered", !chairanim.GetBool("Triggered"));
            }
        }
        
        // checks the hit object and runs the game in the pc
        if (Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, computerLayer))
        {
            whiteDot.SetActive(false);
            GetIntoComputer(); 
            PlayerMovementTutorial.walking.Stop(); 
        }
        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, bed)&&canReset){
            resetGamendSleep(); 
        }

        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, radioOnOff)){
            OnOffradio(); 
        }

        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, changetrack)){
            changeTrackScript(hit.collider.gameObject); 
        }
    }
    // changing the music
    void changeTrackScript(GameObject hitTrack){
        if(Input.GetKeyDown(KeyCode.E)){
            string name = hitTrack.name;
            string[] nameparts = name.Split(".");
            int.TryParse(nameparts[1], out int trackNumber);
            RadioScript.controlNumber = trackNumber;
        }
    }

    // turns on and off the radio
    void OnOffradio(){
        radioState=!radioState; 
        if(radioState==false){
            RadioScript.firstTime=true;
        }
        radioAnimator.SetBool("RadioState",radioState); // Setarea animației radio-ului
    }

    // main method of resetting the game
    void resetGamendSleep()
    {
        loadData save = new loadData();
        motherString = motherString+ shopScript.musicBuy+ shopScript.plantBuy+ shopScript.decoBuy;
        save.Save();
        WorkCamScript.GameStarted = false;
        RunningGameDB.score = 0;
        RunningGameDB.CorrectAnswer =0; 
        canReset = false;
        runnginGameNew.ResetGame(); 

        // gaining the bought objects 
        string[] musicBought = shopScript.musicBuy.Split('_').Select(str => str.Trim()).ToArray();
        if(musicBought!=null){
            Transform[] musicTransforms = albumparentobj.GetComponentsInChildren<Transform>(true);
            foreach (Transform musicTransform in musicTransforms)
            {
                GameObject music = musicTransform.gameObject;
                if (musicBought.Contains(music.name))
                {
                    music.SetActive(true);
                }
            }
            shopScript.musicBuy = ""; // resetting the shopping list
        }

        // turns on the bought plants
        string[] plantBought = shopScript.plantBuy.Split('_').Select(str => str.Trim()).ToArray();
        if(plantBought!=null){ 
            Transform[] plantTransofrms = plantparentobj.GetComponentsInChildren<Transform>(true);
            foreach (Transform plantTransform in plantTransofrms){
                GameObject plant = plantTransform.gameObject;
                if(plantBought.Contains(plant.name)){
                    plant.SetActive(true);
                }
            }
            shopScript.plantBuy = ""; // resetting the shopping list
        }

        // turns on the bought decorations
        string[] decoBought = shopScript.decoBuy.Split('_').Select(str => str.Trim()).ToArray();
        if(decoBought!=null){
            Transform[] decoTransofrms = decoparentobj.GetComponentsInChildren<Transform>(true);
            foreach (Transform decoTransform in decoTransofrms){
                GameObject deco = decoTransform.gameObject;
                if(decoBought.Contains(deco.name)){
                    deco.SetActive(true);
                }  
            }
            shopScript.decoBuy = ""; // resetting the shopping list
        }

        AudioSource pat = GameObject.Find("Trantesc").GetComponent<AudioSource>();
        pat.Play();


        restOfThings.SetActive(false);
        transitionscreen.SetActive(true);
        transitioncam.gameObject.SetActive(true);

        player.SetActive(false);
        whiteDot.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        wait(); 
        restOfThings.SetActive(true); 
    }

    // waiting method
    IEnumerable wait(){
        yield return new WaitForSeconds(3); 
    }
    // enters the game on the pc
    void GetIntoComputer()
    {   
        joculPrincipal.SetActive(true); 
        CanInteract = false;
        playerCam.SetActive(false); 
        player.SetActive(false); 
        gameCam.SetActive(true); 
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.Confined; 
        checkRange(); 
    }

    // chacks the ray of action
    void checkRange()
    {
        interactButton.SetActive(
            IsInteractable(radioOnOff, true) ||
            IsInteractable(computerLayer, CanInteract) ||
            IsInteractable(bed, CanInteract && canReset) ||
            IsInteractable(changetrack, CanInteract)
        );
    }

    // checks if certain object is interactive
    bool IsInteractable(LayerMask layer, bool condition)
    {
        return condition && Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, layer);
    }
}

