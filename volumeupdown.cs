using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Unity.VisualScripting nu este folosit în script și ar putea fi eliminat
using UnityEngine;
using UnityEngine.Rendering;

public class volumeupdown : MonoBehaviour
{
    public static float volume = 0.5f; // Volumul implicit inițial
    public Camera Settingcam; // Referință la camera pentru setările de volum
    public static GameObject parentObj; // Referință la obiectul părinte al sunetelor
    public GameObject parentObj2;
    public LayerMask interact; // Masca stratului pentru interacțiunea cu obiectele

    void Start()
    {
        // Găsește obiectul părinte al sunetelor și setează volumul inițial
        parentObj = GameObject.Find("Sounds");
        setVolume(volume);
    }

    void Update()
    {
        // Verifică dacă s-a apăsat butonul mouse-ului
        if (Input.GetMouseButtonDown(0))
        {
            // Verifică dacă referința la camera de setări este valabilă
            if (Settingcam != null)
            {
                // Determină poziția cursorului mouse-ului și trasează un raycast către cursor
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Settingcam.ScreenPointToRay(mousePosition);
                RaycastHit hit;
                // Verifică dacă raycast-ul lovește un obiect în stratul interacțiunii
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, interact))
                {   
                    // Verifică dacă obiectul lovit nu este nul
                    if(hit.collider.gameObject!=null){
                        // Verifică numele obiectului lovit și ajustează volumul în funcție de acesta
                        switch (hit.collider.gameObject.name){
                            case "volumeDown": // Dacă este butonul pentru volum mai mic
                                Debug.Log("volumjos");
                                volume-=0.05f; // Redu volumul cu 0.05
                                setVolume(volume); // Setează volumul nou
                                break;
                            case "volumeUp.kid": // Dacă este butonul pentru volum mai mare
                                volume+=0.05f; // Crește volumul cu 0.05
                                setVolume(volume); // Setează volumul nou
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Main camera reference is null!"); // Afiseaza eroare daca referinta la camera principala este nula
            }
        }
    }

    // Metodă pentru setarea volumului pentru toate sursele audio
    void setVolume(float volume){
        // Obține toate elementele de tip transofmr ale copiilor obiectului părinte al sunetelor
        Transform[] audiosTransform = parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in audiosTransform){
            // Obține componenta AudioSource a fiecărui copil
            AudioSource auxSource = t.gameObject.GetComponent<AudioSource>();
            if(auxSource!=null){
                auxSource.volume = volume; // Setează volumul pentru componenta AudioSource
            }
        }
        Transform[] audio2 = parentObj2.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in audio2){
            // Obține componenta AudioSource a fiecărui copil
            AudioSource auxSource = t.gameObject.GetComponent<AudioSource>();
            if(auxSource!=null){
                auxSource.volume = volume; // Setează volumul pentru componenta AudioSource
            }
        }
    }
}
