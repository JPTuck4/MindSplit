/**** 
 * Created by: JP Tucker
 * Date Created: Mar 7, 2022
 * 
 * Last Edited by: JP
 * Last Edited: Mar 7, 2022
 * 
 * Description: Makes the player light directions follow the active players
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public int dir;
    public MindScript mind;
    public Movement move;

    readonly int RIGHT = 0;
    readonly int LEFT = 1;
    readonly int UP = 2;
    readonly int DOWN = 3;

    // Update is called once per frame
    void Update()
    {
        GameObject player = mind.activePlayers[dir];
        Vector3 pos = player.transform.position;
        //adjust position based on which direction the player is
        if(dir == RIGHT)
        {
            pos.x = player.transform.position.x + 1.0f; 
            pos.z = player.transform.position.z;
        }
        if (dir == LEFT)
        {
            pos.x = player.transform.position.x - 1.0f;
            pos.z = player.transform.position.z;
        }
        if (dir == UP)
        {
            pos.x = player.transform.position.x;
            pos.z = player.transform.position.z + 1.0f;
        }
        if (dir == DOWN)
        {
            pos.x = player.transform.position.x;
            pos.z = player.transform.position.z - 1.0f;
        }
        pos.y = this.transform.position.y;

        //if the cube is moving turn off the light to visualize noone else can move that character
        if (player.GetComponent<Rigidbody>().GetPointVelocity(Vector3.zero).magnitude <= .01f)
        {
            this.GetComponent<Light>().enabled = true;
        }
        else
        {
            this.GetComponent<Light>().enabled = false;
        }

        this.transform.position = pos;
    }
}
