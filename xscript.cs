using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xscript : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&RunningGameDB.Canclick){
            GameObject deathScreen = GameObject.Find("EndOfGameScreen");
            deathScreen.transform.localPosition = new Vector3(-99f, -99f, -99f);
            InteractionHandler.canReset = true;
            RunningGameDB.Canclick = false;
        }
    }
}
