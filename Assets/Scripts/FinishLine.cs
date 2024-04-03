using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class FinishLine : MonoBehaviourPun
{
    public TMP_Text winnerText; // Assign in the Inspector
    public TMP_Text timerText; // Assign in the Inspector
    private bool winnerDeclared = false;
    private float countdown = 15f; // 2 minutes countdown
    private bool startCountdown = false;
    private float TimerUp = 0f;
    private int score = 0;
    public GameObject playerCamera;
    private Dictionary<int, float> playerTimes = new Dictionary<int, float>(); // Stocker le temps écoulé pour chaque joueur

    private void Update()
    {
            // Incrémenter le temps écoulé
            TimerUp += Time.deltaTime;
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
        if (other.gameObject.CompareTag("Player"))
        {
            winnerDeclared = true;
            startCountdown = true;
            // Obtenez le composant PhotonView de l'objet joueur
            PhotonView pv = other.GetComponent<PhotonView>();

                // Obtenez le pseudo du joueur propriétaire du PhotonView
                string playerName = pv.Owner.NickName;
                //pv.gameObject.GetComponentInChildren<TMP_Text>().text = playerName + " is the winner!";
                //pv.gameObject.GetComponentInChildren<TMP_Text>().gameObject.SetActive(true);
                winnerText.text = playerName + " is the winner!";
                winnerText.gameObject.SetActive(true);
                // Stocker le temps écoulé pour ce joueur
                playerTimes[pv.ViewID] = TimerUp;
                int playerScore = CalculateScore(TimerUp); // Calculer le score pour ce joueur
                string playerScoreKey = "PlayerScore_" + pv.Owner.ActorNumber;
                PlayerPrefs.SetInt(playerScoreKey, playerScore); // Stocker le score dans PlayerPrefs
                // Désactiver la caméra uniquement pour le joueur gagnant

                    Debug.Log(playerCamera);
                playerCamera.SetActive(false);
                
        }
    }
    private int CalculateScore(float playerTime)
    {
        // Vous pouvez ajuster la logique de calcul du score selon vos besoins
        // Par exemple, attribuer un score plus élevé à un temps plus court
            if (playerTime != 0f)
    {
            score = 1000 / (Mathf.RoundToInt(playerTime)); // Exemple de logique de score
        Debug.Log("Player's Score: " + score);
    }
    return score;
    }
    private void EndGame()
    {
        // Logic to end the game if timer runs out
        Debug.Log("Game Over");
        PhotonNetwork.LoadLevel("ResultsScene");
        // Optionally load a scene or show a game over screen
    }
}