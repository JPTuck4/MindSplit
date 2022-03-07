/**** 
 * Created by: JP Tucker
 * Date Created: Mar 7, 2022
 * 
 * Last Edited by: JP
 * Last Edited: Mar 7, 2022
 * 
 * Description: checks if a player reaches its goal
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject player; //player assigned to the goal

    public bool isWon;

    // Start is called before the first frame update
    void Start()
    {
        isWon = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            isWon = true; //player has achieved the goal
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            isWon = false; //player stepped out
        }

    }
}
