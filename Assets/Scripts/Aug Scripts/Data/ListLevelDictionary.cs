using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListLevelDictionary : ILevelDictionary
{
    private List<string> data;

    public ListLevelDictionary(List<string> data)
    {
        this.data = data;
    }

    public string GetData()
    {
        // Implement logic to process list data and return a string representation
        return string.Join("", data);
    }
}