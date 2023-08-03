using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float maxLifetime = 2.0f;
    public float speed = 500.0f;
    PhotonView view;
    public PlayerScript player;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void addPlayer(PlayerScript p)
    {
        player = p;
    }

    private void Awake()
    {

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    //Delete Bullets When Invisible (off screen) 8/3/2023
    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public void Project(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);

        //Seems like unnecessary to destroy based on timer if it is deleted when off screen
        //Destroy(this.gameObject, this.maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
