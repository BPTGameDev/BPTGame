using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighestScore : MonoBehaviour
{
    public TextMeshProUGUI textmeshPro;
    public static int highestScore;
    public static int currentHighest;
    // Start is called before the first frame update
    void Start()
    {
        if(highestScore > currentHighest){
            textmeshPro.SetText("Your Highest Score: " + highestScore);
            currentHighest = highestScore;
        } else{
            textmeshPro.SetText("Your Highest Score: " + currentHighest);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(highestScore > currentHighest){
            textmeshPro.SetText("Your Highest Score: " + highestScore);
            currentHighest = highestScore;
        } else{
            textmeshPro.SetText("Your Highest Score: " + currentHighest);
        }
    }
}
