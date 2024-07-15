using System;
using MySql.Data.MySqlClient;
using UnityEngine;

public class DBcon
{
    private string connectionString;
    public MySqlConnection dbConnection;

    public DBcon(string DatabaseName)
    {
        Initialize(DatabaseName);
    }

    private void Initialize(string DatabaseName)
    {
        connectionString = "Server=localhost;Port=3307;Database=" + DatabaseName + ";Uid=adventureUser;Pwd=;";
    }

    public bool OpenConnection()
    {
        try
        {
            dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
            return true;
        }
        catch (Exception ex)
        {
                Debug.LogError($"Error opening connection: {ex.Message}");
            return false;
        }
    }

    public void CloseConnection()
    {
        try
        {
            if (dbConnection != null && dbConnection.State == System.Data.ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error closing connection: {ex.Message}");
        }
    }

    public void ExecuteQuery(string query)
    {
        try
        {
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error executing query: {ex.Message}");
        }
    }

    public object ExecuteScalar(string query)
    {
        try
        {
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                object result = cmd.ExecuteScalar();
                CloseConnection();
                return result;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error executing scalar query: {ex.Message}");
        }

        return null;
    }

    public MySqlDataReader ExecuteReader(string query)
    {
        try
        {
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                return dataReader;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error executing reader query: {ex.Message}");
        }

        return null;
    }
}
