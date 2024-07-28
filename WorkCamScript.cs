using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorkCamScript : MonoBehaviour
{
    public GameObject reportForm;
    public AudioListener gamecam;
    public GameObject explainform;
    public GameObject playerCam;
    public GameObject MessageCanvas;
    public GameObject gameCam;
    public static bool GameStarted=false;
    public static bool aWindowIsOpened= false;
    public GameObject player;
    public GameObject joculPrincipal;
    private GameObject shop;
    private GameObject messages;
    private GameObject settings;
    public GameObject reportCanv;
    private GameObject startgameobj;
    private GameObject viewScreen;
    private GameObject shopObj;
    private GameObject settingsObj;
    public Texture2D poppedscreenBG;
    private bool bg;
    public Texture2D normalBG;
    public LayerMask interactableLayer;
    void Start()
    {
        gamecam.enabled = true;
        shop = GameObject.Find("Shop");
        messages = GameObject.Find("Messages");
        settings = GameObject.Find("Settings");
        startgameobj = GameObject.Find("Startgame");    
        viewScreen = GameObject.Find("ViewScreen");
        shopObj = GameObject.Find("ShopObj");
        settingsObj = GameObject.Find("settingsWindow");
        settingsObj.SetActive(false);
        shopObj.SetActive(false);
    }

    void Update()
    {
        if(GameStarted){
            checkforReports();
            checkforExplanation();
        }
        if(!InteractionHandler.CanInteract){
            checkexit();
            if(!aWindowIsOpened){
                CheckMouseClick();
                }
            if(!aWindowIsOpened&&bg==false){
                setNormalBG();
            }
            if(!aWindowIsOpened){
                rollBackToNormal();
            }
            }
    }

    //handles mouse click to show an explanation form when specific objects are clicked
    void checkforExplanation(){
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                switch (hit.collider.gameObject.name){
                    case "Explain1":{
                        explainform.SetActive(true);
                        GameObject intrebare = GameObject.Find("HolderPosition1");
                        QuestionPos1 question = intrebare.GetComponent<QuestionPos1>();
                        if(question!=null){
                        ExplainFormScript.QuestionName = question.QuestionTXT;
                        ExplainFormScript.tablename = question.tablename;
                        }
                        ExplainFormScript.PositionCalled = 1;
                    }break;
                    case "Explain2":{
                        explainform.SetActive(true);
                        GameObject intrebare = GameObject.Find("HolderPosition2");
                        QuestionPos2 question = intrebare.GetComponent<QuestionPos2>();
                        if(question!=null){
                        ExplainFormScript.QuestionName = question.QuestionTXT;
                        ExplainFormScript.tablename = question.tablename;
                        }
                        ExplainFormScript.PositionCalled = 2;
                    }break;
                    case "Explain3":{
                        explainform.SetActive(true);
                        GameObject intrebare = GameObject.Find("HolderPosition3");
                        QuestionPos3 question = intrebare.GetComponent<QuestionPos3>();
                        if(question!=null){
                        ExplainFormScript.QuestionName = question.QuestionTXT;
                        ExplainFormScript.tablename = question.tablename;
                        }
                        ExplainFormScript.PositionCalled = 3;
                    }break;
                }
            }
        }
    }
    void checkforReports(){
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                switch (hit.collider.gameObject.name){
                    case "ExclamationMark":{
                        reportForm.SetActive(true);
                        GameObject intrebare = GameObject.Find("HolderPosition1");
                        QuestionPos1 question = intrebare.GetComponent<QuestionPos1>();
                        ReportFormScript.questionTXT = question.QuestionTXT;
                        ReportFormScript.quetionAns1 = question.QuestionAns1;
                        ReportFormScript.quetionAns2 = question.QuestionAns2;
                        ReportFormScript.quetionAns3 = question.QuestionAns3;
                        ReportFormScript.quetionAns4 = question.QuestionAns4;
                        hideQuestions();
                        }
                        break;
                    case "ExclamationMark2":{
                       reportForm.SetActive(true);
                        GameObject intrebare = GameObject.Find("HolderPosition2");
                        QuestionPos2 question = intrebare.GetComponent<QuestionPos2>();
                        ReportFormScript.questionTXT = question.QuestionTXT;
                        ReportFormScript.quetionAns1 = question.QuestionAns1;
                        ReportFormScript.quetionAns2 = question.QuestionAns2;
                        ReportFormScript.quetionAns3 = question.QuestionAns3;
                        ReportFormScript.quetionAns4 = question.QuestionAns4;
                        hideQuestions();

                    }break;
                    case "ExclamationMark3":{
                        reportForm.SetActive(true);
                        GameObject intrebare = GameObject.Find("HolderPosition3");
                        QuestionPos3 question = intrebare.GetComponent<QuestionPos3>();
                        ReportFormScript.questionTXT = question.QuestionTXT;
                        ReportFormScript.quetionAns1 = question.QuestionAns1;
                        ReportFormScript.quetionAns2 = question.QuestionAns2;
                        ReportFormScript.quetionAns3 = question.QuestionAns3;
                        ReportFormScript.quetionAns4 = question.QuestionAns4;
                        hideQuestions();
                    }break;
                }
            }
        }
    }
    void hideQuestions(){
        for(int i=1; i<4; i++){
            GameObject aux = GameObject.Find("HolderPosition"+i);
            aux.SetActive(false);
        }
    }
    void checkexit(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            InteractionHandler.CanInteract = true;
            joculPrincipal.SetActive(false);
            playerCam.SetActive(true);
            gameCam.SetActive(false);
            player.SetActive(true);
            GameStarted= false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    //sets the default background when nothing is opened//
    void setNormalBG(){
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.mainTexture = normalBG;
        Renderer viewScreenRenderer = viewScreen.GetComponent<Renderer>();
        viewScreenRenderer.material = newMaterial;
    }
    //check interaction with objects for spawning menues//
    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                GameObject clickedObject = hit.collider.gameObject;
                switch (clickedObject.name){
                    case "Startgame":{
                        startgameFunc();
                        break;
                    }
                    case "Settings":{
                        settingsFunc();
                        break;
                    }
                    case "Shop":{
                        shopFunc();
                        break;
                    }
                    case "Messages":{
                        messageFunc();
                        break;
                    }
                }
            }
        }
    }
    void messageFunc(){
        MessageCanvas.SetActive(true);
        aWindowIsOpened = true;
    }
    void rollBackToNormal(){
        GameStarted = false;
        shop.SetActive(true);
        messages.SetActive(true);
        settings.SetActive(true);
        startgameobj.SetActive(true);
    }
    //starts the main game//
    void startgameFunc(){
        aWindowIsOpened = true;
        GameStarted = true;
        shop.SetActive(false);
        messages.SetActive(false);
        settings.SetActive(false);
        startgameobj.SetActive(false);
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.mainTexture = poppedscreenBG;
        bg=false;
        Renderer viewScreenRenderer = viewScreen.GetComponent<Renderer>();
        viewScreenRenderer.material = newMaterial;
    }
    void settingsFunc(){
        aWindowIsOpened = true;
        settingsObj.SetActive(true);
        settingsObj.transform.localPosition = new Vector3(14.76f, 19.82f,0.51f);
    }
    void shopFunc(){
        aWindowIsOpened = true;
        shopObj.SetActive(true);
        shopObj.transform.localPosition = new Vector3(14.76f, 19.82f,0.51f);
        shopScript.canBuyMusic = true;
        shopScript.canBuyDeco = false;
        shopScript.canBuyNature = false;
    }
}
