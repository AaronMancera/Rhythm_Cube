using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string username;
    public string email;
    //para la leaderboard
    public int score_1=0, score_2=0, score_3=0;

    public User()
    {

    }
    public User(string username, string email)
    {
        this.username = username;
        this.email = email;

    }
    public User(string email, int score_1, int score_2,int score_3, string username)
    {
        this.email = email;
        this.score_1 = score_1;
        this.score_2 = score_2;
        this.score_3 = score_3;
        this.username = username;

    }
    public string getUsername() {
        return this.username;
    }
    public string toStringLeaderBoard()
    {
        return username + " - " + score_1+" - " + score_2+" - " + score_3;
    }
}
