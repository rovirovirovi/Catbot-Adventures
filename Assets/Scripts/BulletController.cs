using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    float timer;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if(transform.position.z > 50 || transform.position.z < -50 || timer >= lifetime)
            Destroy(gameObject);
    }
}
