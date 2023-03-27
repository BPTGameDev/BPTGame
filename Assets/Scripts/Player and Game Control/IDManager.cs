using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDManager : MonoBehaviour
{
    public static string userID;
    // Start is called before the first frame update
    void Start()
    {
        string str = PlayerPrefs.GetString("ID");
        if(string.IsNullOrEmpty(str) == true){
            userID = generateRandomID();
            PlayerPrefs.SetString("ID", userID);
            Debug.Log(userID);
        } else{
            Debug.Log(str);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string generateRandomID(){
        string randomNum = "";
        string randomString = "";
        string[] characters = new string[] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", 
        "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
        randomNum = Random.Range(1, 1000).ToString();
        for (int i = 0; i<=5; i++){
            randomString += characters[Random.Range(0, characters.Length)];
        }
        return randomString + randomNum;
    }
}
