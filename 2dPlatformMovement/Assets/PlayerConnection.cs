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
		
	}
    [Command]
    void CmdSpawnMyPlayer()
    {
        GameObject go = Instantiate(Player);
        NetworkServer.SpawnWithClientAuthority(go,connectionToClient);
    }
}
