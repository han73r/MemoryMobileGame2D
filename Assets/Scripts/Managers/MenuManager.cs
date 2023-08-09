using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Managers
{
    /// <summary>
    /// Create UI in main menu screen, theme menu and level menu. Control actions in menu 
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        private void Start()
        {
            ConstructorManager.CreateLevel();
        }
        
    }
}

