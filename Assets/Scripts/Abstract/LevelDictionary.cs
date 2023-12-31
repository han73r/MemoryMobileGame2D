﻿using System.Collections.Generic;
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
    public Symbols() : base("♥$¥€£%&@#!?§®©*")   // add 🌟⚽⚡🌺🍀🌈🎈 - not working
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