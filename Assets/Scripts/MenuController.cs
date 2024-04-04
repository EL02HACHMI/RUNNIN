using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
[SerializeField] private GameObject UsernameMenu;
[SerializeField] private TMP_InputField UsernameInput;
[SerializeField] private GameObject StartButton;
 [SerializeField] private Slider volumeSlider = null;
 [SerializeField] private TextMeshProUGUI volumeTextUI;
 public GameObject ReglageMenu;

private void Awake(){
         // Pré-remplir le champ de saisie avec le dernier nom d'utilisateur sauvegardé
        string lastUsername = PlayerPrefs.GetString("LastUserName", "");
        UsernameInput.text = lastUsername;
        UsernameInput.onValueChanged.AddListener(delegate { ChangeUserNameInput(); });
        // Vérifier si le nom d'utilisateur est valide pour activer le bouton de démarrage
        ChangeUserNameInput();
}
public void ChangeUserNameInput(){
    if(UsernameInput.text.Length >=3){
        StartButton.SetActive(true);
    }
    else
    {
        StartButton.SetActive(false);
    }
}
public void SetUserName(){
        PlayerPrefs.SetString("LastUserName", UsernameInput.text);
        PlayerPrefs.Save();
        UsernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.text;
        SceneManager.LoadScene("Loading");

}
public void ShowUserNameMenu(){
    UsernameMenu.SetActive(true);
}
public void Start(){
    LoadValues();
    HideSettings();
}
 public void VolumeSlider(float volume){
       float volumePercentage = volume * 100f;
    volumeTextUI.text = volumePercentage.ToString("0");
 }
 public void SaveVolumeButton(){
    float volumeValue = volumeSlider.value;
    PlayerPrefs.SetFloat("VolumeValue", volumeValue);
    LoadValues();
 }
 void LoadValues(){
    float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
    volumeSlider.value = volumeValue;
    AudioListener.volume = volumeValue;
 }
 public void HideSettings(){
   ReglageMenu.SetActive(false);
 }
 public void ShowSettings(){
   ReglageMenu.SetActive(true);
 }
 public void HideUsername(){
    UsernameMenu.SetActive(false);
 }
public void QuitterJeu(){
        Application.Quit();
}
}
