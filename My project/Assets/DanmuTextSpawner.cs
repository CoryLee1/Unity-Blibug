using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DanmuTextSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject textPrefab;
    [SerializeField]
    private float spawnHeight = 400f;
    [SerializeField]
    //private Vector2 spawnRange = new Vector2(-800f, 800f);
    private test test;

    public Transform canvas;

    private void Start()
    {
        test = FindObjectOfType<test>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnText("前方高能");
        }
    }

    public void SpawnText(string content)
    {
        GameObject newText = Instantiate(textPrefab, canvas);
        //content = test.danmuContent;
        Debug.Log("@@@@@"+ content);
        newText.GetComponent<Text>().text = content;
        newText.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-800, 800), spawnHeight);
        //newText.AddComponent<Rigidbody>();
        Destroy(newText, 10f);
    }
}