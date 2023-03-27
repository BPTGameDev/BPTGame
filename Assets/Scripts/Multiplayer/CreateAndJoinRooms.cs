using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;


    
    // private LoadBalancingClient loadBalancingClient;

    public void CreateRoom(){
        PhotonNetwork.OfflineMode = true;
        var strB = Random.Range(100, 999);
        PhotonNetwork.CreateRoom(strB+"");
        //PhotonNetwork.LoadLevel("Game");
    }

    public void JoinRoom(){
        // join a room using the join room input field text
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    
    public override void OnJoinedRoom(){
        // called upon joining a room, transition to game scene
        PhotonNetwork.LoadLevel("Game");
        Debug.Log("Joined room");
    }

    public void QuickMatch()
    {
        // loadBalancingClient.OpJoinRandomOrCreateRoom(null, null);;
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Joined random lobby!");
    }
}
