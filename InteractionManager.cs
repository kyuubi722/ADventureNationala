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
    // Declarații de variabile publice și private
    public LayerMask bed; // Strat pentru pat
    public string saveFilePath;
    private bool lightactive = true;
    public LayerMask clickableObj;
    public static string motherString;
    public Camera transitioncam; // Camera pentru tranziție
    public GameObject restOfThings; // Obiectele ramase
    public static bool CanInteract = true; // Flag pentru a permite interacțiunile
    [SerializeField] private LayerMask computerLayer; // Strat pentru calculator
    [SerializeField] public AudioSource usadeschis; // Sursa audio pentru deschidere
    [SerializeField] public AudioSource usaclose; // Sursa audio pentru închidere
    public GameObject player; // Referință la jucător
    public GameObject transitionscreen; // Ecranul de tranziție
    public static bool canReset=false; // Flag pentru a permite resetarea
    public LayerMask changetrack; // Strat pentru schimbarea melodiei
    private GameObject whiteDot; // Punctul alb
    public LayerMask radioOnOff; // Strat pentru radio
    public GameObject joculPrincipal; // Jocul principal
    public GameObject playerCam; // Camera jucătorului
    public Transform RaycastOrigin; // Originea razelor
    public Animator radioAnimator; // Animatorul radio-ului
    private GameObject interactButton; // Butonul de interacțiune
    public GameObject light;
    public Animator chairanim;
    public  GameObject gameCam; // Camera jocului
    public RunningGameDB runnginGameNew; // Referință la alt script
    private GameObject albumparentobj; // Obiectul părinte pentru copertele albumelor
    private GameObject plantparentobj; // Obiectul părinte pentru plante
    private GameObject decoparentobj; // Obiectul părinte pentru decorațiuni
    public static bool radioState=false; // Starea radio-ului
    public bool opened = false; // Flag pentru deschidere
    RaycastHit hit; // Informații despre obiectul lovit

    void Start(){
        transitioncam.gameObject.SetActive(false);
        whiteDot = GameObject.Find("whiteDot"); // Găsirea punctului alb
        joculPrincipal.SetActive(false); // Dezactivarea jocului principal
        interactButton = GameObject.Find("Interact"); // Găsirea butonului de interacțiune
        interactButton.SetActive(false); // Dezactivarea butonului de interacțiune inițial
        albumparentobj = GameObject.Find("albumCovers"); // Găsirea obiectului părinte pentru copertele albumelor
        decoparentobj = GameObject.Find("Decoratiuni"); // Găsirea obiectului părinte pentru decorațiuni
        plantparentobj = GameObject.Find("Plants"); // Găsirea obiectului părinte pentru plante
         if (albumparentobj == null) {
        Debug.LogError("Failed to find 'albumCovers' GameObject."); // Afisarea unei erori dacă obiectul părinte pentru copertele albumelor nu este găsit
    }
    if (decoparentobj == null) {
        Debug.LogError("Failed to find 'Decoratiuni' GameObject."); // Afisarea unei erori dacă obiectul părinte pentru decorațiuni nu este găsit
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
        
        // Activarea punctului alb dacă jucătorul este activ
        if(player.activeSelf)whiteDot.SetActive(true);  
        // Verificarea apăsării tastei E și dacă jucătorul poate interacționa
        if (CanInteract && Input.GetKeyDown(KeyCode.E))
        {
            TryInteract(); // Încercarea de interacțiune
        }
        checkRange(); // Verificarea razei de acțiune
    }
//urmatoarea functie verifica apasarea asupra diferitor obiecte care vor desfasura actiuni//
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
        
        // Verificarea obiectului lovit și activarea jocului de pe computer
        if (Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, computerLayer))
        {
            whiteDot.SetActive(false); // Dezactivarea punctului alb
            GetIntoComputer(); // Intrarea în computer
            PlayerMovementTutorial.walking.Stop(); // Oprim sunetul de mers al jucătorului
        }
        // Verificarea obiectului lovit și resetarea jocului dacă jucătorul doarme în pat
        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, bed)&&canReset){
            resetGamendSleep(); // Resetarea jocului și somnul
        }
        // Verificarea obiectului lovit și activarea/dezactivarea radio-ului
        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, radioOnOff)){
            OnOffradio(); // Activarea/dezactivarea radio-ului
        }
        // Verificarea obiectului lovit și schimbarea melodiei radio-ului
        if(Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, changetrack)){
            changeTrackScript(hit.collider.gameObject); // Schimbarea melodiei
        }
    }
    //schimba melodia//
    void changeTrackScript(GameObject hitTrack){
        if(Input.GetKeyDown(KeyCode.E)){
            string name = hitTrack.name;
            string[] nameparts = name.Split(".");
            int.TryParse(nameparts[1], out int trackNumber);
            RadioScript.controlNumber = trackNumber;
        }
    }
    //activeaza sau dezactiveaza radioul//
    void OnOffradio(){
        radioState=!radioState; // Schimbarea stării radio-ului
        if(radioState==false){
            RadioScript.firstTime=true;
        }
        radioAnimator.SetBool("RadioState",radioState); // Setarea animației radio-ului
    }
    //functia principala de resetare a jocului, se ocupa si de spaunarea obiectelor cumparate in ziua respectiva//
    void resetGamendSleep()
    {
        loadData save = new loadData();
        motherString = motherString+ shopScript.musicBuy+ shopScript.plantBuy+ shopScript.decoBuy;
        save.Save();
        // Resetarea unor variabile și pregătirea pentru somn
        WorkCamScript.GameStarted = false;
        RunningGameDB.score = 0;
        RunningGameDB.CorrectAnswer =0; // Resetarea scorului corect
        canReset = false; // Dezactivarea posibilității de resetare
        runnginGameNew.ResetGame(); // Resetarea jocului

        // Obținerea obiectelor cumpărate în ziua respectivă și activarea acestora
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
            shopScript.musicBuy = ""; // Resetarea listei de cumpărături
        }

        // Activarea plantelor cumpărate în ziua respectivă
        string[] plantBought = shopScript.plantBuy.Split('_').Select(str => str.Trim()).ToArray();
        if(plantBought!=null){ 
            Transform[] plantTransofrms = plantparentobj.GetComponentsInChildren<Transform>(true);
            foreach (Transform plantTransform in plantTransofrms){
                GameObject plant = plantTransform.gameObject;
                if(plantBought.Contains(plant.name)){
                    plant.SetActive(true);
                }
            }
            shopScript.plantBuy = ""; // Resetarea listei de cumpărături
        }

        // Activarea decorațiunilor cumpărate în ziua respectivă
        string[] decoBought = shopScript.decoBuy.Split('_').Select(str => str.Trim()).ToArray();
        if(decoBought!=null){
            Transform[] decoTransofrms = decoparentobj.GetComponentsInChildren<Transform>(true);
            foreach (Transform decoTransform in decoTransofrms){
                GameObject deco = decoTransform.gameObject;
                if(decoBought.Contains(deco.name)){
                    deco.SetActive(true);
                }  
            }
            shopScript.decoBuy = ""; // Resetarea listei de cumpărături
        }

        // Pornește sunetul de trantit la pat
        AudioSource pat = GameObject.Find("Trantesc").GetComponent<AudioSource>();
        pat.Play();

        // Dezactivează obiectele nededuse la pat și activează ecranul de tranziție
        restOfThings.SetActive(false);
        transitionscreen.SetActive(true);
        transitioncam.gameObject.SetActive(true);

        // Dezactivează jucătorul și punctul alb, și setează cursorul
        player.SetActive(false);
        whiteDot.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        wait(); // Așteaptă pentru o perioadă de timp
        restOfThings.SetActive(true); // Activează obiectele înapoi
    }

    // Funcție de așteptare
    IEnumerable wait(){
        yield return new WaitForSeconds(3); // Așteaptă 3 secunde
    }
    // Intră în jocul de pe computer
    void GetIntoComputer()
    {   
        joculPrincipal.SetActive(true); // Activează jocul de pe computer
        CanInteract = false; // Dezactivează posibilitatea de interacțiune
        playerCam.SetActive(false); // Dezactivează camera jucătorului
        player.SetActive(false); // Dezactivează jucătorul
        gameCam.SetActive(true); // Activează camera jocului
        Cursor.visible = true; // Setează cursorul vizibil
        Cursor.lockState = CursorLockMode.Confined; // Blocare a cursorului
        checkRange(); // Verifică raza de acțiune
    }

    // Verifică raza de acțiune și activează butonul de interacțiune
    void checkRange()
    {
        interactButton.SetActive(
            IsInteractable(radioOnOff, true) ||
            IsInteractable(computerLayer, CanInteract) ||
            IsInteractable(bed, CanInteract && canReset) ||
            IsInteractable(changetrack, CanInteract)
        );
    }

    // Verifică dacă un obiect este interactiv
    bool IsInteractable(LayerMask layer, bool condition)
    {
        return condition && Physics.Raycast(RaycastOrigin.transform.position, RaycastOrigin.forward, out hit, 2f, layer);
    }
}

