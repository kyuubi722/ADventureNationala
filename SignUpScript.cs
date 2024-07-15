using TMPro;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Linq;

public class SignUpScript : MonoBehaviour
{
    public string nume;
    public string email;
    private string errortextForEmail= "";
    public string parola;
    public TMP_Text errortext;
    public GameObject authmenu;
    public TMP_InputField numeInput;
    public TMP_InputField emailInput;
    public TMP_InputField parolaInput;
    public void ClosePannel(){
        GameObject signupCanvas = GameObject.Find("SignUpForm");
            signupCanvas.SetActive(false);
             authmenu.SetActive(true);
    }
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameObject signupCanvas = GameObject.Find("SignUpForm");
            signupCanvas.SetActive(false);
             authmenu.SetActive(true);
        }
    }
    public void buttonPress(){
        nume=numeInput.text;
        email=emailInput.text;  
        parola=parolaInput.text;
        if(numeInput.text.Length>=1&&emailInput.text.Length>=2&&parolaInput.text.Length>=2){
        errortext.text="Finalizat cu succes!";
        StartCoroutine(waitSecond(3));
        if(CheckUsedData(email)){
            InteractionHandler.UserName = nume;
            InteractionHandler.UserMail = email;
            adaugaUtilizator(nume, email, parola);
            MainMenuCamScript.authentified=true;
            MainMenuCamScript.Role = 1;
            GameObject signupCanvas = GameObject.Find("SignUpForm");
            signupCanvas.SetActive(false);  
        }else{
            errortext.text=errortextForEmail;
            StartCoroutine(waitSecond(3));

        }
        numeInput.text="";
        emailInput.text="";
        parolaInput.text="";
        }else{
            errortext.text="Te rog introdu toate valorile!";
            StartCoroutine(waitSecond(3));
        }
    }
    IEnumerator waitSecond(int seconds)
    {
        yield return new WaitForSeconds(seconds);
         errortext.text="";
    }

    void adaugaUtilizator(string nume, string email, string parola){
        DBcon connection= new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "INSERT INTO utilizatori (Nume, Email, Parola, Rol) VALUES (@Nume, @Email, @Parola, @Rol)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nume", nume);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Parola", parola);
                    cmd.Parameters.AddWithValue("Rol", 1);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT LAST_INSERT_ID()";
                        int userId = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        // Set the UserID in the InteractionHandler
                        InteractionHandler.UserID = userId;
                    }
                    catch
                    {
                    }
                    finally
                    {
                        connection.CloseConnection();
                    }
                }
            }
        }else{
            Debug.Log("problem:(((");
        }
    }
   bool CheckUsedData(string email) {
    char[] litere = email.ToCharArray();
    if(litere.Contains('@') && litere.Contains('.')){

    }
    else {
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

                return true; // Neither the name nor the email is used
            } catch{
                // Handle the exception as needed
                return false;
            }
        }
    } else {
        return false;
    }
}


}
