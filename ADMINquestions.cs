using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using System.Windows.Forms;
using System.IO;
public class ADMINquestions : MonoBehaviour
{
    //UI ELEMENTS//
    public TMP_InputField questionSearchField;
    public GameObject AddQuestionField;
    public TMP_Text nrintrebaritxt;
    public TMP_InputField AddQuestionText;
    public TMP_InputField AddQuestionRightans;
    public TMP_InputField AddQuestionAns1;
    public TMP_InputField AddQuestionAns2;
    public TMP_InputField AddQuestionAns3;
    public TMP_InputField AddQuestionAns4;
    public TMP_InputField AddQuestionSolution;
    public TMP_InputField ModifcareIntrebareText;
    public TMP_InputField ModifcareIntrebareAns1;
    public TMP_InputField ModifcareIntrebareAns2;
    public TMP_InputField ModifcareIntrebareAns3;
    public TMP_InputField ModifcareIntrebareAns4;
    public TMP_InputField ModifcareIntrebareRaspunsCorect;
    public TMP_InputField ModifcareIntrebareExplicatie;
    //VARS//
    public GameObject dropdownbutton;
    private int IdIntrebareGasita;
    public GameObject AfisareIntrebareCanvas;
    public GameObject ModificareIntrebareCanvas;
    public TMP_Text Afisare_IntrebareText;
    public TMP_InputField numeTabelNou;
    private string FindQuestiontableName;
    private string NumeTabelNou;
    public TMP_Dropdown finddropdown;
    public TMP_Dropdown addDropdown;

    void Update(){
    }
    //load the question ammount//
     void loadQuestionNr(string selectedValue)
    {
        DBcon connection = new DBcon("adventure");
        if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "SELECT COUNT(*) FROM " + selectedValue;
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        object result = cmd.ExecuteScalar();
                        int count = Convert.ToInt32(result);
                        nrintrebaritxt.text = "Exista " + count + " intrebari in categoria " + selectedValue;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Error executing query: " + ex.Message);
                    }
                    finally
                    {
                        CloseAddQuestionSolo();
                        conn.Close();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Unable to open database connection.");
        }
    }
    //Initialize dropdowns and listener//
    void Start()
    {
         finddropdown.onValueChanged.AddListener(delegate
        {
            loadQuestionNr(finddropdown.options[finddropdown.value].text);
        });
        LoadFindDropDown();
        LoadAddDropDown();
    }
    //Open Dialog for adding questions from txt files//
     public void OpenDialog()
{
    FindQuestiontableName = getSelectedElementAdd();
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Title = "Select a .txt file";
    openFileDialog.Filter = "Text Files (*.txt)|*.txt";
    openFileDialog.InitialDirectory = @"C:\"; // Set initial directory to C:\

    if (openFileDialog.ShowDialog() == DialogResult.OK)
    {
        string selectedFilePath = openFileDialog.FileName;
        if (Path.GetExtension(selectedFilePath).ToLower() == ".txt")
        {
            ProcessSelectedFile(selectedFilePath);
        }
        else
        {
            UnityEngine.Debug.Log("Selected file is not a .txt file.");
        }
    }
    else
    {
        UnityEngine.Debug.Log("No file was selected.");
    }
}

//processes added questions//
    private void ProcessSelectedFile(string filePath)
{
    try
    {
        string[] lines = File.ReadAllLines(filePath);

        DBcon connection = new DBcon("adventure");

        if (connection.OpenConnection())
        {
            using (MySqlConnection con = connection.dbConnection)
            {
                foreach (string line in lines)
                {
                    string[] values = line.Split('_');

                    if (values.Length >= 7)
                    {
                        string query = "INSERT INTO " + FindQuestiontableName + " (QuestionTXT, QuestionAns1, QuestionAns2, QuestionAns3, QuestionAns4, QuestionRightAns, QuestionSolution) " +
                            "VALUES (@txt, @ans1, @ans2, @ans3, @ans4, @rightans, @solution)";

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@txt", values[0]);
                            cmd.Parameters.AddWithValue("@ans1", values[1]);
                            cmd.Parameters.AddWithValue("@ans2", values[2]);
                            cmd.Parameters.AddWithValue("@ans3", values[3]);
                            cmd.Parameters.AddWithValue("@ans4", values[4]);
                            cmd.Parameters.AddWithValue("@rightans", values[5]);
                            cmd.Parameters.AddWithValue("@solution", values[6]);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Insufficient data in line. Skipping: " + line);
                    }
                }
            }
            connection.CloseConnection();
            Debug.Log("File data inserted successfully.");
        }
        else
        {
            Debug.LogError("Failed to open database connection.");
        }
    }
    catch (Exception ex)
    {
        Debug.LogError("Error processing file: " + ex.Message);
    }
}
//handles opening different canvases//
    public void OpenAddQuestionSolo(){
        AddQuestionField.SetActive(true);
    }
    public void CloseAddQuestionSolo(){
        AddQuestionField.SetActive(false);
    }
//handles opening different canvases//

//adds a solo question based on different parameteres added by the user//
    public void ConfirmAddQuestionSolo(){
        if(AddQuestionText.text!=""&&AddQuestionAns1.text!=""&&AddQuestionAns2.text!=""&&AddQuestionAns3.text!=""&&AddQuestionAns4.text!=""&&AddQuestionRightans.text!=""&&AddQuestionSolution.text!=""){
        FindQuestiontableName = getSelectedElementAdd();
        DBcon connection = new DBcon("adventure");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "INSERT INTO "+FindQuestiontableName+"(QuestionTXT, QuestionAns1, QuestionAns2, QuestionAns3, QuestionAns4, QuestionRightAns, QuestionSolution) VALUES (@txt, @ans1, @ans2, @ans3, @ans4, @rightans, @solution)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                    cmd.Parameters.AddWithValue("@txt", AddQuestionText.text);
                    cmd.Parameters.AddWithValue("@ans1", AddQuestionAns1.text);
                    cmd.Parameters.AddWithValue("@ans2", AddQuestionAns2.text);
                    cmd.Parameters.AddWithValue("@ans3", AddQuestionAns3.text);
                    cmd.Parameters.AddWithValue("@ans4", AddQuestionAns4.text);
                    cmd.Parameters.AddWithValue("@rightans", AddQuestionRightans.text);
                    cmd.Parameters.AddWithValue("@solution", AddQuestionSolution.text);
                    try{
                        cmd.ExecuteNonQuery();
                    }catch{

                    }finally{
                        AddQuestionAns1.text = "";
                        AddQuestionAns2.text = "";
                        AddQuestionAns3.text = "";
                        AddQuestionAns4.text = "";
                        AddQuestionRightans.text ="";
                        AddQuestionSolution.text = "";
                        AddQuestionText.text = "";
                        CloseAddQuestionSolo();
                        conn.Close();
                    }
                }
            }
        }
        }else{
            TMP_Text errortext = GameObject.Find("NeedAllInputError").GetComponent<TMP_Text>();
            errortext.text = "Este obligatoriu ca toate casetele sa fie completate!";
        }
    }

//adds a table//
    public void AddTable(){
        NumeTabelNou = numeTabelNou.text;
        if(NumeTabelNou!=""){
            DBcon connection = new DBcon("adventure");
            if(connection.OpenConnection()){
                using (MySqlConnection conn = connection.dbConnection){
                    string query = "CREATE TABLE "+NumeTabelNou+" (QuestionID INT AUTO_INCREMENT PRIMARY KEY,QuestionTXT VARCHAR(255) NOT NULL,QuestionAns1 VARCHAR(255) NOT NULL,QuestionAns2 VARCHAR(255) NOT NULL,QuestionAns3 VARCHAR(255) NOT NULL,QuestionAns4 VARCHAR(255) NOT NULL,QuestionRightAns INT(1) NOT NULL,QuestionSolution VARCHAR(255) NOT NULL,QuestionAnswersRight INT(100) NOT NULL,QuestionAnswersWrong INT(100) NOT NULL);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                         try
                        {
                            cmd.ExecuteNonQuery();
                        } catch
                        {
                        }finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            numeTabelNou.text = "";
            LoadFindDropDown();
            LoadAddDropDown();
            AddColumnUtilizatori(NumeTabelNou);
        }
    }
//adds the respective column to the users table//
    void AddColumnUtilizatori(string numetabel){
        DBcon connection = new DBcon("users");
        if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "ALTER TABLE utilizatori ADD COLUMN "+numetabel+" VARCHAR(255) NOT NULL DEFAULT '0';";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
    public void searchQuestion(){
    FindQuestiontableName = getSelectedElementFind();
    DBcon connection = new DBcon("adventure");
    if(connection.OpenConnection()){
        using (MySqlConnection conn = connection.dbConnection){
            string query = "SELECT * FROM " + FindQuestiontableName + " WHERE " +
                           "QuestionID LIKE @id OR " +
                           "QuestionTXT LIKE @txt OR " +
                           "QuestionAns1 LIKE @ans1 OR " +
                           "QuestionAns2 LIKE @ans2 OR " +
                           "QuestionAns3 LIKE @ans3 OR " +
                           "QuestionAns4 LIKE @ans4 OR " +
                           "QuestionSolution LIKE @solution " +
                           "ORDER BY CASE " +
                           "WHEN QuestionID LIKE @id THEN 1 " +
                           "ELSE 2 END LIMIT 1";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                string searchText = "%" + questionSearchField.text + "%";
                cmd.Parameters.AddWithValue("@id", searchText);
                cmd.Parameters.AddWithValue("@txt", searchText);
                cmd.Parameters.AddWithValue("@ans1", searchText);
                cmd.Parameters.AddWithValue("@ans2", searchText);
                cmd.Parameters.AddWithValue("@ans3", searchText);
                cmd.Parameters.AddWithValue("@ans4", searchText);
                cmd.Parameters.AddWithValue("@solution", searchText);

                try
                {
                    // Execute the query
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IdIntrebareGasita = reader.GetInt32("QuestionID");
                            loadAfisareIntrebare(reader["QuestionTXT"].ToString(), reader.GetInt32("QuestionAnswersRight"), reader.GetInt32("QuestionAnswersWrong"));
                            nrintrebaritxt.text = string.Empty;
                        }
                        else
                        {
                            nrintrebaritxt.text = "Nu a fost gasita o intrebare!";
                            AfisareIntrebareCanvas.SetActive(false);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Debug.LogError("MySQL Error: " + ex.Message);
                }
            }
        }
        connection.CloseConnection();
    }
    else
    {
        Debug.LogError("Failed to open connection to database.");
    }
}
//loads found question//
    void loadAfisareIntrebare(string QuestionTXT, int raspunsuriC, int raspunsuriG){
        AfisareIntrebareCanvas.SetActive(true);
        Afisare_IntrebareText.text = QuestionTXT;
        TMP_Text raspunsuriCorecte = GameObject.Find("Raspunsuricorecte").GetComponent<TMP_Text>();
        raspunsuriCorecte.text = "Raspunsuri corecte: "+ raspunsuriC.ToString();
        TMP_Text raspunsuriGresite = GameObject.Find("Raspunsurigresite").GetComponent<TMP_Text>();
        raspunsuriGresite.text = "Raspunsuri gresite: "+ raspunsuriG.ToString(); 
    }
    public void unloadAfisareIntrebare(){
        AfisareIntrebareCanvas.SetActive(false);
    }
    string getSelectedElementFind(){
        // Get the index of the selected option
        int selectedIndex = finddropdown.value;

        // Get the text of the selected option
        string selectedElement = finddropdown.options[selectedIndex].text;

        // Optionally, you can directly return the selected element
        return selectedElement;
    }
    string getSelectedElementAdd(){
        // Get the index of the selected option
        int selectedIndex = addDropdown.value;

        // Get the text of the selected option
        string selectedElement = addDropdown.options[selectedIndex].text;

        // Optionally, you can directly return the selected element
        return selectedElement;
    }

    public void CancelModificareIntrebare(){
        ModificareIntrebareCanvas.SetActive(false);
    }
    public void ModificareIntrebare(){
        ModificareIntrebareCanvas.SetActive(true);
        loadModificareIntrebare();
    }
    //loads modify question//
    void loadModificareIntrebare()
    {
        DBcon connection = new DBcon("adventure");
        if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "SELECT * FROM " + FindQuestiontableName + " WHERE QuestionID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", IdIntrebareGasita);
                    
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()){
                                ModifcareIntrebareText.text = reader.GetString("QuestionTXT");
                                ModifcareIntrebareAns1.text = reader.GetString("QuestionAns1");
                                ModifcareIntrebareAns2.text = reader.GetString("QuestionAns2");
                                ModifcareIntrebareAns3.text = reader.GetString("QuestionAns3");
                                ModifcareIntrebareAns4.text = reader.GetString("QuestionAns4");
                                ModifcareIntrebareRaspunsCorect.text = reader.GetString("QuestionRightAns");
                                ModifcareIntrebareExplicatie.text = reader.GetString("QuestionSolution");
                            }
                            else
                            {
                                Debug.LogWarning("No question found with QuestionID: " + IdIntrebareGasita);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Error loading question from database: " + ex.Message);
                    }
                }
            }
            connection.CloseConnection(); // Close the database connection
        }
        else
        {
            Debug.LogError("Failed to open database connection.");
        }
    }
    public void SubmitModificareIntrebare()
{
    string questionTxt = ModifcareIntrebareText.text;
    string questionAns1 = ModifcareIntrebareAns1.text;
    string questionAns2 = ModifcareIntrebareAns2.text;
    string questionAns3 = ModifcareIntrebareAns3.text;
    string questionAns4 = ModifcareIntrebareAns4.text;
    string questionRightAnswer = ModifcareIntrebareRaspunsCorect.text;
    string questionExplication = ModifcareIntrebareExplicatie.text;

    DBcon connection = new DBcon("adventure");
    if (connection.OpenConnection())
    {
        using (MySqlConnection conn = connection.dbConnection)
        {
            // Start building the base query
            string query = "UPDATE " + FindQuestiontableName + " SET ";
            bool first = true;

            // Build the query dynamically based on non-empty fields
            if (!string.IsNullOrEmpty(questionTxt))
            {
                query += "QuestionTXT = @questionTxt";
                first = false;
            }
            if (!string.IsNullOrEmpty(questionAns1))
            {
                if (!first) query += ", ";
                query += "QuestionAns1 = @questionAns1";
                first = false;
            }
            if (!string.IsNullOrEmpty(questionAns2))
            {
                if (!first) query += ", ";
                query += "QuestionAns2 = @questionAns2";
                first = false;
            }
            if (!string.IsNullOrEmpty(questionAns3))
            {
                if (!first) query += ", ";
                query += "QuestionAns3 = @questionAns3";
                first = false;
            }
            if (!string.IsNullOrEmpty(questionAns4))
            {
                if (!first) query += ", ";
                query += "QuestionAns4 = @questionAns4";
                first = false;
            }
            if (!string.IsNullOrEmpty(questionRightAnswer))
            {
                if (!first) query += ", ";
                query += "QuestionRightAns = @questionRightAnswer";
                first = false;
            }
            if (!string.IsNullOrEmpty(questionExplication))
            {
                if (!first) query += ", ";
                query += "QuestionSolution = @questionExplication";
            }

            query += " WHERE QuestionID = @questionID";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                // Always add the ID parameter
                cmd.Parameters.AddWithValue("@questionID", IdIntrebareGasita);

                // Add parameters only if the corresponding field is not empty
                if (!string.IsNullOrEmpty(questionTxt))
                {
                    cmd.Parameters.AddWithValue("@questionTxt", questionTxt);
                }
                if (!string.IsNullOrEmpty(questionAns1))
                {
                    cmd.Parameters.AddWithValue("@questionAns1", questionAns1);
                }
                if (!string.IsNullOrEmpty(questionAns2))
                {
                    cmd.Parameters.AddWithValue("@questionAns2", questionAns2);
                }
                if (!string.IsNullOrEmpty(questionAns3))
                {
                    cmd.Parameters.AddWithValue("@questionAns3", questionAns3);
                }
                if (!string.IsNullOrEmpty(questionAns4))
                {
                    cmd.Parameters.AddWithValue("@questionAns4", questionAns4);
                }
                if (!string.IsNullOrEmpty(questionRightAnswer))
                {
                    cmd.Parameters.AddWithValue("@questionRightAnswer", questionRightAnswer);
                }
                if (!string.IsNullOrEmpty(questionExplication))
                {
                    cmd.Parameters.AddWithValue("@questionExplication", questionExplication);
                }

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Debug.LogError("MySQL Error: " + ex.Message);
                }
                finally
                {
                    
                    conn.Close();
                }
            }
            }
        }
        unloadAfisareIntrebare();
    CancelModificareIntrebare();
    }

    public void DeleteRow(){
        DBcon connection = new DBcon("adventure");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "DELETE FROM "+FindQuestiontableName+" WHERE QuestionID = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query,conn)){
                    cmd.Parameters.AddWithValue("@id",IdIntrebareGasita);
                    try{ cmd.ExecuteNonQuery();
                    }catch{

                    }finally{
                        conn.Close();
                    }
                }
            }
        }
        unloadAfisareIntrebare();
    }
    void LoadFindDropDown()
    {
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
                        unloadAfisareIntrebare();
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

        finddropdown.ClearOptions();
        finddropdown.AddOptions(tablenames);
    }
    public void DeleteTable(){
        FindQuestiontableName = getSelectedElementAdd();
        DBcon connection = new DBcon("adventure");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "DROP TABLE adventure."+FindQuestiontableName+";";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch 
                    {
                    }
                    finally
                    {
                        conn.Close();
                    }
            }
        }
        }
        LoadFindDropDown();
        LoadAddDropDown();
        deleteUserRow(FindQuestiontableName);
    }
    void deleteUserRow(string columnName){
         DBcon connection = new DBcon("users");
        if(connection.OpenConnection()){
            using (MySqlConnection conn = connection.dbConnection){
                string query = "ALTER TABLE utilizatori DROP COLUMN "+columnName+";";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        conn.Close();
                    }
            }
        }
        }
    }
    void LoadAddDropDown(){
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

        addDropdown.ClearOptions();
        addDropdown.AddOptions(tablenames);
    }
}

