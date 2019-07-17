using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float xDiv = 2f, yDiv;
    public Vector3 offset;
    float shakeIntensity, shakeDuration;
    float shakeTimer;
    bool inShake;
    Vector3 shakeVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inShake)
            shakeVector = Vector3.right * Random.Range(-shakeIntensity, shakeIntensity) + Vector3.up * Random.Range(-shakeIntensity, shakeIntensity);

        if(inShake){
            shakeTimer += Time.deltaTime;
            if(shakeTimer >= shakeDuration){
                shakeIntensity = 0;
                shakeDuration = 0;
                inShake = false;
                shakeTimer = 0;
                shakeVector = Vector3.zero;

            }
        }

        transform.position = Vector3.right * target.position.x / xDiv + Vector3.up * target.position.y / yDiv + offset + shakeVector;
    }

    public void Shake(float intensity, float duration){
        inShake = true;
        shakeIntensity = intensity;
        shakeDuration = duration;
    }
}
