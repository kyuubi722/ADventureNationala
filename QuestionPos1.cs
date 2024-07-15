using UnityEngine;
using System.Collections;
using System.Linq;
using System.Transactions;
using TMPro;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.Common;

public class QuestionPos1 : MonoBehaviour {

    public string QuestionTXT;
    public int QuestionID;
    private bool loaded= false;
    public string QuestionAns1;
    public string QuestionAns2;
    public string QuestionAns3;
    public TMP_FontAsset textFont;
    public string QuestionAns4;
    public string tablename;
    public int QuestionCorrectAns;
    private RunningGameDB gamescript;
    public string position= " ";
    TextMeshPro textIntrebare;
    TextMeshPro textIntrebare1;
    TextMeshPro textIntrebare2;
    TextMeshPro textIntrebare3;
    TextMeshPro textIntrebare4;
    GameObject rightanswer;
    GameObject ans1;
    GameObject ans2;
    GameObject ans3;
    GameObject ans4;
    void Start(){
        GameObject RunningGameObject = GameObject.Find("Joc");
        gamescript = RunningGameObject.GetComponent<RunningGameDB>();
    }
    void Update(){
        if(position!=" "){
            loadtext();
            checkforclick();
            checkforanswer();
        }
    } 
    void checkforclick(){
         if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

             if(Physics.Raycast(ray, out hit)){
                GameObject clickedobject = hit.collider.gameObject;
                Transform gamepannel = gameObject.transform.Find("casetaIntrebare");
                GameObject Pannel = gamepannel.gameObject;
                if(clickedobject == Pannel){
                   positionChange();
                }
            }
        }
    }
    void checkforanswer(){
        Transform answer1child = gameObject.transform.Find("H1aswer1");
        Transform answer2child = gameObject.transform.Find("H1aswer2");
        Transform answer3child = gameObject.transform.Find("H1aswer3");
        Transform answer4child = gameObject.transform.Find("H1aswer4");
        if (answer1child != null&&answer2child != null&&answer3child != null&&answer4child!=null)
        {
            ans1 = answer1child.gameObject;
            ans2 = answer2child.gameObject;
            ans3 = answer3child.gameObject;
            ans4 = answer4child.gameObject;
            rightanswer = GameObject.Find("H1aswer"+ QuestionCorrectAns);
        }
        if(Input.GetMouseButtonDown(0)){
            //verifica daca mausul este apasat//
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)){
                GameObject clickedobject = hit.collider.gameObject;
            //verifica daca raspunsul este corect, respectiv gresit//
                if(clickedobject == rightanswer){
                    answerisright(position);
                 }else if(clickedobject==ans1||clickedobject==ans2||clickedobject==ans3||clickedobject==ans4){
                    if(clickedobject != rightanswer){
                        answeriswrong(position);
                    }
                }
            }
        }
    }
    void answerisright(string position)
    {
    DBcon connection = new DBcon("adventure");
    if (connection.OpenConnection())
    {
        using (MySqlConnection conn = connection.dbConnection)
        {
            string query = "UPDATE "+tablename+" SET QuestionAnswersRight = QuestionAnswersRight + 1 WHERE QuestionID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", QuestionID);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error: " + ex.Message);
                }finally{
                    conn.Close();
                }
            }
        }
    }
    DBcon connection2 = new DBcon("users");
    if (connection2.OpenConnection())
    {
        using (MySqlConnection conn = connection2.dbConnection)
        {
            string query = "UPDATE utilizatori SET " + tablename + " = " + tablename + " + 1 WHERE Email = @Email";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", InteractionHandler.UserMail);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error: " + ex.Message);
                }finally{
                    conn.Close();
                }
            }
        }
    }
    

        RunningGameDB.CorrectAnswer++;
        gamescript.SpawnQuestionsCoroutine(position.ToString());
        loaded = false;
    }

public void answeriswrong(string position)
{
    DBcon connection = new DBcon("adventure");
    if (connection.OpenConnection())
    {
        using (MySqlConnection conn = connection.dbConnection)
        {
            // Corrected the typo in the table name
            string query =  "UPDATE "+tablename+" SET QuestionAnswersWrong = QuestionAnswersWrong + 1 WHERE QuestionID = @id";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", QuestionID);
                try
                {
                   cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error: " + ex.Message);
                }
            }
        }
    }
     DBcon connection2 = new DBcon("users");
    if (connection2.OpenConnection())
    {
        using (MySqlConnection conn = connection2.dbConnection)
        {
            string query = "UPDATE utilizatori SET " + tablename + " = " + tablename + " - 1 WHERE Email = @Email";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", InteractionHandler.UserMail);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error: " + ex.Message);
                }finally{
                    conn.Close();
                }
            }
        }
    }
        AudioSource healthpop = GameObject.Find("HealthPop").GetComponent<AudioSource>();
        healthpop.Play();
        RunningGameDB.lives--;
        RunningGameDB.WrongAnswer++;
        gamescript.SpawnQuestionsCoroutine(position.ToString());
        loaded = false;
    }

    void positionChange(){
        gameObject.transform.localPosition = new Vector3(13.4f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        GameObject auxOBJ = GameObject.Find("HolderPosition2");
        auxOBJ.transform.localPosition = new Vector3(14.49f, auxOBJ.transform.localPosition.y, auxOBJ.transform.localPosition.z);
        GameObject auxOBJ2 = GameObject.Find("HolderPosition3");
        auxOBJ2.transform.localPosition = new Vector3(14.01f, auxOBJ2.transform.localPosition.y, auxOBJ2.transform.localPosition.z);
       RunningGameDB.pos2Xcoord = 14.49f;
        RunningGameDB.pos1Xcoord = 13.4f;
       RunningGameDB.pos3Xcoord = 14.01f;
    }
    void TextCasetPosition(GameObject intrebare, GameObject raspuns1, GameObject raspuns2, GameObject raspuns3, GameObject raspuns4){
        //pozitioneaza textul//
        intrebare.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        raspuns1.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        raspuns2.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        raspuns3.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        raspuns4.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        intrebare.transform.localPosition = new Vector3(-0.07f, 0.749f, -0.06f);
        intrebare.transform.localScale = new Vector3(0.174f, 0.158f, 1f);
       raspuns1.transform.localPosition= new Vector3(-0.11f,-0.1f,-0.350f);
        raspuns2.transform.localPosition= new Vector3(-0.11f,-1.33f,-0.350f);
        raspuns3.transform.localPosition= new Vector3(-0.11f,-0.9f,-0.350f);
        raspuns4.transform.localPosition= new Vector3(-0.11f,-0.51f,-0.350f);
        raspuns1.transform.localScale= new Vector3(0.1742f, 0.207f, 0.2409f);
        raspuns2.transform.localScale= new Vector3(0.1742f, 0.207f, 0.2409f);
        raspuns3.transform.localScale= new Vector3(0.1742f, 0.207f, 0.2409f);
        raspuns4.transform.localScale= new Vector3(0.1742f, 0.207f, 0.2409f);
    }
    void loadtext(){
        //incarca textul pentru intrebare//
        if(loaded==false){
             loaded=true;
            Transform textAux = gameObject.transform.Find("questionText");
            Transform A1Aux = gameObject.transform.Find("answer1Text");
            Transform A2Aux = gameObject.transform.Find("answer2Text");
            Transform A3Aux = gameObject.transform.Find("answer3Text");
            Transform A4Aux = gameObject.transform.Find("answer4Text");
            GameObject textIntrebareObiect = textAux.gameObject;
            GameObject answer1Text = A1Aux.gameObject; 
            GameObject answer2Text = A2Aux.gameObject; 
            GameObject answer3Text = A3Aux.gameObject; 
            GameObject answer4Text = A4Aux.gameObject; 
            TextMeshPro aux = textIntrebareObiect.GetComponent<TextMeshPro>();
            if(aux==null){
                textIntrebareObiect.AddComponent<TextMeshPro>();
                answer1Text.AddComponent<TextMeshPro>();
                answer2Text.AddComponent<TextMeshPro>();
                answer3Text.AddComponent<TextMeshPro>();
                answer4Text.AddComponent<TextMeshPro>();
                textIntrebare = textIntrebareObiect.GetComponent<TextMeshPro>();
                textIntrebare1 = answer1Text.GetComponent<TextMeshPro>();
                textIntrebare2 = answer2Text.GetComponent<TextMeshPro>();
                textIntrebare3 = answer3Text.GetComponent<TextMeshPro>();
                textIntrebare4 = answer4Text.GetComponent<TextMeshPro>();
            }else{
                textIntrebare = textIntrebareObiect.GetComponent<TextMeshPro>();
                textIntrebare1 = answer1Text.GetComponent<TextMeshPro>();
                textIntrebare2 = answer2Text.GetComponent<TextMeshPro>();
                textIntrebare3 = answer3Text.GetComponent<TextMeshPro>();
                textIntrebare4 = answer4Text.GetComponent<TextMeshPro>();
            }
            if(textIntrebare1!=null&&textIntrebare2!=null&&textIntrebare3!=null&&textIntrebare4!=null&&textIntrebare!=null){
                TextCasetPosition(textIntrebareObiect, answer1Text, answer2Text, answer3Text, answer4Text);
                GenerateText(textIntrebare, textIntrebare1, textIntrebare2, textIntrebare3, textIntrebare4);
            }else{
                Debug.Log("A aparut o problema!");
            }
        }
    }
    void GenerateText(TextMeshPro intrebare, TextMeshPro raspuns1, TextMeshPro raspuns2, TextMeshPro raspuns3, TextMeshPro raspuns4){
        intrebare.text = QuestionTXT;
        raspuns1.text = QuestionAns1;
        raspuns2.text = QuestionAns2;
        raspuns3.text = QuestionAns3;
        raspuns4.text = QuestionAns4;
        intrebare.fontSize=10f;
        raspuns1.fontSize = 7f;
        raspuns2.fontSize = 7f;
        raspuns3.fontSize = 7f;
        raspuns4.fontSize = 7f;
        intrebare.color = new Color(45 / 255f, 51 / 255f, 57 / 255f);
        raspuns4.color = new Color(45 / 255f, 51 / 255f, 57 / 255f);
        raspuns3.color = new Color(45 / 255f, 51 / 255f, 57 / 255f);
        raspuns2.color = new Color(45 / 255f, 51 / 255f, 57 / 255f);
        raspuns1.color = new Color(45 / 255f, 51 / 255f, 57 / 255f);
        intrebare.font = textFont;
        raspuns1.font = textFont;
        raspuns2.font = textFont;
        raspuns3.font = textFont;
        raspuns4.font = textFont;
    }
}