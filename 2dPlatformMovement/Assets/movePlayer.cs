using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class movePlayer : NetworkBehaviour {
    public float speed;             //Floating point variable to store the player's movement speed.

    private Rigidbody2D rb2d;
    // Use this for initialization
    void Start () {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
    }

    private float canJump = 0f;
    // Update is called once per frame
    void Update () {
        if (!hasAuthority) {
            return;
        }
        
        if (Input.GetKeyDown("space") && Time.time > canJump)
        {
            rb2d.AddForce(new Vector2(0, 7), ForceMode2D.Impulse);
            canJump = Time.time + 1.0f;
        }
        if (Input.GetKey("d"))
        {
            rb2d.AddForce(new Vector2(5, 0));
        }
        if (Input.GetKey("a"))
        {
            rb2d.AddForce(new Vector2(-5, 0));
        }
    }
}
