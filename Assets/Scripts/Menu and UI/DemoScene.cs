using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DemoScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -6 );
    }

    public void GoToLobby() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -4 );
    }

    public void GoToGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -3);
    }
}
