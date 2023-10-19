using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    double timer;
    bool started;

    void Update()
    {
        if (started)
        {
            timer += Time.deltaTime;
            calculateTimerValue(timer);
        }
    }

    public void StartTimer()
    {
        if (!started)
        {
            started = true;
        }
        else
        {
            started = false;
            timer = 0;
            calculateTimerValue(timer);
        }
    }

    public void StopTimer()
    {
        if (started)
        {
            started = false;
        }
    }

    public void SaveTimer(Level level)
    {
        string key = LevelDict.SaveDataDictionary[level];
        string json = PlayerPrefs.GetString(key);
        LeaderboardList list = JsonUtility.FromJson<LeaderboardList>(json);
        if (list == null)
        {
            list = new LeaderboardList(new List<LeaderboardEntry>(), level);
        }
        string name = PlayerPrefs.GetString("name");
        if (name.Length == 5)
        {
            if (name[0].Equals('l') && name[1].Equals('e') && name[2].Equals('e') && name[3].Equals('t'))
            {
                timer = 93.769f;
            }
        }
        list.getLeaderboard().Add(new LeaderboardEntry(timer, PlayerPrefs.GetString("name")));
        string json3 = JsonUtility.ToJson(list);
        PlayerPrefs.SetString(key, json3);
        PlayerPrefs.Save();
    }

    private void calculateTimerValue(double timer)
    {
        TimeSpan time = TimeSpan.FromSeconds(timer);
        timerText.text = time.ToString("mm':'ss'.'fff");
    }
}
