/// <summary>
/// Add Different all methods here 
/// and create Interfaces here for different Dictionaries
/// </summary>
public abstract class LevelDictionary
{
    public abstract string GetData();
}
public class StringLevelDictionary : LevelDictionary
{
    private string data;

    public StringLevelDictionary(string data)
    {
        this.data = data;
    }

    public override string GetData()
    {
        return data;
    }
}
public class EnglishAlphabet : LevelDictionary
{
    public override string GetData()
    {
        return "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}
public class Numerals : LevelDictionary
{
    public override string GetData()
    {
        return "0987654321";
    }
}