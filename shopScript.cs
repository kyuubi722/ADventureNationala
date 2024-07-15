using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class shopScript : MonoBehaviour
{
    public LayerMask interactableLayer;
    public LayerMask buyMask;
    private GameObject pannelMuzica;
    private GameObject money;
    public static string musicBuy= "";
    public static string decoBuy ="";
    public static string plantBuy = "";
    private GameObject pannelDecoratiuni;
    public static bool canBuyMusic, canBuyDeco, canBuyNature;
    private GameObject pannelPlante;
    private Vector3 focusPos = new Vector3(0, 0,0);
    private Vector3 unfocusPos = new Vector3(0.1f,0,0);
    void Start(){
        pannelMuzica = GameObject.Find("mainShit_muzica");
        pannelDecoratiuni = GameObject.Find("mainShit_decoratiuni");
        pannelPlante = GameObject.Find("mainShit_plante");
        money = GameObject.Find("bani");
        canBuyMusic = true;
        canBuyDeco = false;
        canBuyNature = false;
        loadShop(InteractionHandler.motherString);
    }
    void Update()
    {
        TextMeshPro moneyaux = money.GetComponent<TextMeshPro>();
        moneyaux.text ="Bani: "+ RunningGameDB.Cash;
        if(gameObject.transform.localPosition== new Vector3(14.76f, 19.82f,0.51f)){
            executeShopCommands();
        }
        if(canBuyMusic)buyMusic();
        if(canBuyDeco)buyDeco();
        if(canBuyNature)buyNature();
    }
    //script simplu pentru shop. Concateneaza un string cu diferite elemente predefinite urmand sa fie separat in cuvinte diferite pentru a fi activate(in metoda ResetGame din Interaction Handler)
    void executeShopCommands(){
         if (Input.GetMouseButtonDown(0))
        {
            //loadText();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                switch (hit.collider.gameObject.name){
                    case "butonX":{
                        GameObject xbutton = GameObject.Find("butonX");
                        if(xbutton!=null){
                            GameObject shopPannel = GameObject.Find("ShopObj");
                            shopPannel.transform.localPosition = new Vector3(10, 20, 45);
                            WorkCamScript.aWindowIsOpened = false;
                            pannelMuzica.transform.localPosition = focusPos;
                            pannelDecoratiuni.transform.localPosition = unfocusPos;
                            pannelPlante.transform.localPosition = unfocusPos;
                            canBuyMusic = true;
                            canBuyDeco = false;
                            canBuyNature = false;
                        }
                        break;
                    }
                    case "muzica":{
                        if(pannelMuzica != null){
                            pannelMuzica.transform.localPosition = focusPos;
                            pannelDecoratiuni.transform.localPosition = unfocusPos;
                            pannelPlante.transform.localPosition = unfocusPos;
                            canBuyMusic = true;
                            canBuyDeco = false;
                            canBuyNature = false;
                        }
                       break; 
                    }
                    case "decoratiuni":{
                        if(pannelDecoratiuni != null){
                            pannelMuzica.transform.localPosition = unfocusPos;
                            pannelDecoratiuni.transform.localPosition = focusPos;
                            pannelPlante.transform.localPosition = unfocusPos;
                            canBuyMusic = false;
                            canBuyDeco = true;
                            canBuyNature = false;
                        }
                        break;
                    }
                    case "plante":{
                        if(pannelPlante != null){
                            pannelMuzica.transform.localPosition = unfocusPos;
                            pannelDecoratiuni.transform.localPosition = unfocusPos;
                            pannelPlante.transform.localPosition = focusPos;
                            canBuyMusic = false;
                            canBuyDeco = false;
                            canBuyNature = true;
                        }
                        break;
                    }
                }
            }
        }
    }
    void buyMusic(){
        GameObject buyAux;
        TextMeshPro priceAux;
         if (Input.GetMouseButtonDown(0))
        {   
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buyMask))
            {
                if(RunningGameDB.Cash>=100)
                switch (hit.collider.gameObject.name){
                    case "Track.0.buy":{
                        buyAux = GameObject.Find("Track.0.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        musicBuy = musicBuy + "Track.0_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 100;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "Track.1.buy":{
                        buyAux = GameObject.Find("Track.1.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        musicBuy = musicBuy + "Track.1_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 100;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "Track.2.buy":{
                        buyAux = GameObject.Find("Track.2.price");
                       priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        musicBuy = musicBuy + "Track.2_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 100;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "Track.3.buy":{
                        buyAux = GameObject.Find("Track.3.price");
                       priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        musicBuy = musicBuy + "Track.3_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 100;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "Track.4.buy":{
                        buyAux = GameObject.Find("Track.4.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        musicBuy = musicBuy + "Track.4_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 100;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                }
            }
        }
    }
    void buyDeco(){
        GameObject buyAux;
        TextMeshPro priceAux;
   
         if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buyMask))
            {
                if(RunningGameDB.Cash>=200)
                switch (hit.collider.gameObject.name){
                    case "lampa":{
                        buyAux = GameObject.Find("deco1.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        decoBuy = decoBuy + "deco1_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "books":{
                        buyAux = GameObject.Find("deco3.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        decoBuy = decoBuy + "deco3_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                       priceAux.text="Sold!";
                        break;
                    }
                    case "LGTV":{
                        buyAux = GameObject.Find("deco2.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        decoBuy = decoBuy + "deco2_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "poster.1":{
                        buyAux = GameObject.Find("deco5.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        decoBuy = decoBuy + "deco5_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                       priceAux.text="Sold!";
                        break;
                    }
                    case "poster.2":{
                        buyAux = GameObject.Find("deco6.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        decoBuy = decoBuy + "deco6_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                       priceAux.text="Sold!";
                        break;
                    }
                    case "noptiera":{
                        buyAux = GameObject.Find("deco4.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        decoBuy = decoBuy + "deco4_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                }
            }
        }
    }
    void buyNature(){
        GameObject buyAux;
        TextMeshPro priceAux;
         if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buyMask))
            {
                if(RunningGameDB.Cash>=200)
                switch (hit.collider.gameObject.name){
                    case "plant1":{
                        priceAux = GameObject.Find("plant1.price").GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        plantBuy = plantBuy + "plant1_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "plant2":{
                        buyAux = GameObject.Find("plant2.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        plantBuy = plantBuy + "plant2_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                       
                        break;
                    }
                    case "plant3":{
                        buyAux = GameObject.Find("plant3.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        plantBuy = plantBuy + "plant3_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "plant4":{
                        buyAux = GameObject.Find("plant4.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        plantBuy = plantBuy + "plant4_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                    case "plant5":{
                        buyAux = GameObject.Find("plant5.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        plantBuy = plantBuy + "plant5_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                    priceAux.text="Sold!";
                        break;
                    }
                    case "plant6":{
                        buyAux = GameObject.Find("plant6.price");
                        priceAux = buyAux.GetComponent<TextMeshPro>();
                        if(priceAux.text!="Sold!"){
                        plantBuy = plantBuy + "plant6_";
                        RunningGameDB.Cash = RunningGameDB.Cash - 200;
                        }
                        priceAux.text="Sold!";
                        break;
                    }
                }
            }
        }
    }
  void loadShop(string BoughtItemsList)
{
    if (BoughtItemsList != null)
    {
        string[] BoughtItem = BoughtItemsList.Split('_').Select(str => str.Trim()).ToArray();
        for (int i = 0; i < BoughtItem.Length - 1; i++)
        {
            string item = BoughtItem[i];
            Debug.Log(item);
            TextMeshPro buyText = GameObject.Find(item + ".price")?.GetComponent<TextMeshPro>();
            if (buyText != null)
            {
                buyText.text = "Sold!";
            }
        }
    }
}

}