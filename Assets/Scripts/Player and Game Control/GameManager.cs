using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;


public class GameManager : MonoBehaviour
{
    public PlayerScript player;
    public GameObject explosion;
    public int lives = 1;
    public float respawnInvulnerabilityTime = 3.0f;
    public float respawnTime = 3.0f;
    public static int players = 0;
    public GameObject playScene;
    public GameObject gameOverScene;
    public Transform cameraPosition;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public PlayfabManager playfabmanager;

    public AudioSource ExplosionAudioSource;
    
    public void AddPlayer(PlayerScript newPlayer)
    {
      players = PhotonNetwork.CurrentRoom.PlayerCount;
      this.player = newPlayer;
    }
    
    public void CovidDestroyed(Covid asteroid, PlayerScript killer, int point) {
        ExplosionAudioSource.Play();
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.SetActive(false);
        this.explosion.SetActive(true);

        if (killer == this.player){
            this.score += point;
            if (this.score < 0)
            {
                this.score = 0;
            }
            scoreText.text = "Score: " + this.score;
            PlayerPrefs.SetInt("GameScore", this.score);
            PlayerPrefs.Save();
        }
       
    }

    public void PlayerDied() {
        this.lives--;
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.SetActive(false);
        this.explosion.SetActive(true);

        if (this.lives == 0 || PlayerScript.bulletNumber == 0) {
            GameOver();
        } else {
            Invoke(nameof(Respawn), respawnTime);
        }
    }
    //this function is unused for now since lives is equal to one
    private void Respawn() {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.currentHealth = PlayerScript.maxHealth;
        this.player.gameObject.SetActive(true);
    
        
        Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
    
    }
    //function to provide invinsibility to ship
    private void TurnOnCollisions() {
        this.player.gameObject.layer = LayerMask.NameToLayer("Ship");
    }

    private void GameOver() {
        int finalScore = this.score;
        PhotonNetwork.LeaveRoom();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 

        playfabmanager.SendLeaderboard(score);
        this.score = 0;
    }
}
