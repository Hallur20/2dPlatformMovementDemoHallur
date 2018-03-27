using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

    public GameObject Player;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdSpawnMyPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.name == null)
        {
            Debug.Log(Player.name);
            Debug.Log("test");
        }		
	}
    [Command]
    void CmdSpawnMyPlayer()
    {
        GameObject go = Instantiate(Player);
        NetworkServer.SpawnWithClientAuthority(go,connectionToClient);
    }
}
