using TMPro;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Linq;

public class SignUpScript : MonoBehaviour
{
    public string nume; // Variable to store the user's name
    public string email; // Variable to store the user's email
    private string errortextForEmail= ""; // Variable to store error messages related to email
    public string parola; // Variable to store the user's password
    public TMP_Text errortext; // TextMeshPro text object for displaying error messages
    public GameObject authmenu; // Reference to the authentication menu GameObject
    public TMP_InputField numeInput; // Input field for the user's name
    public TMP_InputField emailInput; // Input field for the user's email
    public TMP_InputField parolaInput; // Input field for the user's password

    // Method to close the sign-up panel and show the authentication menu
    public void ClosePannel(){
        GameObject signupCanvas = GameObject.Find("SignUpForm");
        signupCanvas.SetActive(false);
        authmenu.SetActive(true);
    }

    // Update method called once per frame to check for the Escape key press
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameObject signupCanvas = GameObject.Find("SignUpForm");
            signupCanvas.SetActive(false);
            authmenu.SetActive(true);
        }
    }

    // Method called when the sign-up button is pressed
    public void buttonPress(){
        nume = numeInput.text;
        email = emailInput.text;  
        parola = parolaInput.text;
        
        // Check if all input fields have valid values
        if(numeInput.text.Length >= 1 && emailInput.text.Length >= 2 && parolaInput.text.Length >= 2){
            errortext.text = "Finalizat cu succes!";
            StartCoroutine(waitSecond(3));
            
            // Check if the email is already used
            if(CheckUsedData(email)){
                // Set user data in InteractionHandler
                InteractionHandler.UserName = nume;
                InteractionHandler.UserMail = email;
                adaugaUtilizator(nume, email, parola);
                MainMenuCamScript.authentified = true;
                MainMenuCamScript.Role = 1;
                
                // Close the sign-up form
                GameObject signupCanvas = GameObject.Find("SignUpForm");
                signupCanvas.SetActive(false);  
            } else {
                errortext.text = errortextForEmail;
                StartCoroutine(waitSecond(3));
            }
            
            // Clear input fields
            numeInput.text = "";
            emailInput.text = "";
            parolaInput.text = "";
        } else {
            errortext.text = "Te rog introdu toate valorile!";
            StartCoroutine(waitSecond(3));
        }
    }

    // Coroutine to wait for a specified number of seconds before clearing the error text
    IEnumerator waitSecond(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        errortext.text = "";
    }

    // Method to add a user to the database
    void adaugaUtilizator(string nume, string email, string parola){
        DBcon connection = new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "INSERT INTO utilizatori (Nume, Email, Parola, Rol) VALUES (@Nume, @Email, @Parola, @Rol)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@Nume", nume);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Parola", parola);
                    cmd.Parameters.AddWithValue("@Rol", 1);

                    try {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT LAST_INSERT_ID()";
                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        // Set the UserID in the InteractionHandler
                        InteractionHandler.UserID = userId;
                    } catch {
                        // Handle exceptions if needed
                    } finally {
                        connection.CloseConnection();
                    }
                }
            }
        } else {
            Debug.Log("problem:(((");
        }
    }

    // Method to check if the email is already used in the database
    bool CheckUsedData(string email) {
        char[] litere = email.ToCharArray();
        if(litere.Contains('@') && litere.Contains('.')){
            // Email format is valid
        } else {
            errortextForEmail = "Introdu un email valid!";
            return false;
        }

        DBcon connection = new DBcon("users");
        if (connection.OpenConnection()) {
            using (MySqlConnection conn = connection.dbConnection) {
                string query2 = "SELECT COUNT(*) FROM utilizatori WHERE Email = @Email";
                try {
                    using (MySqlCommand cmd2 = new MySqlCommand(query2, conn)) {
                        cmd2.Parameters.AddWithValue("@Email", email);
                        int emailCount = Convert.ToInt32(cmd2.ExecuteScalar());
                        
                        if (emailCount > 0) {
                            errortextForEmail = "Email deja in folosinta!";
                            return false; // Email is already used
                        }
                    }

                    return true; // Email is not used
                } catch {
                    // Handle the exception as needed
                    return false;
                }
            }
        } else {
            return false;
        }
    }
}