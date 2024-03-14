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
    public Button startGameButton; // Référence au bouton "Commencer"

    void Start()
    {
        UnityEngine.Debug.Log("Le game Commencer ");

        // Assurez-vous que seul le hôte voit le bouton au début
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.gameObject.SetActive(true);
            startGameButton.interactable = true; // Le bouton est désactivé jusqu'à ce que les conditions soient remplies
            // startGameButton.onClick.AddListener(StartGame); // Ajoute un écouteur d'événements au bouton
        }
        else
        {
            startGameButton.gameObject.SetActive(false); // Les joueurs non hôtes ne voient pas le bouton
        }

        // Pour le moment, le joueur est instancié dès que la scène démarre
        // Vous voudrez peut-être déplacer cette logique pour qu'elle soit exécutée après que le hôte appuie sur "Commencer"
         spawnPlayer(); 
    }

    void spawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation);
    }


    public void StartGame()
    {
        // Votre logique pour commencer le jeu, comme charger une nouvelle scène.
        UnityEngine.Debug.Log("Le bouton Commencer a été cliqué.");

        // Charge la scène de jeu pour tous les joueurs dans la salle
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameplayScene");
        }
    }


    void CheckPlayersReady()
    {
        // Ici, vous pouvez vérifier le nombre de joueurs pour activer le bouton
        // Remplacez 2 par le nombre minimum de joueurs requis pour démarrer le jeu
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 1) 
        {
            startGameButton.interactable = true;
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // Cette méthode est appelée chaque fois qu'un joueur rejoint la salle
        CheckPlayersReady();
    }

    
    

}