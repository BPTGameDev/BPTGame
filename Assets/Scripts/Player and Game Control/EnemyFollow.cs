using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 1.0f;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Vector2 displacement = target.position- transform.position;
        if (Mathf.Abs(Vector2.SignedAngle(Vector2.Dot(Vector2.right, displacement)*Vector2.right, displacement)) < 60f) {
            transform.right = Vector2.Dot(Vector2.right, displacement) * Vector2.right;
        }
    }
}
