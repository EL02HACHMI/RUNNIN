using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;
public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField JoinInput;

    public void CreateRoom(){
        PhotonNetwork.CreateRoom(createInput.text, new RoomOptions {MaxPlayers = 4}, null);
    }
    public void JoinRoom(){
        PhotonNetwork.JoinRoom(JoinInput.text, null);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameplayScene");
    }
}
