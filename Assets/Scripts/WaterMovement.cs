using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    public float speed = 5f;
    public float distanceToReset = 5f;
    float initialY;
    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
        if(transform.position.z <= -distanceToReset)
            transform.position = Vector3.up * initialY;
    }
}
