using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActives : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject LeaderBoard;
    public GameObject LoginButton;

    // Update is called once per frame
    void Update()
    {
        if (PlayfabLogin.loggedIn)
        {
            PlayButton.SetActive(true);
            LeaderBoard.SetActive(true);
            LoginButton.SetActive(false);
        }
    }
}
