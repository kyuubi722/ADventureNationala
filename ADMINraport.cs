using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;
using System;

public class ADMINraport : MonoBehaviour
{
    private int RaportControlNR;
    private const int batchSize = 5;
    private int[] reportsID = new int[]{
        0,0,0,0,0
    };

    void Start()
    {
        RaportControlNR = 0;
        LoadRaports();
    }

    public void IncrementVar()
    {
        RaportControlNR += batchSize;
        LoadRaports();
    }

    public void DecrementVar()
    {
        if (RaportControlNR >= batchSize)
        {
            RaportControlNR -= batchSize;
            LoadRaports();
        }
    }

    private void LoadRaports()
{
    DBcon connection = new DBcon("users");
    if (connection.OpenConnection())
    {
        using (MySqlConnection conn = connection.dbConnection)
        {
            string query = $"SELECT * FROM reports LIMIT {batchSize} OFFSET {RaportControlNR}";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int index = 0;
                    // Clear all UI elements initially
                    for (int i = 0; i < batchSize; i++)
                    {
                        TMP_Text semnalareText = GameObject.Find($"Semnalare{i}").GetComponent<TMP_Text>();
                        TMP_Text intrebareText = GameObject.Find($"Intrebare{i}").GetComponent<TMP_Text>();
                        TMP_Text utilizatorText = GameObject.Find($"Utilizator{i}").GetComponent<TMP_Text>();
                        semnalareText.text = "";
                        intrebareText.text = "";
                        utilizatorText.text = "";
                    }

                    // Populate UI elements with fetched data
                    while (reader.Read() && index < batchSize)
                    {
                        TMP_Text semnalareText = GameObject.Find($"Semnalare{index}").GetComponent<TMP_Text>();
                        TMP_Text intrebareText = GameObject.Find($"Intrebare{index}").GetComponent<TMP_Text>();
                        TMP_Text utilizatorText = GameObject.Find($"Utilizator{index}").GetComponent<TMP_Text>();
                        reportsID[index] = reader.GetInt32("ReportID");
                        semnalareText.text = reader["ReportTXT"].ToString();
                        intrebareText.text = reader["QuestionTXT"].ToString();
                        utilizatorText.text = reader["SenderEmail"].ToString();

                        index++;
                    }

                    // Clear remaining UI elements if fewer than batchSize rows were fetched
                    for (int i = index; i < batchSize; i++)
                    {
                        TMP_Text semnalareText = GameObject.Find($"Semnalare{i}").GetComponent<TMP_Text>();
                        TMP_Text intrebareText = GameObject.Find($"Intrebare{i}").GetComponent<TMP_Text>();
                        TMP_Text utilizatorText = GameObject.Find($"Utilizator{i}").GetComponent<TMP_Text>();

                        semnalareText.text = "";
                        intrebareText.text = "";
                        utilizatorText.text = "";
                    }
                }
            }
        }
        connection.CloseConnection();
    }
}
    public void deleteReport(int reportnr)
{
    // Check if reportnr is within valid range
    if (reportnr >= 0 && reportnr < batchSize)
    {
        DBcon connection = new DBcon("users");
        if (connection.OpenConnection())
        {
            using (MySqlConnection conn = connection.dbConnection)
            {
                string query = "DELETE FROM reports WHERE ReportID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", reportsID[reportnr]);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Error deleting report: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                        LoadRaports(); // Reload data after deletion
                    }
                }
            }
        }
    }
    else
    {
        Debug.LogWarning("Invalid report index for deletion: " + reportnr);
    }
}

}
