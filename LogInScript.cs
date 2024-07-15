using UnityEngine;
using TMPro;
using MySql.Data.MySqlClient;
using System;
using System.Collections;

public class LogInScript : MonoBehaviour
{
   public string nume;
   public int rol;
    public string email;
    public string parola;
    public GameObject authmenu;
    public TMP_Text errortext;
    public TMP_InputField emailInput;
    public TMP_InputField parolaInput;
    public void ClosePannel(){
        GameObject LogInCanvas = GameObject.Find("LogInForm");
            LogInCanvas.SetActive(false);
             authmenu.SetActive(true);
    }
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            GameObject LogInCanvas = GameObject.Find("LogInForm");
            LogInCanvas.SetActive(false);
             authmenu.SetActive(true);
        }
    }
    public void buttonPress(){
        email=emailInput.text;  
        parola=parolaInput.text;
        if(emailInput.text.Length>=2&&parolaInput.text.Length>=2){
        logareUtilizator(email, parola);
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
        errortext.text = "";
    }
    void logareUtilizator(string email, string parola){
        DBcon connection= new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "SELECT * FROM utilizatori WHERE Email = @Email AND Parola = @Parola;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Parola", parola);
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader()){
                            if(reader.Read()){
                                InteractionHandler.UserName = reader["Nume"].ToString();
                                InteractionHandler.UserMail = email;
                                InteractionHandler.UserID = reader.GetInt32("UtilizatorID");
                                MainMenuCamScript.Role = reader.GetInt32("Rol");
                                GameObject signupCanvas = GameObject.Find("LogInForm");
                                signupCanvas.SetActive(false);
                                MainMenuCamScript.authentified=true;
                                GameObject authMenu = GameObject.Find("AuthMenu");
                                authMenu.transform.localPosition = new Vector3(-99f, -99f, -99f);
                            }else{
                                errortext.text="Autentificare gresita!";
                                StartCoroutine(waitSecond(3));
                            }
                        }
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
}
