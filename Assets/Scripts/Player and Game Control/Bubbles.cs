using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AnimRestart() {
        StartCoroutine(AnimWaitRestart());
    }
    private IEnumerator AnimWaitRestart() {
        GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(Random.Range(1, 3));
        GetComponent<Animator>().enabled = true;
    }
}
