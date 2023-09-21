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

public class AlphabetDictionary : LevelDictionary
{
    public override string GetData()
    {
        return "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}

public class NumbersDictionary : LevelDictionary
{
    public override string GetData()
    {
        return "0987654321";
    }
}