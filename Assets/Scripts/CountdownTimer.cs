using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text countdownText;
    public float countdownDuration = 3f;

    IEnumerator Start()
    {
        float timeLeft = countdownDuration;

        while (timeLeft > 0)
        {
            countdownText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        countdownText.text = "Commencez !";
        yield return new WaitForSeconds(1f); // Attendez une seconde avant de commencer

if (SceneManager.GetActiveScene().name != "GameplayScene")
    {
        SceneManager.LoadScene("GameplayScene");
    }
    }
}
