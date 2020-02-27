using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInfo
{
    public string login, password, name;

    public UserInfo(LoginSystem loginSystem) {
        login = loginSystem.login;
        password = loginSystem.password;
        name = loginSystem.name;
    }
    public UserInfo(Registration loginSystem)
    {
        login = loginSystem.login;
        password = loginSystem.password;
        name = loginSystem.name;

    }


}