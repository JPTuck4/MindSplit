/**** 
 * Created by: JP Tucker
 * Date Created: Mar 7, 2022
 * 
 * Last Edited by: JP
 * Last Edited: Mar 7, 2022
 * 
 * Description: Saves Best Time in player prefabs
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestTime : MonoBehaviour
{
    public int level;
    public Timer timer;
    static public float bestTime = -1;
    void Start()
    {
        // If the PlayerPrefs BestTime already exists, read it
        if (PlayerPrefs.HasKey("BestTime" + level))
        {
            bestTime = PlayerPrefs.GetFloat("BestTime" + level);
        }
        else
        {
            bestTime = -1;
        }
    }


    void Update()
    {
        Text gt = this.GetComponent<Text>();
        //if game is won and they got a new high score update the level prefab
        if (timer.gameIsWon && (timer.time < bestTime || bestTime == -1))
        {
            bestTime = timer.time;
            gt.text = "Best: \n" + Mathf.Round(timer.time*10)/10.0;
            PlayerPrefs.SetFloat("BestTime" + level, timer.time);
        }
        //otherwise display old best time
        else if(bestTime == -1)
        {
            gt.text = "Best: \n-";
        }
        else
        {
            gt.text = "Best: \n" + Mathf.Round(bestTime * 10) / 10.0;
        }
    }
}
