using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AdminPannelScript : MonoBehaviour
{
    public GameObject intrebariPannel;
    public Sprite Selectat;
    Image aux;
    public Sprite JucatoriSelectat;
    public Sprite JucatoriNeselectat;
    public Sprite IntrebariSelectat;
    public Sprite IntrebariNeselectat;
    public Sprite RaportSelectat;
    public Sprite RaportNeselectat;
    public Sprite Neselectat;
    public GameObject jucatoriPannel;
    public GameObject raporturiPannel;
    //Switches different screens based on what is pressed//
    public void SelectIntrebari(){
        intrebariPannel.SetActive(true);
        jucatoriPannel.SetActive(false);
        raporturiPannel.SetActive(false);
         aux = GameObject.Find("IntrebariBttn").GetComponent<Image>();
            aux.sprite =  Selectat;
            aux = GameObject.Find("JucatoriBttn").GetComponent<Image>();
            aux.sprite =  Neselectat;
            aux = GameObject.Find("RaproturiBttn").GetComponent<Image>();
            aux.sprite =  Neselectat;
            aux = GameObject.Find("RaporturiImageChange").GetComponent<Image>();
            aux.sprite = RaportNeselectat;
            aux = GameObject.Find("JucatoriImageChange").GetComponent<Image>();
            aux.sprite = JucatoriNeselectat;
            aux = GameObject.Find("IntrebariImageChange").GetComponent<Image>();
            aux.sprite = IntrebariSelectat;

    }
    public void SelectJucatori(){
        intrebariPannel.SetActive(false);
        jucatoriPannel.SetActive(true);
        raporturiPannel.SetActive(false);
        aux = GameObject.Find("JucatoriBttn").GetComponent<Image>();
            aux.sprite =  Selectat;
            aux = GameObject.Find("RaproturiBttn").GetComponent<Image>();
            aux.sprite =  Neselectat;
            aux = GameObject.Find("IntrebariBttn").GetComponent<Image>();
            aux.sprite =  Neselectat;
            aux = GameObject.Find("RaporturiImageChange").GetComponent<Image>();
            aux.sprite = RaportNeselectat;
            aux = GameObject.Find("IntrebariImageChange").GetComponent<Image>();
            aux.sprite = IntrebariNeselectat;
            aux = GameObject.Find("JucatoriImageChange").GetComponent<Image>();
            aux.sprite = JucatoriSelectat;

    }  
    public void SelectRaporturi(){
        intrebariPannel.SetActive(false);
        jucatoriPannel.SetActive(false);
        raporturiPannel.SetActive(true);
         aux = GameObject.Find("RaproturiBttn").GetComponent<Image>();
            aux.sprite =  Selectat;
            aux = GameObject.Find("IntrebariBttn").GetComponent<Image>();
            aux.sprite =  Neselectat;
            aux = GameObject.Find("JucatoriBttn").GetComponent<Image>();
            aux.sprite =  Neselectat;
            aux = GameObject.Find("RaporturiImageChange").GetComponent<Image>();
            aux.sprite = RaportSelectat;
            aux = GameObject.Find("JucatoriImageChange").GetComponent<Image>();
            aux.sprite = JucatoriNeselectat;
            aux = GameObject.Find("IntrebariImageChange").GetComponent<Image>();
            aux.sprite = IntrebariNeselectat;
    }
    public void QuitButton(){
        Application.Quit();
    }
}
