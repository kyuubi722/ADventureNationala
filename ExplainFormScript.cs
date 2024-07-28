using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;

public class ExplainFormScript : MonoBehaviour
{
    // References to question position classes
    QuestionPos1 question1;
    QuestionPos2 question2;
    QuestionPos3 question3;

    // UI elements for displaying explanations
    public TMP_Text explanation;
    
    // Static fields to track the question position, table name, and question text
    public static int PositionCalled;
    public static string tablename;
    public static string QuestionName;

    // Game objects for the explanation canvas and submission canvas
    public GameObject explaincanvas;
    public GameObject explainSubmitCanvas;

    // Method to cancel/hide the explanation canvas
    public void cancelCanvas()
    {
        explaincanvas.SetActive(false);
    }

    // Method to confirm and load the explanation submission canvas
    public void confirm()
    {
        Debug.Log(QuestionName + tablename + PositionCalled);
        explainSubmitCanvas.SetActive(true);
        loadexplainsubmitCanvas();
        ReloadExplainedQuestion();
    }

    // Load the explanation submission canvas with the solution from the database
    void loadexplainsubmitCanvas()
    {
        DBcon connection = new DBcon("adventure");
        if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "SELECT QuestionSolution FROM " + tablename + " WHERE QuestionTXT = @txt";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@txt", QuestionName);
                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string solution = reader["QuestionSolution"].ToString();
                                explanation.text = solution;
                            }
                            else
                            {
                                Debug.LogWarning("No solution found for the question.");
                            }
                        }
                    }
                    catch 
                    {
                        // Handle exceptions if needed
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Failed to open database connection.");
        }
    }

    // Method to exit/close all explanation-related canvases
    public void exitall()
    {
        explaincanvas.SetActive(false);
        explainSubmitCanvas.SetActive(false);
    }

    // Reload and display the explained question based on its position
    void ReloadExplainedQuestion()
    {
        // Find the question object based on its position
        GameObject intrebare = GameObject.Find("HolderPosition" + PositionCalled);
        if (intrebare != null)
        {
            switch (PositionCalled)
            {
                case 1:
                    question1 = intrebare.GetComponent<QuestionPos1>();
                    if (question1 != null) goquestion1(question1);
                    else Debug.LogError("QuestionPos1 component not found on HolderPosition1");
                    break;
                case 2:
                    question2 = intrebare.GetComponent<QuestionPos2>();
                    if (question2 != null) goquestion2(question2);
                    else Debug.LogError("QuestionPos2 component not found on HolderPosition2");
                    break;
                case 3:
                    question3 = intrebare.GetComponent<QuestionPos3>();
                    if (question3 != null) goquestion3(question3);
                    else Debug.LogError("QuestionPos3 component not found on HolderPosition3");
                    break;
                default:
                    Debug.LogError("Invalid PositionCalled value: " + PositionCalled);
                    break;
            }
        }
        else
        {
            Debug.LogError("HolderPosition" + PositionCalled + " not found");
        }
    }

    // Call the method to handle a wrong answer for the first question position
    void goquestion1(QuestionPos1 question)
    {
        question.answeriswrong(question.position);
    }

    // Call the method to handle a wrong answer for the second question position
    void goquestion2(QuestionPos2 question)
    {
        question.answeriswrong(question.position);
    }

    // Call the method to handle a wrong answer for the third question position
    void goquestion3(QuestionPos3 question)
    {
        question.answeriswrong(question.position);
    }
}
