using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string username;
    public string email;
    //para la leaderboard
    public int score_1;

    public User()
    {

    }
    public User(string username, string email)
    {
        this.username = username;
        this.email = email;

    }
    public User(string email, int score_1, string username)
    {
        this.email = email;
        this.score_1 = score_1;
        this.username = username;

    }

    public string toStringLeaderBoard()
    {
        return username + " - " + score_1;
    }
}
