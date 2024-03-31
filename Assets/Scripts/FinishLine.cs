using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;

public class FinishLine : MonoBehaviourPun
{
    public TMP_Text winnerText; // Assign in the Inspector
    public TMP_Text timerText; // Assign in the Inspector
    private bool winnerDeclared = false;
    private float countdown = 1f; // 2 minutes countdown
    private bool startCountdown = false;
    private bool raceFinished = false;

    private void Update()
    {
        if (startCountdown && countdown >= 0)
        {
        // Obtenez le composant PhotonView de l'objet joueur
        PhotonView pv = GetComponent<PhotonView>();

        // Vérifiez si le composant PhotonView existe
        if (pv != null)
        {
            // Activer les textes du côté de l'objet joueur
            pv.gameObject.GetComponentInChildren<TMP_Text>().text = "Time Left: " + Mathf.Round(countdown).ToString();
            pv.gameObject.GetComponentInChildren<TMP_Text>().gameObject.SetActive(true);
        }
            countdown -= Time.deltaTime;
            //timerText.text = "Time Left: " + Mathf.Round(countdown).ToString();

            if (countdown <= 0)
            {
                EndGame();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!winnerDeclared && other.gameObject.CompareTag("Player"))
        {
            winnerDeclared = true;
            startCountdown = true;
            // Obtenez le composant PhotonView de l'objet joueur
            PhotonView pv = other.GetComponent<PhotonView>();

                // Obtenez le pseudo du joueur propriétaire du PhotonView
                string playerName = pv.Owner.NickName;
            // pv.gameObject.GetComponentInChildren<TMP_Text>().text = playerName + " is the winner!";
                //  pv.gameObject.GetComponentInChildren<TMP_Text>().gameObject.SetActive(true);
                winnerText.text = playerName + " is the winner!";
                winnerText.gameObject.SetActive(true);
                   
        }
    }

    private void EndGame()
    {
        // Logic to end the game if timer runs out
        Debug.Log("Game Over");
        PhotonNetwork.LoadLevel("ResultsScene");
        // Optionally load a scene or show a game over screen
    }
}