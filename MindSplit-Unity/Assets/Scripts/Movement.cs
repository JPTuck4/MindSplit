/**** 
 * Created by: JP Tucker
 * Date Created: Mar 7, 2022
 * 
 * Last Edited by: JP
 * Last Edited: Mar 7, 2022
 * 
 * Description: Moves the players
****/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("SET IN INSPECTOR")]
    public float speed = 10f;
    public float delay = .75f;
    public MindScript mind;

    [Header("SET DYNAMICALLY")]
    Rigidbody rb;
    private float[] moveTime = new float[] { -1f, -1f, -1f, -1f };
    public bool[] isBlocked = new bool[] { true, true, true, true };
    public int moveDir;

    readonly int STOP = -1;
    readonly int RIGHT = 0;
    readonly int LEFT = 1;
    readonly int UP = 2;
    readonly int DOWN = 3;
    // Start is called before the first frame update
    void Start()
    {
        moveDir = -1;
        rb = this.GetComponent<Rigidbody>();
    }


    private void Update()
    {
        updateisBlocked(); //make sure isBlocked is accurate
        //if a key is pressed start a timer to start a move after a delay
        if (Input.GetKeyDown(KeyCode.C) && mind.activePlayers[RIGHT] == this.gameObject)
        {
            moveTime[RIGHT] = Time.time + delay;
            if (moveDir == STOP)
                StartCoroutine(Move(RIGHT));
        }
        if (Input.GetKeyDown(KeyCode.M) && mind.activePlayers[DOWN] == this.gameObject)
        {
            moveTime[DOWN] = Time.time + delay;
            if (moveDir == STOP)
                StartCoroutine(Move(DOWN));
        }
        if (Input.GetKeyDown(KeyCode.P) && mind.activePlayers[LEFT] == this.gameObject)
        {
            moveTime[LEFT] = Time.time + delay;
            if (moveDir == STOP)
                StartCoroutine(Move(LEFT));
        }
        if (Input.GetKeyDown(KeyCode.Q) && mind.activePlayers[UP] == this.gameObject)
        {
            moveTime[UP] = Time.time + delay;
            if (moveDir == STOP)
                StartCoroutine(Move(UP));
        }
        //if the key is released early, cancel the move and change players
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (moveTime[RIGHT] > Time.time)
            {
                mind.ChangePlayer(this.gameObject, RIGHT);
            }
            moveTime[RIGHT] = -1f;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            if (moveTime[DOWN] > Time.time)
            {
                mind.ChangePlayer(this.gameObject, DOWN);
            }
            moveTime[DOWN] = -1f;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (moveTime[LEFT] > Time.time)
            {
                mind.ChangePlayer(this.gameObject, LEFT);
            }
            moveTime[LEFT] = -1f;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (moveTime[UP] > Time.time)
            {
                mind.ChangePlayer(this.gameObject, UP);
            }
            moveTime[UP] = -1f;
        }
    }

    private void FixedUpdate()
    {
        //move players according to movedir
        Vector3 pos = rb.position;
        if (moveDir == LEFT)
        {
            pos.x += -speed * Time.deltaTime;
        }
        else if (moveDir == RIGHT)
        {
            pos.x += speed * Time.deltaTime;
        }
        else if (moveDir == UP)
        {
            pos.z += speed * Time.deltaTime;
        }
        else if (moveDir == DOWN)
        {
            pos.z += -speed * Time.deltaTime;
        }
        rb.MovePosition(pos);
    }

    IEnumerator Move(int dir)
    {
        //wait for delay
        yield return new WaitForSeconds(delay);



        // Code to execute after the delay
        //if the object hasnt started moving by another player, start moving the object
        if (moveDir == STOP)
        {
            if (moveTime[LEFT] > 0 && moveTime[LEFT] <= Time.time && !isBlocked[LEFT])
            {
                moveDir = LEFT;
                moveTime[LEFT] = -1f;
            }
            else if (moveTime[RIGHT] > 0 && moveTime[RIGHT] <= Time.time && !isBlocked[RIGHT])
            {
                moveDir = RIGHT;
                moveTime[RIGHT] = -1f;
            }
            else if (moveTime[DOWN] > 0 && moveTime[DOWN] <= Time.time && !isBlocked[DOWN])
            {
                moveDir = DOWN;
                moveTime[DOWN] = -1f;
            }
            else if (moveTime[UP] > 0 && moveTime[UP] <= Time.time && !isBlocked[UP])
            {
                moveDir = UP;
                moveTime[UP] = -1f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = this.transform.position;
        Vector3 relativePosition = this.transform.InverseTransformPoint(collision.collider.ClosestPoint(pos));
        if ((relativePosition.normalized.x > .96f && moveDir == RIGHT)
            || (relativePosition.normalized.x < -.96f && moveDir == LEFT)
            || (relativePosition.normalized.z > .96f && moveDir == UP)
            || (relativePosition.normalized.z < -.96f && moveDir == DOWN))
        {
            if (collision.collider.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<Movement>().moveDir == STOP && !collision.gameObject.GetComponent<Movement>().isBlocked[moveDir])
                {
                    //print(this + " forced " + collision.gameObject + " to move");
                    collision.gameObject.GetComponent<Movement>().moveDir = moveDir;
                    Vector3 cpos = collision.gameObject.transform.position;
                    if (moveDir == LEFT && pos.x > cpos.x)
                    {
                        cpos.x = pos.x - 1.0f;
                        cpos.z = pos.z;
                    }
                    if (moveDir == RIGHT && pos.x < cpos.x)
                    {
                        cpos.x = pos.x + 1.0f;
                        cpos.z = pos.z;
                    }
                    if (moveDir == UP && pos.z < cpos.z)
                    {
                        cpos.x = pos.x;
                        cpos.z = pos.z + 1.0f;
                    }
                    if (moveDir == DOWN && pos.z > cpos.z)
                    {
                        cpos.x = pos.x;
                        cpos.z = pos.z - 1.0f;
                    }
                    collision.gameObject.transform.position = cpos;
                }
                else if (collision.gameObject.GetComponent<Movement>().isBlocked[moveDir])
                {
                    isBlocked[moveDir] = true;
                }
            }
            else if (collision.collider.CompareTag("Wall") || collision.gameObject.GetComponent<Movement>().moveDir == STOP)
            {
                isBlocked[moveDir] = true;
                moveDir = STOP;
                pos.x = Mathf.Round(pos.x);
                pos.z = Mathf.Round(pos.z);
                this.transform.position = pos;
            }
        }
    }

    private List<bool[]> contacts = new List<bool[]>();
    private void OnCollisionStay(Collision collision)
    {
        Vector3 playerPos = this.transform.position;
        Vector3 relativePosition = this.transform.InverseTransformPoint(collision.collider.ClosestPoint(playerPos));
        if ((relativePosition.normalized.x > .96f)
            || (relativePosition.normalized.x < -.96f)
            || (relativePosition.normalized.z > .96f)
            || (relativePosition.normalized.z < -.96f))
        {
            bool[] newIsBlocked = new bool[4] { false, false, false, false };
            if (relativePosition.magnitude <= .51)
            {
                if (collision.collider.CompareTag("Wall"))
                {
                    Vector3 cpos = collision.gameObject.transform.position;
                    if (relativePosition.x < .51 && relativePosition.x > 0)
                    {
                        newIsBlocked[RIGHT] = true;
                        if (moveDir == RIGHT || moveDir == STOP)
                            playerPos.x = collision.collider.ClosestPoint(playerPos).x - .5f;
                    }
                    if (relativePosition.x > -.51 && relativePosition.x < 0)
                    {
                        newIsBlocked[LEFT] = true;
                        if (moveDir == LEFT || moveDir == STOP)
                            playerPos.x = collision.collider.ClosestPoint(playerPos).x + .5f;
                    }
                    if (relativePosition.z < .51 && relativePosition.z > 0)
                    {
                        newIsBlocked[UP] = true;
                        if (moveDir == UP || moveDir == STOP)
                            playerPos.z = collision.collider.ClosestPoint(playerPos).z - .5f;
                    }
                    if (relativePosition.z > -.51 && relativePosition.z < 0)
                    {
                        newIsBlocked[DOWN] = true;
                        if (moveDir == DOWN || moveDir == STOP)
                            playerPos.z = collision.collider.ClosestPoint(playerPos).z + .5f;
                    }
                    if (((relativePosition.normalized.x > .96f && moveDir == RIGHT)
                        || (relativePosition.normalized.x < -.96f && moveDir == LEFT)
                        || (relativePosition.normalized.z > .96f && moveDir == UP)
                        || (relativePosition.normalized.z < -.96f && moveDir == DOWN)) || moveDir == STOP)
                    {
                        this.transform.position = playerPos;
                        moveDir = STOP;
                    }
                }
                if (collision.collider.CompareTag("Player"))
                {
                    Vector3 cpos = collision.body.gameObject.transform.position;
                    if (relativePosition.normalized.x > .96f && relativePosition.x < .51f && relativePosition.x > 0)
                    {
                        if (collision.gameObject.GetComponent<Movement>().isBlocked[RIGHT])
                        {
                            //print(this + "can't go right because " + collision.gameObject + " can't go right");
                            newIsBlocked[RIGHT] = true;
                            if (moveDir == RIGHT || moveDir == STOP)
                                playerPos.x = cpos.x - 1;
                        }
                        else if (collision.gameObject.GetComponent<Movement>().moveDir == STOP && moveDir == RIGHT)
                        {
                            //print(this + "forced " + collision.gameObject + " to move right");
                            collision.gameObject.GetComponent<Movement>().moveDir = moveDir;
                        }
                    }
                    if (relativePosition.normalized.x < -.96f && relativePosition.x > -.51f && relativePosition.x < 0)
                    {
                        if (collision.gameObject.GetComponent<Movement>().isBlocked[LEFT])
                        {
                            newIsBlocked[LEFT] = true;
                            if (moveDir == LEFT || moveDir == STOP)
                                playerPos.x = cpos.x + 1;
                        }
                        else if (collision.gameObject.GetComponent<Movement>().moveDir == STOP && moveDir == LEFT)
                        {
                            collision.gameObject.GetComponent<Movement>().moveDir = moveDir;
                        }
                    }
                    if (relativePosition.normalized.z > .96f && relativePosition.z < .51f && relativePosition.z > 0)
                    {
                        if (collision.gameObject.GetComponent<Movement>().isBlocked[UP])
                        {
                            newIsBlocked[UP] = true;
                            if (moveDir == UP || moveDir == STOP)
                                playerPos.z = cpos.z - 1;
                        }
                        else if (collision.gameObject.GetComponent<Movement>().moveDir == STOP && moveDir == UP)
                        {
                            collision.gameObject.GetComponent<Movement>().moveDir = moveDir;
                        }
                    }
                    if (relativePosition.normalized.z < -.96f && relativePosition.z > -.51f && relativePosition.z < 0)
                    {
                        if (collision.gameObject.GetComponent<Movement>().isBlocked[DOWN])
                        {
                            newIsBlocked[DOWN] = true;
                            if (moveDir == DOWN || moveDir == STOP)
                                playerPos.z = cpos.z + 1;
                        }
                        else if (collision.gameObject.GetComponent<Movement>().moveDir == STOP && moveDir == DOWN)
                        {
                            collision.gameObject.GetComponent<Movement>().moveDir = moveDir;
                        }
                    }
                    if ((moveDir != STOP && collision.gameObject.GetComponent<Movement>().isBlocked[moveDir]
                        && ((relativePosition.normalized.x > .96f && moveDir == RIGHT)
                        || (relativePosition.normalized.x < -.96f && moveDir == LEFT)
                        || (relativePosition.normalized.z > .96f && moveDir == UP)
                        || (relativePosition.normalized.z < -.96f && moveDir == DOWN))) || moveDir == STOP)
                    {
                        this.transform.position = playerPos;
                        moveDir = STOP;
                    }
                }
            }
            contacts.Add(newIsBlocked);
        }
    }

    private void updateisBlocked()
    {
        if (contacts.Count != 0)
        {
            bool[] toSet = new bool[] { false, false, false, false };
            foreach (bool[] contact in contacts)
            {
                for (int i = 0; i < 4; i++)
                {
                    toSet[i] = toSet[i] || contact[i];
                }
            }
            contacts.Clear();
            isBlocked = toSet;
        }
    }
}
