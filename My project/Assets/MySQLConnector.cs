using System;
using System.Data;
using MySql.Data.MySqlClient;
using UnityEngine;

public class MySQLConnector
{
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;

    // ���캯��
    public MySQLConnector()
    {
        server = "localhost"; // ���ݿ����ڷ�������ַ
        database = "your_database"; // ���ݿ�����
        uid = "username"; // ���ݿ��û���
        password = "password"; // ���ݿ�����
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }

    // �����ݿ�����
    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            Debug.Log("���ݿ����ӳɹ���");
            return true;
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    Debug.LogError("�޷����ӵ��������� ����ϵ����Ա��");
                    break;

                case 1045:
                    Debug.LogError("��Ч���û���/���룬�����ԡ�");
                    break;
            }
            return false;
        }
    }

    // �ر����ݿ�����
    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            Debug.Log("���ݿ������ѹرգ�");
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    // ����������
    public void Insert(string userId, string danmuContent, string aiResponse, DateTime createdTime)
    {
        string query = $"INSERT INTO your_table (user_id, danmucontent, ai_response, created_time) VALUES('{userId}', '{danmuContent}', '{aiResponse}', '{createdTime.ToString("yyyy-MM-dd HH:mm:ss")}')";

        if (this.OpenConnection() == true)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);

            cmd.ExecuteNonQuery();

            this.CloseConnection();
        }
    }
}

