using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveUserData
{
    public static void SaveUser(LoginSystem loginSystem) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/user.radiant";
        FileStream stream = new FileStream(path, FileMode.Create);

        UserInfo data = new UserInfo(loginSystem);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveUser(Registration loginSystem)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/user.radiant";
        FileStream stream = new FileStream(path, FileMode.Create);

        UserInfo data = new UserInfo(loginSystem);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static UserInfo LoadUserInfo() {
        string path = Application.persistentDataPath + "/user.radiant";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            UserInfo data =  formatter.Deserialize(stream) as UserInfo;
            stream.Close();
            return data;
        }
        else {
            Debug.LogError("NoSuchFile");
            return null;
        }
    }
    public static void Delete() {
        string path = Application.persistentDataPath + "/user.radiant";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        
    }
}
