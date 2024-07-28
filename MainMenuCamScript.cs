using System.Runtime.Remoting.Messaging;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class MainMenuCamScript : MonoBehaviour
{   
    public LayerMask ClickableObject;
    public GameObject restrictedmenu;
    public static bool authentified=false;
    public static int Role=1;
    private GameObject SignInCanvas;
    private GameObject LogInCanvas;
    public static bool openedMenu=false;
    public AudioSource menuMusic;

    public GameObject adminPannel;
    public Camera mainCamera;
    public GameObject Mainmenu;
    public GameObject settings;
    public GameObject tutorial;
    public GameObject authMenu;


    //at the start of the application
    void Start(){
        SignInCanvas = GameObject.Find("SignUpForm");
            SignInCanvas.SetActive(false);
        LogInCanvas = GameObject.Find("LogInForm");
        LogInCanvas.SetActive(false);
        authentified = false;
    }

    //running every frame
    void Update()
    {   
        if(Role!=3){
        if(Role==2){
            openAdminMenu();
        }else{
        if (Input.GetMouseButtonDown(0)&&openedMenu==false)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (mainCamera != null)
            {
                if(!authentified){
                    checkForAuth();
                }
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ClickableObject)&&authentified)
                {   
                        if(hit.collider.gameObject!=null){
                            seeAction(hit.collider.gameObject.name);
                        }
                    }
                }
            }
        }
    }else{
       restrictedmenu.SetActive(true);
    }
}

    //identifies the scene
    void seeAction(string name){
        switch(name){
            case "StartGame":{
                mainmenuscripr.GameStarted=true;
                break;
            }
            case "Settings":{
                openedMenu=true;
                settings.transform.localPosition = new Vector3(-32f, 1.31f,16.01f);
                break;
            }
            case "Tutorial":{
                openedMenu=true;
                tutorial.transform.localPosition = new Vector3(-32f, 1.31f,16.01f);
                break;
            }
            case "Quit": {
                Application.Quit();
                break;
            }
        }
    }

    //raycast to detect if the mouse is clicking on certain UI elements
    void checkForAuth(){
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        // performs a raycast to see if it hits a clickable object
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, ClickableObject)){
                switch(hit.collider.gameObject.name){
                    case "LogIn":{
                        LogInCanvas.SetActive(true);
                        authMenu.SetActive(false);
                    }break;
                    case "SignUp":{
                        SignInCanvas.SetActive(true);    
                        authMenu.SetActive(false);
                    }break;
                    case "Quit": {
                    Application.Quit();
                    break;
                    }
                }
            }
        }
    void openAdminMenu(){ 
            adminPannel.SetActive(true);
            menuMusic.Stop();
        }
    }

