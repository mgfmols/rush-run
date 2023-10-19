using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Data;

public class Leaderboard : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Level level;
    [SerializeField] Menu menu;
    [Header("Leaderboard Objects")]
    [SerializeField] Transform leaderboardContainer;
    [SerializeField] Transform leaderboardTemplate;
    [SerializeField] Transform leaderboardEmptyText;
    [SerializeField] Scrollbar leaderboardScrollbar;
    [SerializeField] ScrollRect leaderboardScrollRect;
    [Header("Buttons")]
    [SerializeField] Button previousButton;
    [SerializeField] TMP_Text previousButtonText;
    [SerializeField] Button nextButton;
    [SerializeField] TMP_Text nextButtonText;

    private LeaderboardList leaderboardEntries;
    private List<Transform> leaderboardEntryTransforms;
    private int page;
    private int amountPerPage;

    public void Awake()
    {
        leaderboardTemplate.gameObject.SetActive(false);
        page = 1;
        amountPerPage = 50;
        leaderboardEntryTransforms = new List<Transform>();
    }

    public void OnEnable()
    {
        // Load leaderboard
        string json = PlayerPrefs.GetString(LevelDict.SaveDataDictionary[level]);
        leaderboardEntries = JsonUtility.FromJson<LeaderboardList>(json);
        if (leaderboardEntries == null)
        {
            leaderboardEntries = new LeaderboardList(new List<LeaderboardEntry>() { new LeaderboardEntry(49.944f, "Dev Brian"), new LeaderboardEntry(58.692f, "Dev Michael") }, level);
            string json2 = JsonUtility.ToJson(leaderboardEntries);
            PlayerPrefs.SetString(LevelDict.SaveDataDictionary[level], json2);
            PlayerPrefs.Save();
        }
        LoadPage();
    }

    private void LoadPage()
    {
        // Sorts leaderboard entries based on time
        if (leaderboardEntries.getLeaderboard().Count == 0)
        {
            leaderboardEmptyText.gameObject.SetActive(true);
        }
        else
        {
            leaderboardEmptyText.gameObject.SetActive(false);
            leaderboardEntries.getLeaderboard().Sort();

            // Loading entries
            foreach(LeaderboardEntry entry in leaderboardEntries.getLeaderboard())
            {
                CreateLeaderboardEntry(entry, leaderboardContainer, leaderboardEntryTransforms);
            }
        }
    }

    public void ClearPage()
    {
        List<Transform> toBeDeleted = new List<Transform>();
        foreach(Transform transform in leaderboardEntryTransforms)
        {
            Destroy(transform.gameObject);
            toBeDeleted.Add(transform);
        }
        foreach(Transform toDelete in toBeDeleted)
        {
            leaderboardEntryTransforms.Remove(toDelete);
        }
    }

    private void CheckButtons()
    {
        // Color of the text on the button
        Color faded = new Color(0.322f, 1f, 0f, 0.235f);
        Color nonFaded = new Color(0.322f, 1f, 0f, 1f);
        // If on first page, disable previous button
        if (page <= 1)
        {
            previousButtonText.gameObject.GetComponent<TMP_Text>().color = faded;
            previousButton.enabled = false;
            previousButton.GetComponent<Image>().enabled = false;
        }
        else
        {
            previousButtonText.gameObject.GetComponent<TMP_Text>().color = nonFaded;
            previousButton.enabled = true;
            previousButton.GetComponent<Image>().enabled = true;
        }

        // If on last page (based on entries), disable next button
        if (page * amountPerPage >= leaderboardEntries.getLeaderboard().Count)
        {
            nextButtonText.gameObject.GetComponent<TMP_Text>().color = faded;
            nextButton.enabled = false;
            nextButton.GetComponent<Image>().enabled = false;
        }
        else
        {
            nextButtonText.gameObject.GetComponent<TMP_Text>().color = nonFaded;
            nextButton.enabled = true;
            nextButton.GetComponent<Image>().enabled = true;
        }
    }

    public void NextPage()
    {
        page++;
        ClearPage();
        LoadPage();
    }

    public void PreviousPage()
    {
        page--;
        ClearPage();
        LoadPage();
    }

    // Create a leaderboard entry item in Unity with the right information.
    private void CreateLeaderboardEntry(LeaderboardEntry entry, Transform container, List<Transform> transforms)
    {
        Transform leaderboardEntry = Instantiate(leaderboardTemplate, container);
        transforms.Add(leaderboardEntry);
        RectTransform leaderboardRectTransform = leaderboardEntry.GetComponent<RectTransform>();
        leaderboardRectTransform.anchoredPosition = new Vector2(0, -leaderboardTemplate.GetComponent<RectTransform>().rect.height * (transforms.Count - 1));
        leaderboardEntry.gameObject.SetActive(true);
        leaderboardEntry.gameObject.name = "LeaderboardEntry";

        TMP_Text positionText = leaderboardEntry.Find("Position").GetComponent<TMP_Text>();
        TMP_Text timeText = leaderboardEntry.Find("Time").GetComponent<TMP_Text>();
        TMP_Text nameText = leaderboardEntry.Find("Name").GetComponent<TMP_Text>();

        // Entering the values
        positionText.text = "# " + (leaderboardEntries.getLeaderboard().IndexOf(entry) + 1);
        timeText.text = entry.getTime();
        nameText.text = entry.getName();

        // Changing color if 1st, 2nd or 3rd
        switch (leaderboardEntries.getLeaderboard().IndexOf(entry) + 1)
        {
            case 1:
                Color gold = new Color(0.9f, 0.72f, 0.04f);
                positionText.color = gold;
                timeText.color = gold;
                nameText.color = gold;
                break;
            case 2:
                Color silver = new Color(0.8157f, 0.8196f, 0.8353f);
                positionText.color = silver;
                timeText.color = silver;
                nameText.color = silver;
                break;
            case 3:
                Color bronze = new Color(0.8f, 0.5f, 0.2f);
                positionText.color = bronze;
                timeText.color = bronze;
                nameText.color = bronze;
                break;
        }
    }
}
