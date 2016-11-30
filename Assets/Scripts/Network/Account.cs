using UnityEngine;
using System.Collections;

public class Account {
    
    public readonly string email;
    public readonly string username;
    public readonly string password;

    public Account(string email, string username, string password){
        this.email = email;
        this.username = username;
        this.password = password;
    }
}
