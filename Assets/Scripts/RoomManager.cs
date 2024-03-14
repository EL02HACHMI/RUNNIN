using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI; // Ajoutez cette ligne pour utiliser le namespace UI

public class RoomManager : MonoBehaviourPunCallbacks // Changez MonoBehaviour à MonoBehaviourPunCallbacks pour accéder aux callbacks de Photon
{
    public GameObject playerPrefab;
    public Button startGameButton; // Référence au bouton "Commencer"

    void Start()
    {
        // Assurez-vous que seul le hôte voit le bouton au début
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.interactable = true; // Le bouton est désactivé jusqu'à ce que les conditions soient remplies
            startGameButton.onClick.AddListener(StartGame); // Ajoute un écouteur d'événements au bouton
        }
        else
        {
            startGameButton.gameObject.SetActive(false); // Les joueurs non hôtes ne voient pas le bouton
        }

         spawnPlayer(); 
    }

    void StartGame()
    {
        // Charge la scène de jeu pour tous les joueurs dans la salle
    if (PhotonNetwork.IsMasterClient)
    {
        PhotonNetwork.LoadLevel("TimingGame");
    }
    }
void Awake()
{
    PhotonNetwork.AutomaticallySyncScene = true;
}
    void spawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation);
    }
}