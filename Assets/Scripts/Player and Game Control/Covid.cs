using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Covid : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer _spriteRenderer;
    public float startTime;
    public float currTime;
    public float interval = 2.0f;
    public float speed = 50.0f;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxLifetime = 30.0f;
    public float maxSize = 1.5f;
    public float halfMultiplier = 0.5f;
    public int variant;
    // variant = 0 regular covid
    // variant = 1 tracker covid
    // variant = 2 speeder covid
    public float pointMultiplier;
    private Rigidbody2D _rigidbody;

    PhotonView view;

    //public TextMeshProUGUI scoreText;
    public int scoreValue;

    public Transform player;
    public bool stayHorizontal = false;

    private void Awake()
    {
        //splat.Play();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();

        startTime = Time.time;
        if (!stayHorizontal) {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        }
        this.transform.localScale = Vector3.one * this.size;

        _rigidbody.mass = this.size;
    }

    void Update()
    {
        currTime = Time.time;
        if (currTime - startTime > interval)
        {
            startTime = currTime;
            speed += 2.0f;
        }

        // if (Vector3.Distance(transform.position, player.position) > 1f)
        // {
        //     RotateTowardsTarget();
        // }
    }
    void OnBecameInvisible()
    {
        // Tricky here, scene view also counts as a camera. But function itself works fine with scene view closed.
        if (Time.time - startTime > maxLifetime) {
            Destroy(gameObject);
        }
    }

    public void getPlayer(Transform target)
    {
        player = target;
    }

    private void RotateTowardsTarget()
    {
        var offset = 90f;
        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
    
    public void SetVariant(int n)
    {
        variant = n;
        switch (variant)
        {
            case 0: // normal
                pointMultiplier = 1.0f;
                this.GetComponent<SpriteRenderer>().material.color = Color.white;
                break;

            case 1: // tracker
                gameObject.AddComponent<EnemyFollow>();
                pointMultiplier = 1.6f;
                this.GetComponent<SpriteRenderer>().material.color = Color.red;
                break;

            case 2: // speeder
                speed *= 2;
                pointMultiplier = 2.0f;
                this.GetComponent<SpriteRenderer>().material.color = Color.blue;
                break;
        }
    }

    public void setTrajectory(Vector2 direction)
    {
        direction = direction.normalized;
        if (variant != 1)
        {
            _rigidbody.AddForce(direction * this.speed);
        }

        if (stayHorizontal) {
            transform.right = Vector2.Dot(direction, Vector2.right) * Vector2.right;
        }

        //Destroy(this.gameObject, this.maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Vax") //the bullet tag is Vax
        {
            if (variant == 0 && this.size * halfMultiplier >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
            int pointEarned = 0;
            if (this.size < 0.8f)
            {
                pointEarned = 100;
                PlayerScript.bulletNumber+=5;
            }
            else if (this.size < 1.2f)
            {
                pointEarned = 50;
                PlayerScript.bulletNumber+=3;
            }
            else if (this.size >= 1.2f)
            {
                pointEarned = 25;
                PlayerScript.bulletNumber+=2;
            }
            
            FindObjectOfType<GameManager>().CovidDestroyed(this, bullet.player, (int)(pointEarned * pointMultiplier));
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            FindObjectOfType<GameManager>().CovidDestroyed(this, player, -75);
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;
        //need photon instantiate here?
        Covid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * halfMultiplier;
        half.setTrajectory(Random.insideUnitCircle.normalized);
    }
}

