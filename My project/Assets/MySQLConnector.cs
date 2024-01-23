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

    // 构造函数
    public MySQLConnector()
    {
        server = "localhost"; // 数据库所在服务器地址
        database = "your_database"; // 数据库名称
        uid = "username"; // 数据库用户名
        password = "password"; // 数据库密码
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);
    }

    // 打开数据库连接
    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            Debug.Log("数据库连接成功！");
            return true;
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    Debug.LogError("无法连接到服务器。 请联系管理员。");
                    break;

                case 1045:
                    Debug.LogError("无效的用户名/密码，请重试。");
                    break;
            }
            return false;
        }
    }

    // 关闭数据库连接
    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            Debug.Log("数据库连接已关闭！");
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    // 插入新数据
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

