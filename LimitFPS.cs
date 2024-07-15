using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPS : MonoBehaviour
{
   public int frameLimit = 144;

    void Update()
    {
        Application.targetFrameRate = frameLimit;
    }
}
