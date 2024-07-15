using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using MySql.Data.MySqlClient;

public class ReportFormScript : MonoBehaviour
{
    public GameObject reportForm;
    public TMP_Text qtxt;
    public TMP_Text qa1;
    public TMP_Text qa2;
    public TMP_Text qa3;
    public GameObject question1;
    public GameObject question2;
    public GameObject question3;
    public TMP_Text qa4;
    public TMP_InputField report;
    public static string questionTXT;
    public static string quetionAns1;
    public static string quetionAns2;
    public static string quetionAns3;
    public static string quetionAns4;
    public static string senderName;
    public static string senderEmail;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            showQuestions();
        }
        qtxt.text = questionTXT;
        qa1.text = quetionAns1;
        qa2.text = quetionAns2;
        qa3.text = quetionAns3;
        qa4.text = quetionAns4;
    }
    public void CloseCanvas(){
        showQuestions();
    }
    public void onButtonPress(){
        if(report.text!=""){
            DBcon connection = new DBcon("users");
            if(connection.OpenConnection()){
                using (MySqlConnection conn = connection.dbConnection){
                    string Query = "INSERT INTO reports (QuestionTXT, ReportTXT, SenderName, SenderEmail) VALUES (@QuestionTXT, @txt, @sendername, @sendermail)";
                    using (MySqlCommand cmd = new MySqlCommand(Query, connection.dbConnection)){
                        cmd.Parameters.AddWithValue("@txt", report.text);
                        cmd.Parameters.AddWithValue("@QuestionTxt", questionTXT);
                        cmd.Parameters.AddWithValue("@sendername", InteractionHandler.UserName);
                        cmd.Parameters.AddWithValue("@sendermail", InteractionHandler.UserMail);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        report.text = "";
                        showQuestions();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Error: " + ex.Message);
                    }
                    finally
                    {
                        connection.CloseConnection();
                    }

                }
            }
        }
    }
    }
    void showQuestions(){
        question1.SetActive(true);
        question2.SetActive(true);
        question3.SetActive(true);
        reportForm.SetActive(false);
        WorkCamScript.GameStarted = true;
    }
}
