using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;

public class ADMINjucatori : MonoBehaviour
{
    public TMP_Text FoundUserName;
    public TMP_Dropdown SelectareMaterii;
    public TMP_Text FoundUserRol;
    public TMP_Text scoreText;
    public TMP_Text LoadUserFoundName;
    public TMP_Text LoadUserFoundEmail;
    public TMP_InputField ModifiedName;
    public TMP_InputField ModifiedEmail;
    public TMP_InputField MessageInputField;
    public TMP_InputField SignatureInputField;
    public TMP_Text MessageErrorText;
    private bool isListenerAdded = false;
    public TMP_Text FoundUserID;
    public GameObject modifyUserCanvas;
    public TMP_Text FoundUserEmail;
    public GameObject ShowFoundUserCanvas;
    public TMP_InputField searchPlayerInputField;
    private string[] foundMails = new string[]{
        "", "", "", "", "", "",
    };
    private int FoundID;
    private int FoundRole;
    private int RaportControlNR;
    private const int batchSize = 6;
     void Start()
    {
        RaportControlNR = 0;
        LoadUsers();
    }

    public void Update(){
    if(ShowFoundUserCanvas.activeSelf == true && !isListenerAdded){
        SelectareMaterii.onValueChanged.AddListener(delegate{
            LoadScore(SelectareMaterii.options[SelectareMaterii.value].text);
        });
        isListenerAdded = true;
    } else if (ShowFoundUserCanvas.activeSelf == false && isListenerAdded) {
        // Optionally, remove the listener if the canvas becomes inactive
        SelectareMaterii.onValueChanged.RemoveAllListeners();
        isListenerAdded = false;
    }
}

    void LoadScore(string materie){
        DBcon connection = new DBcon("users");
    if(connection.OpenConnection()){
        using (MySqlConnection conn = connection.dbConnection){
            string query = "SELECT " + materie + " FROM utilizatori WHERE UtilizatorID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                cmd.Parameters.AddWithValue("@id", FoundID);
                try{
                    object result = cmd.ExecuteScalar();
                    int count = Convert.ToInt32(result);
                    scoreText.text = "Scor: " + count;
                }catch(Exception ex){
                    Debug.LogError("Error loading score: " + ex.Message);
                }finally{
                    conn.Close();
                }
            }
        }
    } else {
        Debug.LogError("Unable to open database connection.");
    }
}
    public void IncrementVar()
    {
        RaportControlNR += batchSize;
        LoadUsers();
    }
    public void loadDropdown(){
        List<string> tablenames = new List<string>();
        DBcon connection = new DBcon("adventure");
        if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "SHOW TABLES;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tablenames.Add(reader.GetString(0));
                            }
                        }
                    }
                    catch (MySqlException ex)   
                    {
                        Debug.LogError("MySQL Error: " + ex.Message);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError("General Error: " + ex.Message);
                    }
                    finally{
                        conn.Close();
                    }
                }
            }
            connection.CloseConnection();
        }
        else
        {
            Debug.LogError("Failed to open connection to database.");
        }

        SelectareMaterii.ClearOptions();
        SelectareMaterii.AddOptions(tablenames);
    }
    public void DecrementVar()
    {
        if (RaportControlNR >= batchSize)
        {
            RaportControlNR -= batchSize;
            LoadUsers();
        }
    }
    //loads more users depending on batchSize//
     private void LoadUsers()
    {
    DBcon connection = new DBcon("users");
    if (connection.OpenConnection())
    {
        using (MySqlConnection conn = connection.dbConnection)
        {
            string query = $"SELECT * FROM utilizatori LIMIT {batchSize} OFFSET {RaportControlNR}";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int index = 0;
                    for (int i = 0; i < batchSize; i++)
                    {
                        LoadUserFoundName = GameObject.Find($"FoundUser{i}").GetComponent<TMP_Text>();
                        LoadUserFoundEmail = GameObject.Find("FoundUserMailForLoad"+i).GetComponent<TMP_Text>();
                        LoadUserFoundName.text = "";
                        LoadUserFoundEmail.text = "";
                    }

                    // Populate UI elements with fetched data
                    while (reader.Read() && index < batchSize)
                    {
                        
                        LoadUserFoundName = GameObject.Find($"FoundUser{index}").GetComponent<TMP_Text>();
                        LoadUserFoundEmail = GameObject.Find("FoundUserMailForLoad"+index).GetComponent<TMP_Text>();
                        LoadUserFoundName.text = reader["Nume"].ToString();
                        LoadUserFoundEmail.text = reader["Email"].ToString();
                        foundMails[index]= reader["Email"].ToString();
                        index++;
                    }

                    // Clear remaining UI elements if fewer than batchSize rows were fetched
                    for (int i = index; i < batchSize; i++)
                    {
                        
                        LoadUserFoundName = GameObject.Find($"FoundUser{i}").GetComponent<TMP_Text>();
                        LoadUserFoundEmail = GameObject.Find("FoundUserMailForLoad"+i).GetComponent<TMP_Text>();
                        LoadUserFoundName.text = "";
                        LoadUserFoundEmail.text = "";
                    }
                }
            }
        }
        connection.CloseConnection();
    }
}
        public void ModifyUserButton(int questionNR){
            string FoundEmail=GameObject.Find("FoundUserMailForLoad"+questionNR).GetComponent<TMP_Text>().text;
            DBcon connection = new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "SELECT * FROM utilizatori WHERE Email= @email";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@email", FoundEmail);
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("UtilizatorID");
                                string username = reader["Nume"].ToString();
                                string useremail = reader["Email"].ToString();
                                int rol = reader.GetInt32("Rol");
                                FoundRole = reader.GetInt32("Rol");
                                FoundID = reader.GetInt32("UtilizatorID");
                                loadFoundUser(id, username, useremail, rol);
                            }
                            else
                            {
                                ShowFoundUserCanvas.SetActive(false);
                                Debug.Log("problem");
                            }
                        }
                    }catch{
                        
                    }finally{
                        conn.Close();
                    }
                }
            }
        }
    }
    public void SubmitSearchPlayer(){
        DBcon connection = new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "SELECT * FROM utilizatori WHERE UtilizatorID = @id OR " +
                               "Nume = @nume OR " +
                               "Email = @email LIMIT 1";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@id", searchPlayerInputField.text);
                    cmd.Parameters.AddWithValue("@nume", searchPlayerInputField.text);
                    cmd.Parameters.AddWithValue("@email", searchPlayerInputField.text);
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MessageErrorText.text="";
                                int IdUtilizatorGasit = reader.GetInt32("UtilizatorID");
                                int Rol = reader.GetInt32("Rol");
                                FoundRole = reader.GetInt32("Rol");
                                FoundID = reader.GetInt32("UtilizatorID");
                                loadFoundUser(IdUtilizatorGasit,reader["Nume"].ToString(),reader["Email"].ToString(), Rol);
                            }
                            else
                            {
                                ShowFoundUserCanvas.SetActive(false);
                                Debug.Log("problem");
                            }
                        }
                    }catch(Exception ex){
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }finally{
                        connection.CloseConnection();
                    }
                }
            }
        }
    }
    void loadFoundUser(int id, string username, string useremail, int rol){
        loadDropdown();
        ShowFoundUserCanvas.SetActive(true);
        FoundUserName.text = username;
        FoundUserEmail.text = useremail;
        FoundUserID.text = id.ToString();
        if (rol == 3){
            FoundUserRol.text = "Acest utilizator este restrictionat!";
            TextMeshProUGUI restrictedButton = GameObject.Find("RestrictedButtonTEXT").GetComponent<TextMeshProUGUI>();
            restrictedButton.text = "Elibereaza restrictia";
        }
        else if(rol==1){
            FoundUserRol.text = "";
            TextMeshProUGUI restrictedButton = GameObject.Find("RestrictedButtonTEXT").GetComponent<TextMeshProUGUI>();
            restrictedButton.text = "Restrictioneaza";
        }else if(rol==2){
            FoundUserRol.text = "Administratorul nu poate fi restrictionat";
            TextMeshProUGUI restrictedButton = GameObject.Find("RestrictedButtonTEXT").GetComponent<TextMeshProUGUI>();
            restrictedButton.text = "Administrator";
        }
    }
    public void DeleteUser(){
        DBcon connection = new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "DELETE FROM utilizatori WHERE UtilizatorID = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@id", FoundID);
                    try{
                        cmd.ExecuteNonQuery();
                    }catch(Exception ex){
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }finally{
                        LoadUsers();
                        ShowFoundUserCanvas.SetActive(false);
                        conn.Close();
                    }
                }
            }
        }
    }
    public void closeFoundUserCanvas(){
        ShowFoundUserCanvas.SetActive(false);
    }
    public void OpenModifyCanvas(){
        modifyUserCanvas.SetActive(true);
    }
    public void CloseModifyCanvas(){
        modifyUserCanvas.SetActive(false);
    }
    public void RestrictUser(){
        DBcon connection = new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "UPDATE utilizatori SET Rol = @rol WHERE UtilizatorID = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                    if(FoundRole==3){
                        cmd.Parameters.AddWithValue("@rol", 1);
                    }else if(FoundRole==1){
                        cmd.Parameters.AddWithValue("@rol",3);
                    }else if(FoundRole==2){
                        cmd.Parameters.AddWithValue("@rol",2);
                    }
                    cmd.Parameters.AddWithValue("@id", FoundID);
                    try{
                        cmd.ExecuteNonQuery();
                    }catch(Exception ex){
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }finally{
                        ShowFoundUserCanvas.SetActive(false);
                        conn.Close();
                    }
                }
            }
        }
    }
    public void ConfirmModifyUser()
{
    DBcon connection = new DBcon("users");
    if (connection.OpenConnection())
    {
        using (MySqlConnection conn = connection.dbConnection)
        {
            // Start with the base query
            string query = "UPDATE utilizatori SET ";
            bool first = true;

            // Check each field and build the query dynamically
            if (!string.IsNullOrEmpty(ModifiedName.text))
            {
                query += "Nume = @nume";
                first = false;
            }
            if (!string.IsNullOrEmpty(ModifiedEmail.text))
            {
                if (!first) query += ", ";
                query += "Email = @email";
            }
            query += " WHERE UtilizatorID = @id;";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", FoundID);
                if (!string.IsNullOrEmpty(ModifiedName.text))
                {
                    cmd.Parameters.AddWithValue("@nume", ModifiedName.text);
                }
                if (!string.IsNullOrEmpty(ModifiedEmail.text))
                {
                    cmd.Parameters.AddWithValue("@email", ModifiedEmail.text);
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error: " + ex.Message);
                }
                finally
                {
                    LoadUsers();
                    CloseModifyCanvas();
                    ShowFoundUserCanvas.SetActive(false);
                    conn.Close();
                }
            }
        }
    }
    }
    public void SubmitSentMessage(){
            if(SignatureInputField.text!=""){
                MessageErrorText.text="";
                DBcon connection = new DBcon("users");
            if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "INSERT INTO messages (Message, MessageSender, MessageReciever) VALUES (@message, @messagesender, @messagereciever);";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@message",MessageInputField.text);
                    cmd.Parameters.AddWithValue("@messagesender",SignatureInputField.text);
                    cmd.Parameters.AddWithValue("@messagereciever",FoundID);
                    try{
                        cmd.ExecuteNonQuery();
                    }catch(Exception ex){
                        Debug.LogError("MySQL Error: " + ex.Message);
                    }finally{
                        SignatureInputField.text = "";
                        MessageInputField.text="";
                        conn.Close();
                    }
                }
            }
        }
        }else{
                MessageErrorText.text="Semnatura este obligatorie!";
            }

    }
}
