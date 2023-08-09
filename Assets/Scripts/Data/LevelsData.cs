using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Keep static and dynamic info about levels
    /// </summary>
    public class LevelsData
    {
        // TASK // CREATE FROM CSV? or JSON file?
        class Level
        {
            int Theme;          // What database use?
            int Type;           // Type Enum Condition
            int Number;         // how many cards or field size (rows and coloumns)
        }
    }
}


