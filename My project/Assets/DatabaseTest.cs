using System;
using UnityEngine;

public class DatabaseTest : MonoBehaviour
{
    private MySQLConnector mySQLConnector;

    void Start()
    {
        mySQLConnector = new MySQLConnector();

        // ����һЩ��������
        mySQLConnector.Insert("test_user_id", "test_danmucontent", "test_ai_response", DateTime.Now);
    }
}

