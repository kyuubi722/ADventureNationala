using System.Collections;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f; // intial speed
    [SerializeField] private float sprintSpeedMultiplier = 1.5f; 

    public static bool canRun = true; 

    [SerializeField] private float groundDrag = 6f; 
    private Vector3 moveDirection; 

    [HideInInspector] public float walkSpeed; 
    [HideInInspector] public float sprintSpeed; 

    [Header("Keybinds")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift; 
    public static AudioSource walking; 

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2f; 
    private bool grounded; 
    public Transform orientation; 

    private Rigidbody rb; 

    // initialisation
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        walkSpeed = moveSpeed; 
        sprintSpeed = moveSpeed * sprintSpeedMultiplier; 
        walking = GameObject.Find("FootstepsSound").GetComponent<AudioSource>();
    }

    // updates every frame
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f); 
        MyInput(); 
        SpeedControl(); 
    }

    // physical update
    private void FixedUpdate()
    {
        Playsound(); 
        MovePlayer(); 
    }

    // walking sound
    private void Playsound()
    {
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0; // checks if player can move
        if (isMoving)
        {
            if (Input.GetKey(sprintKey) && canRun) 
            {
                walking.pitch = 1.5f; 
            }
            else
            {
                walking.pitch = 1f; 
            }

            if (!walking.isPlaying) 
            {
                walking.Play();
            }
        }
        else
        {
            walking.Stop(); 
        }
    }

    // checks player input
    private void MyInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 
        float verticalInput = Input.GetAxisRaw("Vertical"); 

        if (Input.GetKey(sprintKey) && canRun) 
        {
            moveSpeed = sprintSpeed; 
        }
        else
        {
            moveSpeed = walkSpeed; 
        }
    }

    // player movement
    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 
        float verticalInput = Input.GetAxisRaw("Vertical"); 

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; 
        moveDirection.Normalize(); 

        if (grounded)
        {
            rb.drag = groundDrag;
            rb.AddForce(moveDirection * moveSpeed * 6f, ForceMode.Acceleration);
        }
    }

    // player speed controller
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); 
        if (flatVel.magnitude > moveSpeed) 
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; 
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); 
        }
    }
}
