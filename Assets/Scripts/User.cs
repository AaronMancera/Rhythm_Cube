using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string username;
    public string email;
    public ArrayList score;
    public User()
    {

    }
    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
        score = new ArrayList();

    }
}
