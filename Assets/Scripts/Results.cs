using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour
{
    public GameObject resultPrefab; // Prefab de l'objet container
    public Transform resultsContainer; // Parent où les résultats seront affichés

    private void Start()
    {
        // Récupérer le nombre de joueurs et afficher les résultats pour chaque joueur
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        float templateHeight = 1.2f;
        // Trier les joueurs par score décroissant
        Player[] sortedPlayers = PhotonNetwork.PlayerList.OrderByDescending(player =>
            PlayerPrefs.GetInt("PlayerScore_" + player.ActorNumber)).ToArray();
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            // Créer une copie de l'objet container
            GameObject resultObject = Instantiate(resultPrefab, resultsContainer);
            RectTransform entryRectTransform = resultObject.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            Player player = sortedPlayers[i];
            //Player player = PhotonNetwork.PlayerList[i];
            string playerName = player.NickName;
            int position = i+1;
            int playerScore = PlayerPrefs.GetInt("PlayerScore_" + player.ActorNumber);

            //int score = PlayerPrefs.GetInt("PlayerScore");

            // Mettre à jour les textes de l'objet container avec les données du joueur
            resultObject.transform.Find("PositionText").GetComponent<TextMeshProUGUI>().text = position.ToString();
            resultObject.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = playerName;
            resultObject.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = playerScore.ToString();
        }
    }
    public void QuitGame(){
        SceneManager.LoadScene("MainMenu");
    }
}