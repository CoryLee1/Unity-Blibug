using System;
using UnityEngine;

public class DatabaseTest : MonoBehaviour
{
    private MySQLConnector mySQLConnector;

    void Start()
    {
        mySQLConnector = new MySQLConnector();

        // 插入一些测试数据
        mySQLConnector.Insert("test_user_id", "test_danmucontent", "test_ai_response", DateTime.Now);
    }
}

