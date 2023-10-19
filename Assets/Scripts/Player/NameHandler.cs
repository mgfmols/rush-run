using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameHandler : MonoBehaviour
{
    [Header("Required Fields")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] TMP_Text inputField;
    [Space]
    [SerializeField] Button enterButton;
    [SerializeField] TMP_Text enterButtonText;

    void Awake()
    {
        if (!PlayerPrefs.GetString("name", "No name").Equals("No name"))
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
            inputField.text = PlayerPrefs.GetString("name");
        }
    }

    void OnEnable()
    {
        if (!PlayerPrefs.GetString("name", "No name").Equals("No name"))
        {
            inputField.text = PlayerPrefs.GetString("name");
        }
    }

    public void EnterName()
    {
        string name = inputField.text;
        PlayerPrefs.SetString("name", name);
    }

    public void ValidateField()
    {
        // Color of the text on the button
        Color faded = new Color(0.322f, 1f, 0f, 0.235f);
        Color nonFaded = new Color(0.322f, 1f, 0f, 1f);

        Regex regex = new Regex("^[a-zA-Z0-9_\\S]{1,20}$");

        if (regex.IsMatch(inputField.text))
        {
            enterButtonText.gameObject.GetComponent<TMP_Text>().color = nonFaded;
            enterButton.enabled = true;
            enterButton.GetComponent<Image>().enabled = true;
        }
        else
        {
            enterButtonText.gameObject.GetComponent<TMP_Text>().color = faded;
            enterButton.enabled = false;
            enterButton.GetComponent<Image>().enabled = false;
        }
    }
}
