

public class Extrathings
{
    public string getEmail()
    {
        string path = "wwwroot/Template/email.html";
        return File.ReadAllText(path);
    }
}

public class Extrathingsforadduser
{
    public string getEmail()
    {
        string path = "wwwroot/Template/emailadduser.html";
        return File.ReadAllText(path);
    }
}