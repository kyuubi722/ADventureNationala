using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickSoundScript : MonoBehaviour
{
    private AudioSource clicksound; // Referință către componenta AudioSource pentru sunetul de clic
    private GameObject mc; // Referință către obiectul de caracter principal

    void Start()
    {
        // Nu se face nimic în Start în acest caz
    }

    // Metoda care verifică apăsarea clicului în diferite condiții pentru a reda sunetul
    void Update()
    {
        // Găsește toate obiectele active din scenă
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {
            // Verifică dacă numele obiectului este "MainCharacter" și îl salvează în mc
            if (obj.name == "MainCharacter")
                mc = obj;
        }
        // Verifică dacă s-a apăsat butonul de mouse și dacă obiectul caracterului principal este inactiv
        if (Input.GetMouseButtonDown(0) && (mc.activeSelf == false))
        {
            // Redă sunetul de clic
            playclick();
        }
    }

    // Metoda care redă sunetul de clic
    public void playclick()
    {
        // Generează un număr aleatoriu între 1 și 4 pentru a selecta sunetul de clic
        int clicktype = Random.Range(1, 5);
        // Găsește componenta AudioSource asociată sunetului de clic
        clicksound = GameObject.Find("click" + clicktype).GetComponent<AudioSource>();
        // Redă sunetul
        clicksound.Play();
    }
}
