using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class movePlayer : NetworkBehaviour {
    public float speed;             //Floating point variable to store the player's movement speed.


    private Rigidbody2D rb2d;
    // Use this for initialization

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Top")) { 
            areWeLanded = true;
        }
    }

    void Start () {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
    }

    private bool areWeLanded = false;
    // Update is called once per frame
    void Update () {
        if (!hasAuthority) {
            return;
        } else
        {
           Vector3 newPosition = this.transform.GetComponent<Transform>().position;

            Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
        }
        
        if (Input.GetKeyDown("space") && areWeLanded == true)
        {
            rb2d.AddForce(new Vector2(0, 7), ForceMode2D.Impulse);
            areWeLanded = false;
        }
        if (Input.GetKey("d"))
        {
            rb2d.AddForce(new Vector2(5*speed, 0));
        }
        if (Input.GetKey("a"))
        {
            rb2d.AddForce(new Vector2(-5*speed, 0));
        }
    }
}
