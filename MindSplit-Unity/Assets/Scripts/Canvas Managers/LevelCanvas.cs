/**** 
 * Created by: JP Tucker
 * Date Created: Feb 23, 2022
 * 
 * Last Edited by: NA
 * Last Edited: Feb 23, 2022
 * 
 * Description: Updates start canvas referecing game manager
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //libraries for UI components

public class LevelCanvas : MonoBehaviour
{
    /*** VARIABLES ***/

    GameManager gm; //reference to game manager


    private void Start()
    {
        gm = GameManager.GM; //find the game manager
    }

    public void next()
    {
        print("next");
        gm.NextLevel();
    }

    public void reset()
    {
        gm.ResetLevel();
    }
}
