using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;

public class MessagesScript : MonoBehaviour
{
    private int RaportControlNR;
    TMP_Text messageload;
    TMP_Text senderload;
    private const int batchSize = 3;
    public void closeCanvas(){
        gameObject.SetActive(false);
        WorkCamScript.aWindowIsOpened = false;
    }
    void Start(){
        RaportControlNR = 0;
        LoadMessages();
    }
     public void IncrementVar()
    {
        RaportControlNR += batchSize;
        LoadMessages();
    }

    public void DecrementVar()
    {
        if (RaportControlNR >= batchSize)
        {
            RaportControlNR -= batchSize;
            LoadMessages();
        }
    }
    void LoadMessages(){
        DBcon connection = new DBcon("users");
    if (connection.OpenConnection())
    {
        using (MySqlConnection conn = connection.dbConnection)
        {
            string query = $"SELECT * FROM messages WHERE MessageReciever = @id LIMIT {batchSize} OFFSET {RaportControlNR}";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id",InteractionHandler.UserID);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int index = 0;
                    for (int i = 0; i < batchSize; i++)
                    {
                        messageload = GameObject.Find($"LoadText{i}").GetComponent<TMP_Text>();
                        senderload = GameObject.Find("Sender"+i).GetComponent<TMP_Text>();
                        messageload.text = "";
                        messageload.text = "";
                    }

                    // Populate UI elements with fetched data
                    while (reader.Read() && index < batchSize)
                    {
                        
                         messageload = GameObject.Find($"LoadText{index}").GetComponent<TMP_Text>();
                        senderload = GameObject.Find("Sender"+index).GetComponent<TMP_Text>();
                         messageload.text = reader["Message"].ToString();
                        senderload.text = reader["MessageSender"].ToString();
                        index++;
                    }

                    // Clear remaining UI elements if fewer than batchSize rows were fetched
                    for (int i = index; i < batchSize; i++)
                    {
                        
                        messageload = GameObject.Find($"LoadText{i}").GetComponent<TMP_Text>();
                        senderload = GameObject.Find("Sender"+i).GetComponent<TMP_Text>();
                        messageload.text = "";
                        senderload.text = "";
                    }
                }
            }
        }
        connection.CloseConnection();
        }
    }
}

