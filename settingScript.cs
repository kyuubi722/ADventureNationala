using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class settingScript : MonoBehaviour
{
    public LayerMask interactableLayer; // Stratul pentru obiectele cu care se poate interacționa

    void Start()
    {
        // Nu se face nimic în Start în acest caz
    }

    void Update()
    {
        // Verifică dacă obiectul are poziția specifică pentru a executa comenzile de magazin
        if(gameObject.transform.localPosition== new Vector3(14.76f, 19.82f,0.51f)){
            executeShopCommands();
        }
    }

    // Metoda pentru executarea comenzilor de magazin (limba și volum)
    void executeShopCommands(){
        // Verifică dacă s-a apăsat butonul mouse-ului
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Lansează un raycast către cursorul mouse-ului
            RaycastHit hit;

            // Verifică dacă raycast-ul lovește un obiect din stratul interacționabil
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                // Verifică numele obiectului lovit și execută acțiunile corespunzătoare
                switch (hit.collider.gameObject.name){
                    case "butonX.settings": // Dacă este butonul pentru închiderea setărilor
                        GameObject xbutton = GameObject.Find("butonX.settings");
                        if(xbutton!=null){
                            GameObject shopPannel = GameObject.Find("settingsWindow");
                            shopPannel.transform.localPosition = new Vector3(10, 20, 45); // Mută panoul de setări în afara ecranului
                            WorkCamScript.aWindowIsOpened = false;
                        }
                        break;
                    case "volumeDown": // Dacă este butonul pentru volum mai mic
                        volumeupdown.volume-=0.05f; // Scade volumul cu 0.05
                        setVolume(volumeupdown.volume); // Setează volumul nou
                        break;
                    case "volumeUp.kid": // Dacă este butonul pentru volum mai mare
                        volumeupdown.volume+=0.05f; // Crește volumul cu 0.05
                        setVolume(volumeupdown.volume); // Setează volumul nou
                        break;
                }
            }
        }
    }

    // Metoda pentru setarea volumului pentru toate sursele audio
    void setVolume(float volume){
        Debug.Log("exist");
        Transform[] audiosTransform = volumeupdown.parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in audiosTransform){
            // Obține componenta AudioSource a fiecărui copil
            AudioSource auxSource = t.gameObject.GetComponent<AudioSource>();
            if(auxSource!=null){
                auxSource.volume = volume; // Setează volumul pentru componenta AudioSource
            }
        }
    }
}
