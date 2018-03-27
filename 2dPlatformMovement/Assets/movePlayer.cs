using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class movePlayer : NetworkBehaviour {
    public float speed;             //Floating point variable to store the player's movement speed. 


    private Rigidbody2D rb2d;
    // Use this for initialization
    public int totalHealth = 100;

    [SyncVar(hook = "OnPlayerNameChanged")]
    public string playerName;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Top")) { 
            areWeLanded = true;
        }
        if (coll.gameObject.name.Equals("lava"))
        {
            Debug.Log("game over");
            playerName = "dead";
            CmdDestroyMyUnit();
            Debug.Log(gameObject.GetComponentInChildren<TextMesh>().text);
        }
        if (coll.gameObject.name == "bullet clone(Clone)") //if player touches bullet clone
        {
            Destroy(coll.gameObject); //destroy bullet clone
            Debug.Log("you've been hit.");
            totalHealth = totalHealth - 10; //take 10 damage
            playerName = "health: " + totalHealth; //update name with healthpoints
            if (totalHealth <= 0) // if health is below or zero
            {
                Debug.Log("game over");
                CmdDestroyMyUnit(); //destroy player object
            }
        }
    }

    void OnPlayerNameChanged(string newName) //hook method for playerName
    {
        Debug.Log("changed name to: " + newName);
        playerName = newName;
        gameObject.GetComponentInChildren<TextMesh>().text = newName; //
        playerName = newName;    //ops remember hook on syncvar doesnt auto update local value
        //tip: while you're in hook, syncvar behaviour is disabled.
    }

    void Start () {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        playerName = "hp: " + totalHealth; //start with max health on the syncvar variable (100)
        Debug.Log(netId);
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
        if (Input.GetKeyDown("f") && readyToShoot == true)
        {
            CmdFire();
        }
        Debug.Log(gameObject);
    }
    public Transform bulletSpawn;
    public GameObject clone;
    GameObject cloneOnClients;
    bool readyToShoot = true;

    [Command]
    void CmdFire()
    {
        GameObject go = Instantiate(clone); //object exists on the server
        Vector3 pos = bulletSpawn.transform.GetComponent<Transform>().position;
        pos.x = pos.x + 1; //start on +1 x away from player so player will not get hit by own bullet
        go.transform.position = pos;
        NetworkServer.Spawn(go); //object is spawned on the server
        RpcsendServerBulletToAllServers(go); //send object to all clients so they can do what they want with it
    }
    [ClientRpc]
    void RpcsendServerBulletToAllServers(GameObject go)
    {
        cloneOnClients = go;
        InvokeRepeating("LaunchProjectile", 0.0f, 0.1f); //start after 0 seconds, repeat every 0.1 second.

    }
    void LaunchProjectile()
    {
        readyToShoot = false;
        if (cloneOnClients != null) //if the server object exists
        {
            cloneOnClients.transform.Translate(1, 0, 0); //move bullet clone +1 x 
            Destroy(cloneOnClients, 1.5F); // destroy bullet clone after 1.5 seconds
        }
        else
        {
            readyToShoot = true;
            CancelInvoke(); //else cancel the invoke and set readyToShoot to true.
        }

    }



    [Command]
    void CmdDestroyMyUnit()
    {
        Destroy(this.gameObject);
    }
}
