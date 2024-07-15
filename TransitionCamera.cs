using System.Collections; // Biblioteca necesară pentru a folosi IEnumerator și corutine
using System.Collections.Generic; // Biblioteca pentru colecții generice
using UnityEngine; // Biblioteca principală Unity pentru funcționalitățile de bază ale motorului

public class TransitionCamera : MonoBehaviour
{
    public Camera transitioncam; // Camera folosită pentru tranziții între zile
    public LayerMask interactableLayer; // Stratul pentru obiectele cu care se poate interacționa
    public Camera maincam; // Camera principală a jocului
    public GameObject whiteDot; // Obiectul "punct alb" utilizat pentru a indica punctul de focalizare sau selecție
    public GameObject mc; // Personajul principal sau obiectul principal de control
    public GameObject transitionscreen; // Ecranul de tranziție între zile

    // Update este apelat o dată pe cadru
    void Update()
    {
        // Verifică dacă s-a făcut un clic stânga al mouse-ului
        if (Input.GetMouseButtonDown(0))
        {
            // Generează un fascicul de raze de la camera de tranziție la poziția mouse-ului
            Ray ray = transitioncam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Verifică dacă fasciculul de raze a lovit un obiect din stratul interactiv
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                // Verifică ce obiect a fost lovit și execută acțiunea corespunzătoare
                checkbuttonclick(hit.collider.gameObject.name);
            }
        }
    }

    // Metoda pentru a verifica numele butonului apăsat și a executa acțiunea corespunzătoare
    void checkbuttonclick(string name)
    {
        switch (name)
        {
            case "NextDay": // Dacă butonul lovit este "NextDay"
                {
                    goToNextDay(); // Merge la următoarea zi
                    break;
                }
        }
    }

    // Metoda pentru a iniția tranziția către ziua următoare
    void goToNextDay()
    {
        // Găsește și redă sunetul cocosului
        AudioSource cocosu = GameObject.Find("Cantacocosu").GetComponent<AudioSource>();
        cocosu.Play();
        // Pornește o corutină pentru a aștepta și activa obiectele după un timp
        StartCoroutine(WaitAndActivateObjects());
    }

    // Corutina care așteaptă un timp și apoi activează/dezactivează obiectele necesare
    IEnumerator WaitAndActivateObjects()
    {
        yield return new WaitForSeconds(2.5f); // Așteaptă 2.5 secunde
        maincam.gameObject.SetActive(true); // Activează camera principală
        mc.SetActive(true); // Activează personajul principal
        transitionscreen.SetActive(false); // Dezactivează ecranul de tranziție
        whiteDot.SetActive(true); // Activează punctul alb
        Cursor.visible = false; // Ascunde cursorul mouse-ului
        Cursor.lockState = CursorLockMode.Locked; // Blochează cursorul mouse-ului la centrul ecranului
    }
}
