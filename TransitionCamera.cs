using System.Collections; // Library necessary to use IEnumerator and coroutines
using System.Collections.Generic; // Library for generic collections
using UnityEngine; // Main Unity library for basic engine functionalities


public class TransitionCamera : MonoBehaviour
{
    public Camera transitioncam; // Camera used for transitions between days
    public LayerMask interactableLayer; // Layer for interactable objects
    public Camera maincam; // Main camera of the game
    public GameObject whiteDot; // "White dot" object used to indicate the focal point or selection
    public GameObject mc; // Main character or main control object
    public GameObject transitionscreen; // Transition screen between days

    // running once every frame
    void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Generate a ray from the transition camera to the mouse position
            Ray ray = transitioncam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hit an object in the interactable layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                // Check which object was hit and execute the corresponding action
                checkbuttonclick(hit.collider.gameObject.name);
            }
        }
    }

    // Method to check the name of the pressed button and execute the corresponding action
    void checkbuttonclick(string name)
    {
        switch (name)
        {
            case "NextDay": // If the hit button is "NextDay"
                {
                    goToNextDay(); // Go to the next day
                    break;
                }
        }
    }

    // transition to the next day method
    void goToNextDay()
    {
        // Find and play the rooster sound
        AudioSource cocosu = GameObject.Find("Cantacocosu").GetComponent<AudioSource>();
        cocosu.Play();
        // Start a coroutine to wait and activate objects after a delay
        StartCoroutine(WaitAndActivateObjects());
    }

    // coroutine that waits and turns on/off objects
    IEnumerator WaitAndActivateObjects()
    {
        yield return new WaitForSeconds(2.5f); 
        maincam.gameObject.SetActive(true); 
        mc.SetActive(true); 
        transitionscreen.SetActive(false); 
        whiteDot.SetActive(true); 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }
}
