using System.Collections;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f; // Viteza de bază de deplasare a jucătorului
    [SerializeField] private float sprintSpeedMultiplier = 1.5f; // Multiplu pentru viteza de alergare

    public static bool canRun = true; // Posibilitatea de a alerga

    [SerializeField] private float groundDrag = 6f; // Frâna pe sol pentru oprirea jucătorului
    private Vector3 moveDirection; // Direcția de mișcare a jucătorului

    [HideInInspector] public float walkSpeed; // Viteza de mers
    [HideInInspector] public float sprintSpeed; // Viteza de alergare

    [Header("Keybinds")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift; // Tasta pentru a alerga
    public static AudioSource walking; // Sursa audio pentru sunetul de mers

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2f; // Înălțimea jucătorului
    private bool grounded; // Starea de a fi pe pământ
    public Transform orientation; // Orientarea jucătorului

    private Rigidbody rb; // Componenta rigidbody a jucătorului

    // Inițializare
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obținerea componentei rigidbody
        rb.freezeRotation = true; // Blocarea rotației
        walkSpeed = moveSpeed; // Inițializarea vitezei de mers
        sprintSpeed = moveSpeed * sprintSpeedMultiplier; // Inițializarea vitezei de alergare
        walking = GameObject.Find("FootstepsSound").GetComponent<AudioSource>(); // Obținerea sursei audio pentru sunetul de mers
    }

    // Actualizare
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f); // Verificarea dacă jucătorul este pe pământ
        MyInput(); // Verificarea input-ului
        SpeedControl(); // Controlul vitezei
    }

    // Actualizare fizică
    private void FixedUpdate()
    {
        Playsound(); // Redarea sunetului de mers
        MovePlayer(); // Mișcarea jucătorului
    }

    // Redarea sunetului de mers
    private void Playsound()
    {
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0; // Verificarea dacă jucătorul se mișcă
        if (isMoving)
        {
            if (Input.GetKey(sprintKey) && canRun) // Verificarea dacă jucătorul alerge și poate să alerge
            {
                walking.pitch = 1.5f; // Schimbarea tonului sunetului de mers pentru alergare
            }
            else
            {
                walking.pitch = 1f; // Resetarea tonului sunetului de mers pentru mers obișnuit
            }

            if (!walking.isPlaying) // Redarea sunetului de mers dacă nu este deja redat
            {
                walking.Play();
            }
        }
        else
        {
            walking.Stop(); // Oprirea sunetului de mers dacă jucătorul nu se mișcă
        }
    }

    // Verificarea input-ului jucătorului
    private void MyInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Inputul pe axa orizontală (stânga-dreapta)
        float verticalInput = Input.GetAxisRaw("Vertical"); // Inputul pe axa verticală (înainte-înapoi)

        if (Input.GetKey(sprintKey) && canRun) // Verificarea dacă jucătorul apasă tasta pentru a alerga și poate să alerge
        {
            moveSpeed = sprintSpeed; // Setarea vitezei de mers la viteza de alergare
        }
        else
        {
            moveSpeed = walkSpeed; // Setarea vitezei de mers la viteza de bază de mers
        }
    }

    // Mișcarea jucătorului
    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Inputul pe axa orizontală (stânga-dreapta)
        float verticalInput = Input.GetAxisRaw("Vertical"); // Inputul pe axa verticală (înainte-înapoi)

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // Calcularea direcției de mișcare
        moveDirection.Normalize(); // Normalizarea direcției de mișcare

        if (grounded) // Verificarea dacă jucătorul este pe pământ
        {
            rb.drag = groundDrag; // Setarea frânei pe sol
            rb.AddForce(moveDirection * moveSpeed * 6f, ForceMode.Acceleration); // Aplicarea forței pentru mișcare
        }
    }

    // Controlul vitezei jucătorului
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Viteza fără componenta verticală
        if (flatVel.magnitude > moveSpeed) // Verificarea dacă viteza este mai mare decât viteza maximă permisă
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; // Limitarea vitezei la valoarea maximă
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // Setarea vitezei limitate
        }
    }
}
