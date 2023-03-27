using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviour
{

    [SerializeField] GameObject Player;
    public Slider slider;
    PhotonView view;
    public Gradient gradient;
    public Image fill;

    void update(){
        //transform.rotation = Quaternion.identity;
    }

    private void Start() {
         view = GetComponent<PhotonView>();
    }

    public void SetMaxHealth (int health) {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void show(){

    }
    
    public void SetHealth (int health) {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
