using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab.MultiplayerModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameManager GameRef;

    // Update is called once per frame
    void Update()
    {

    }

    public void Pause()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        PauseMenu.SetActive(true);
    }

    public void Continue()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void EndGame()
    {
        //Allows game to play if restarted
        Time.timeScale = 1;

        //IF WE WANT TO RETURN TO MENU: 
        //SceneManager.LoadScene("Menu");
        //PhotonNetwork.LeaveRoom();

        //IF WE WANT TO END GAME: 
        GameRef.GameOver();
    }
}