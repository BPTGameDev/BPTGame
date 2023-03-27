using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public static int score;
    //public Text text;
    public void Setup (int score) {
        gameObject.SetActive(true);
        //score = PlayerPrefs.GetInt("score");
        //pointsText.SetText(score.ToString());
        //pointsText.ForceMeshUpdate(true);
        //pointsText.text = score.ToString() + " POINTS";
    }

    void Update() {
        score = PlayerPrefs.GetInt("GameScore");
        HighestScore.highestScore = score;
        pointsText.SetText("Points: " + score.ToString());
        //pointsText.SetText("Points: " + PlayerPrefs.GetInt("GameScore").ToString());
        pointsText.ForceMeshUpdate(true);
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4); 
        }
    }

    public void PlayAgain() {
        PlayerScript.playerHealth = 10;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); 
    }

    public void MainMenu() {
        PlayerScript.playerHealth = 10;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }

    
}
