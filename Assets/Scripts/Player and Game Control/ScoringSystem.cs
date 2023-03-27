using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoringSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static int scoreValue; 
    public AudioSource collectSound;

    void OnTriggerEnter(Collider other)
    {
        collectSound.Play();
        Debug.Log(this.gameObject);
        if (other.gameObject.transform.tag == "Vax")
        {
            scoreValue += 50;
        } else if (other.gameObject.transform.tag == "Player")
        {
            scoreValue -= 50;
        }
        scoreText.GetComponent<Text>().text = "Score: " + scoreValue;
    }
}
