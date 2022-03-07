/**** 
 * Created by: JP Tucker
 * Date Created: Mar 7, 2022
 * 
 * Last Edited by: JP
 * Last Edited: Mar 7, 2022
 * 
 * Description: Updates the timer and checks for level completion
****/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] goals;

    GameManager gm;
    public bool gameIsWon = false;
    public float time = 0f;
    private bool doOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GM;
        this.GetComponent<Text>().text = "" + Mathf.Round(time * 10) / 10;
    }

    void Update()
    {
        bool won = true;
        foreach (GameObject goal in goals) //set gameiswon only if all goals have ben met
        {
            won = goal.GetComponent<Goal>().isWon && won;
        }
        gameIsWon = won;
        if (!gameIsWon)//level is not won, increase timer
        {
            time += Time.deltaTime;
            Text gt = this.GetComponent<Text>();
            int round = (int)Mathf.Round(time * 10);
            if (round % 10 == 0)
            {
                gt.text = round * 1.0 / 10 + ".0";
            }
            else
            {
                gt.text = "" + round * 1.0 / 10;
            }
        }
        else if(doOnce) //game is won, wait 1 second and go to next level
        {
            StartCoroutine(ExampleCoroutine());
            doOnce = false;
        }
    }

    IEnumerator ExampleCoroutine()
    {
        //yield on a new YieldInstruction that waits for 1 second.
        yield return new WaitForSeconds(1);

        //After we have waited 1 seconds print the time again.
        gm.NextLevel();
    }
}
