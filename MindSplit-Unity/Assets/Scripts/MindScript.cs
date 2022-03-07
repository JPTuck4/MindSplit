/**** 
 * Created by: JP Tucker
 * Date Created: Mar 7, 2022
 * 
 * Last Edited by: JP
 * Last Edited: Mar 7, 2022
 * 
 * Description: Alows players to change which cube they are controlling
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindScript : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] players;

    [Header("SET DYNAMICALLY")]
    public GameObject[] activePlayers;
    int[] nextPlayerIndex;

    // Start is called before the first frame update
    void Start()
    {
        //choose first player as active player for all directions
        activePlayers = new GameObject[] { players[0], players[0], players[0], players[0] };
        nextPlayerIndex = new int[] { 1, 1, 1, 1 };
    }

    public void ChangePlayer(GameObject player, int dir)
    {
        //change active player to next player in the list
        if (players.Length > 1)
        {
            activePlayers[dir] = players[nextPlayerIndex[dir]];
            nextPlayerIndex[dir] = (nextPlayerIndex[dir] + 1) % players.Length;
        }
    }
    
}
