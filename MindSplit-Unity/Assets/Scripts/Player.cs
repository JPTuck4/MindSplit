using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("SET IN INSPECTOR")]
    public float speed = 10;
    public float delay = 1;

    [Header("SET DYNAMICALLY")]
    private bool isMoving;
    Rigidbody rb;
    private bool moveIsSet = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        isMoving = false;
    }


    private void Update()
    {
        if (!isMoving && !moveIsSet)
        {
            if (Input.GetKey(KeyCode.C))
            {
                StartCoroutine(Move("right"));
                moveIsSet = true;
            }
            if (Input.GetKey(KeyCode.M))
            {
                StartCoroutine(Move("down"));
                moveIsSet = true;
            }
            if (Input.GetKey(KeyCode.P))
            {
                StartCoroutine(Move("left"));
                moveIsSet = true;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                StartCoroutine(Move("up"));
                moveIsSet = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Q))
        {
            print("cancel move");
            moveIsSet = false;
        }
    }

    IEnumerator Move(string dir)
    {
        yield return new WaitForSeconds(delay);

        // Code to execute after the delay
        if (moveIsSet)
        {
            print("Start Moving");
            isMoving = true;
            if (dir == "right")
                rb.velocity = new Vector3(speed, 0, 0);
            if (dir == "left")
                rb.velocity = new Vector3(-speed, 0, 0);
            else if (dir == "down")
                rb.velocity = new Vector3(0, 0, -speed);
            else if (dir == "up")
                rb.velocity = new Vector3(0, 0, speed);

        }
        moveIsSet = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        isMoving = false;
    }
}
