using UnityEngine;
using Photon.Pun;

public class FinishLine : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            // Suppose que chaque joueur a un PhotonView attaché.
            string winnerName = other.GetComponent<PhotonView>().Owner.NickName;
            photonView.RPC("NotifyVictory", RpcTarget.All, winnerName);
        }
    }

    [PunRPC]
    void NotifyVictory(string winnerName)
    {
        Debug.Log(winnerName + " has won the race!");
        // Optionnel : Afficher un écran de victoire ou une notification.
        
        // Le MasterClient charge la scène de lobby pour tout le monde après un court délai.
        if (PhotonNetwork.IsMasterClient)
        {
            // Assurez-vous que cette option est activée quelque part dans votre gestionnaire de réseau ou au démarrage du jeu.
            PhotonNetwork.AutomaticallySyncScene = true; 
            Invoke("LoadLobbyScene", 5f); // Délai de 5 secondes avant de charger la scène de lobby.
        }
    }

    void LoadLobbyScene()
    {
        PhotonNetwork.LoadLevel("Lobby"); // Remplacez par le nom réel de votre scène de lobby.
    }
}
