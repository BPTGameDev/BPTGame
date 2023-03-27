using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserID : MonoBehaviour
{
    public TextMeshProUGUI userID;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        userID.text = "ID: " + PlayerPrefs.GetString("ID");
    }
}
