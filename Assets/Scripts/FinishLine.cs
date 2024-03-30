using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FinishLine : MonoBehaviour
{
    public TMP_Text winnerText; // Assign in the Inspector
    public TMP_Text timerText; // Assign in the Inspector
    private bool winnerDeclared = false;
    private float countdown = 20f; // 2 minutes countdown
    private bool startCountdown = false;

    private void Update()
    {
        if (startCountdown)
        {
            countdown -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Round(countdown).ToString();

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
            winnerText.text = other.name + " is the winner!";
            winnerText.transform.parent.gameObject.SetActive(true);

            // Disable the player's movement
            PlayerController playerMovement = other.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                playerMovement.SetMovement(false);
            }

            // Start the countdown for other players
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            // For players finishing after the winner, you might still want to show their rank
            // And possibly disable their movement if the game is ending
        }
    }

    private void EndGame()
    {
        // Logic to end the game if timer runs out
        timerText.gameObject.SetActive(false); // Hide the timer
        Debug.Log("Game Over");
        // Optionally load a scene or show a game over screen
    }
}