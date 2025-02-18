

public class Extrathings{
    public string getEmail(){
    string path = "wwwroot/Template/email.html";
    return  File.ReadAllText(path);
}
}