using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 1f;
    float realInterval = 1f;
    float shootTimer;
    Transform player;
    public bool inCurrentSection = true;
    public float moveSpeed = 10f;
    public float rotDelta = 10f;
    public AudioClip sfxShoot;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        realInterval = shootInterval + Random.Range(-.2f, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(inCurrentSection){
            shootTimer += Time.deltaTime;
            if(shootTimer >= realInterval){
                Quaternion bulletRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
                
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.forward * 1.5f, bulletRotation);
                bullet.transform.GetChild(0).transform.rotation = Quaternion.identity;
                shootTimer = 0;
                realInterval = shootInterval + Random.Range(-.2f, .2f);
                GetComponent<AudioSource>().PlayOneShot(sfxShoot);
            }
        }
        else{
            transform.position += Vector3.up * moveSpeed * 2 * Time.deltaTime + Vector3.back * moveSpeed * Time.deltaTime;
            if(transform.position.y >= 20)
                Destroy(gameObject);
        }

        Quaternion targetRot = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotDelta * Time.deltaTime);
    }

    public void ExitSection(){
        inCurrentSection = false;
    }
}
