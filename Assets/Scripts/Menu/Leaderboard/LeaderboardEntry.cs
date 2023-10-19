using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardEntry : IComparable
{
    [SerializeField] private double time;
    [SerializeField] private string name;

    public LeaderboardEntry(double time, string name)
    {
        this.time = time;
        this.name = name;
    }

    public string getTime()
    {
        // Return time string in format. mm = minutes, ss = seconds, fff = milliseconds
        TimeSpan time = TimeSpan.FromSeconds(this.time);
        return time.ToString("mm':'ss'.'fff");
    }
    
    public string getName()
    {
        return name;
    }

    public int CompareTo(object obj)
    {
        // Check if object we are comparing to is a LeaderboardEntry, if not: return 1
        if (obj.GetType() == typeof(LeaderboardEntry))
        {
            // Compare the time of the object we are comparing to to our time.
            LeaderboardEntry entry = obj as LeaderboardEntry;
            return this.time.CompareTo(entry.time);
        }
        else
        {
            return 1;
        }
    }
}
