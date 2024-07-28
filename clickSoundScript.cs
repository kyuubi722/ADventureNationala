using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickSoundScript : MonoBehaviour
{
    private AudioSource clicksound; // AudioSource reference for click sound
    private GameObject mc; // main character reference

    void Start()
    {
        // nothing happens in start
    }

    // methods checks if click is pressed so it can run sounds
    void Update()
    {
        // finds all active objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "MainCharacter")
                mc = obj;
        }

        if (Input.GetMouseButtonDown(0) && (mc.activeSelf == false))
        {
            playclick();
        }
    }

    // method that runs the click sound
    public void playclick()
    {
        int clicktype = Random.Range(1, 5);
        clicksound = GameObject.Find("click" + clicktype).GetComponent<AudioSource>();
        clicksound.Play();
    }
}
