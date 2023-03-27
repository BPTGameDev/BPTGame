using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
using TMPro;

public class LeaderBoardController : MonoBehaviour
{
    public int ID;
    public TextMeshProUGUI userID;
    public TextMeshProUGUI userScore;
    int maxScores = 5;

    private void Start(){
        LootLockerSDKManager.StartSession("virus1", (response)=>
        {
            if(response.success)
            {   
                ShowScores();
                Debug.Log("Success");
            } else
            {
                Debug.Log("Failed");
            }
        });
    }

    public void ShowScores(){
        string playerNames = "";
        string playerScores = "";
        LootLockerSDKManager.GetScoreList(ID, maxScores, (response) => {
            if(response.success)
            {   
                LootLockerLeaderboardMember[] scores = response.items;
                for (int i = 0; i < scores.Length; i++){
                    playerNames += scores[i].member_id;
                    playerNames = playerNames + "\n" + "\n";
                    playerScores += scores[i].score;
                    playerScores = playerScores + "\n" + "\n";
                }
                if(scores.Length<maxScores){
                    for (int i = scores.Length; i < maxScores; i++){
                        playerNames += "-";
                        playerNames = playerNames + "\n" + "\n";
                        playerScores += "-";
                        playerScores = playerScores + "\n" + "\n";
                    }
                }
                userID.text = playerNames;  
                userScore.text = playerScores;
            } else
            {
                Debug.Log("Failed");
            }
        });
        
    }
    public void SubmitScore(){
        LootLockerSDKManager.SubmitScore(PlayerPrefs.GetString("ID"), HighestScore.currentHighest, ID, (response)=>
        {
            if(response.success)
            {   
                ShowScores();
                Debug.Log("Success");
            } else
            {
                Debug.Log("Failed");
            }
        });
    }
}

