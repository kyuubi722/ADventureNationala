using System.Collections;
using UnityEngine;

public class CycleBGauth : MonoBehaviour
{
    public GameObject backgroundCube;
    public Material frame0;
    public Material frame1;
    public Material frame2;
    public Material frame3;
    private Renderer render;
    private Material[] frames;
    //cycles background images (homemade animation :3)//
    void Start()
    {
        render = backgroundCube.GetComponent<Renderer>();
        frames = new Material[] { frame0, frame1, frame2, frame3 };
        StartCoroutine(CycleMaterials());
    }

    IEnumerator CycleMaterials()
    {
        while (true)
        {
            // Quick cycle through frames
            for (int i = 0; i < frames.Length; i++)
            {
                render.material = frames[i];
                yield return new WaitForSeconds(0.1f); // Quick cycle, change this value as needed
            }

            // Ensure it resets to frame0 at the end of the cycle
            render.material = frames[0];

            // Wait for 4 seconds before the next quick cycle
            yield return new WaitForSeconds(4f);
        }
    }
}
