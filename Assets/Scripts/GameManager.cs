using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
       spawnPlayer(); 
    }

    void spawnPlayer(){
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position,playerPrefab.transform.rotation);
    }
}
