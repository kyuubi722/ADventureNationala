using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public Behaviour gamescript;
    //saves the answers localy//
    public static int[] CorrectanswersC1 = new int[]{
        2,3,4,3,3,2,2,1,2,3,3,3,1,4,4,1,1,1,2,2,2,2,3,1,1,4,4,2,2,4,4,1,2,1,2,1,1,3,4,4,1,3,2,4,3
    };
    public static int[] CorrectanswersC2 = new int[]{
        3,1,3,4,1,3,2,3,1,4,3,2,2,4,3,4,2,2,4,3,1,2,2,3,3,4,2,3,2,2,1,3,1,4,3,2,4,1,2,4,1,4,2,3,1
    };
    public static int[] CorrectanswersC3 = new int[]{
        2,4,3,3,2,4,3,1,1,3,1,1,2,1,4,3,4,2,3,4,3,2,1,4,2,4,3,3,1,3,1,4,1,1,2,4,3,3,4,3,4,4,3,1,3
    };
    public static int[] CorrectanswersC4 = new int[]{
        3,2,1,2,1,4,1,3,3,4,4,2,2,2,2,3,4,2,1,1,1,3,2,2,4,4,1,2,2,2,3,3,2,2,1,1,1,2,1,4,4,4,4,4,4
    };
    void Start(){
        gamescript.enabled = false;
    }
    void Update()
    {
        if(WorkCamScript.GameStarted){
            gamescript.enabled = true;
        }
        if(WorkCamScript.GameStarted==false){
            gamescript.enabled = false;
        }
    }
}
