using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAutoDisable : MonoBehaviour
{
    public void AutoDisable() {
        GetComponent<Animator>().StopPlayback();
        gameObject.SetActive(false);
    }
}
