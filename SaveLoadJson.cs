using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveLoadJson
{
    public int Cash;
    public int MaxScore;
    public string boughtitems;

    public SaveLoadJson(int cash, int maxScore, string Boughtitems)
    {
        Cash = cash;
        MaxScore = maxScore;
        boughtitems = Boughtitems;
    }
}
