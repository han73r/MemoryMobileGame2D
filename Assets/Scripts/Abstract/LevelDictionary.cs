using System.Collections.Generic;
/// <summary>
/// Add Different all methods here 
/// and create Interfaces here for different Dictionaries
/// </summary>
public abstract class LevelDictionary: ILevelDictionary
{
    protected string dictionary;            // TAST // Add behavior dict not string
    public LevelDictionary(string initialData)
    {
        dictionary = initialData;
    }
    public string GetData() 
    { 
        return dictionary;
    }
    public virtual void SetData(string newData)
    {
        dictionary = newData;
    }

}
public class SimpleFigures : LevelDictionary
{
    public SimpleFigures() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
    }
}
public class TagGame : LevelDictionary
{
    public TagGame() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ")   // what?
    {
    }
}
public class Symbols : LevelDictionary
{
    public Symbols() : base("♥$%&@#!*🌟⚽⚡🌺🍀🌈🎈")   // add
    {
    }
}
public class ArithmeticOperations : LevelDictionary
{
    public ArithmeticOperations() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
    }
}
public class RomanNumerals : LevelDictionary
{
    public RomanNumerals() : base("")
    {
        string romanNumeralsDictionary = "I,II,III,IV,V,VI,VII,VIII,IX,X";
        SetData(romanNumeralsDictionary);
    }
    
    public override void SetData(string newData)
    {
        dictionary = newData;
    }
}

public class EnglishAlphabet : LevelDictionary
{
    public EnglishAlphabet() : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
    }
}
public class Numerals : LevelDictionary
{
    public Numerals() : base("0987654321")
    {
    }
}

//public class StringLevelDictionary : LevelDictionary
//{
//    private string data;

//    public StringLevelDictionary(string data)
//    {
//        this.data = data;
//    }

//    public override string GetData()
//    {
//        return data;
//    }
//}

//public override string GetData()
//{
//    return "0987654321";
//}
//public override string GetData()
//{
//    return "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
//}