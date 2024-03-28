using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; // Ajoutez cette ligne pour utiliser le namespace UI

public class GameManager : MonoBehaviourPunCallbacks // Changez MonoBehaviour à MonoBehaviourPunCallbacks pour accéder aux callbacks de Photon
{
    public GameObject playerPrefab;
    void Start(){
            PhotonNetwork.AutomaticallySyncScene = true;

         spawnPlayer(); 
    }

    void spawnPlayer()
    {
       if (PhotonNetwork.IsConnectedAndReady)
       {
            PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation);
        }
    }

    

}