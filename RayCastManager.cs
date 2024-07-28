using Unity.VisualScripting;
using UnityEngine;

public class RayCastManager : MonoBehaviour
{ 
    public bool casted;
    public static bool Raycast(Vector3 origin, Vector3 direction, float distance, LayerMask layerMask)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void Update(){
       
    }
}
