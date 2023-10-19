using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] TMP_Text nameField;

    public void Awake()
    {
        SetName();
    }

    void OnEnable()
    {
        SetName();
    }

    void SetName()
    {
        nameField.text = PlayerPrefs.GetString("name", "No name");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Farm");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
