using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainmenuscripr : MonoBehaviour
{
    public GameObject mc;
    public GameObject transitionscreen;
    public GameObject house;
    public GameObject mcCam;
    public GameObject Joc;
    public GameObject MainMenuCam;
    public AudioSource menuMusic;
    public GameObject Text;
    public GameObject mainmenu;
    public static bool GameStarted=false;
    //urmatorul script activeaza elementele ce nu fac parte din meniu si le dezactiveaza pe cele ce fac parte din el
    void Start()
    {
        transitionscreen.SetActive(false);
        mc.SetActive(false);
        house.SetActive(false);
        mcCam.SetActive(false);
        Joc.SetActive(false);
        MainMenuCam.SetActive(true);
        Text.SetActive(false);
        menuMusic.Play();
    }

    void Update()
    {
        if(GameStarted){
            transitionscreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menuMusic.Stop();
            mc.SetActive(true);
            house.SetActive(true); 
            mcCam.SetActive(true);
            Joc.SetActive(true);
            MainMenuCam.SetActive(false);
            Text.SetActive(true);
            mainmenu.SetActive(false);
            GameStarted=false;
        }
    }
}
