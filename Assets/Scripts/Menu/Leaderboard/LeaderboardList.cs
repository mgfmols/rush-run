using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardList
{
    [SerializeField] private List<LeaderboardEntry> leaderboard;
    [SerializeField] private Level level;

    public LeaderboardList(List<LeaderboardEntry> leaderboard, Level level)
    {
        this.leaderboard = leaderboard;
        this.level = level;
    }

    public List<LeaderboardEntry> getLeaderboard()
    {
        return leaderboard;
    }

    public Level getLevel()
    {
        return level;
    }
}
