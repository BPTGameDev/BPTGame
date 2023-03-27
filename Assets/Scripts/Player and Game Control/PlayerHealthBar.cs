using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider _health;
    public Image onHit;
    private bool onHitPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        _health.value = PlayerScript.playerHealth;
        onHit.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _health.value = PlayerScript.playerHealth;
    }
    public void TakeDamage() {
        if (onHitPlaying) {
           StopCoroutine(HandleTakeDamage()); 
        }
        onHit.gameObject.SetActive(false);
        onHit.gameObject.SetActive(true);
        onHit.color = new Vector4(onHit.color.r, onHit.color.g, onHit.color.b, 0f);
        StartCoroutine(HandleTakeDamage());
    }
    private IEnumerator HandleTakeDamage()
    {
        onHitPlaying = true;
        float transparency = 0f;
        for(float i = 0; i < 0.4f; i += Time.deltaTime)
        {
            transparency = Mathf.Lerp(transparency, 0.5f, 0.1f);
            onHit.color = new Vector4(onHit.color.r, onHit.color.g, onHit.color.b, transparency);
            yield return new WaitForEndOfFrame();
        }
        for (float i = 0; i < 0.4f; i += Time.deltaTime)
        {
            transparency = Mathf.Lerp(transparency, 0, 0.1f);
            onHit.color = new Vector4(onHit.color.r, onHit.color.g, onHit.color.b, transparency);
            yield return new WaitForEndOfFrame();
        }
        onHit.color = new Vector4(onHit.color.r, onHit.color.g, onHit.color.b, 0);
        onHit.gameObject.SetActive(false);
        onHitPlaying = false;
    }
}
