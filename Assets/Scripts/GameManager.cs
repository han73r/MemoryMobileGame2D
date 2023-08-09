using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DataBase;
using Managers;

/// <summary>
/// Game starts here.
/// Control Scenes
/// Other Managers (only them), do not control data directly!
/// </summary>
public class GameManager : MonoBehaviour
{
    // TASK // DDOL and Singleton
    private void Start()
    {
        ConstructorManager.CreateLevel();
    }

}
