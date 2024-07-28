using UnityEngine;
using TMPro;
using MySql.Data.MySqlClient;
using System;
using System.Collections;

public class LogInScript : MonoBehaviour
{
    // User details
    public string nume;
    public int rol;
    public string email;
    public string parola;
    
    // UI elements
    public GameObject authmenu;
    public TMP_Text errortext;
    public TMP_InputField emailInput;
    public TMP_InputField parolaInput;

    // Method to close the login panel and show the authentication menu
    public void ClosePannel()
    {
        GameObject LogInCanvas = GameObject.Find("LogInForm");
        LogInCanvas.SetActive(false);
        authmenu.SetActive(true);
    }

    // Method called every frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject LogInCanvas = GameObject.Find("LogInForm");
            LogInCanvas.SetActive(false);
            authmenu.SetActive(true);
        }
    }

    // Method called when the login button is pressed
    public void buttonPress()
    {
        email = emailInput.text;
        parola = parolaInput.text;

        // Check if email and password inputs are valid
        if (emailInput.text.Length >= 2 && parolaInput.text.Length >= 2)
        {
            logareUtilizator(email, parola);
            emailInput.text = "";
            parolaInput.text = "";
        }
        else
        {
            errortext.text = "Te rog introdu toate valorile!";
            StartCoroutine(waitSecond(3));
        }
    }

    // Coroutine to clear the error message after a delay
    IEnumerator waitSecond(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        errortext.text = "";
    }

    // Method to handle user login
    void logareUtilizator(string email, string parola)
    {
        DBcon connection = new DBcon("users");
        
        // Check if the database connection is open
        if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "SELECT * FROM utilizatori WHERE Email = @Email AND Parola = @Parola;";
                
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Parola", parola);

                    try
                    {
                        // Execute the query and read the results
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // If user is found, set user details and update the UI
                                InteractionHandler.UserName = reader["Nume"].ToString();
                                InteractionHandler.UserMail = email;
                                InteractionHandler.UserID = reader.GetInt32("UtilizatorID");
                                MainMenuCamScript.Role = reader.GetInt32("Rol");

                                GameObject signupCanvas = GameObject.Find("LogInForm");
                                signupCanvas.SetActive(false);
                                MainMenuCamScript.authentified = true;

                                GameObject authMenu = GameObject.Find("AuthMenu");
                                authMenu.transform.localPosition = new Vector3(-99f, -99f, -99f);
                            }
                            else
                            {
                                // If user is not found, display error message
                                errortext.text = "Autentificare gresita!";
                                StartCoroutine(waitSecond(3));
                            }
                        }
                    }
                    catch
                    {
                        // Handle any exceptions that occur during the query execution
                    }
                    finally
                    {
                        // Ensure the database connection is closed
                        connection.CloseConnection();
                    }
                }
            }
        }
        else
        {
            Debug.Log("problem:(((");
        }
    }
}
