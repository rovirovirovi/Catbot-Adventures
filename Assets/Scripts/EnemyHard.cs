using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHard : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 1f;
    float realInterval = 1f;
    float shootTimer;
    Transform player;
    public bool inCurrentSection = true;
    public float moveSpeed = 10f;
    public int shotsInBurst = 4;
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
                for(int i = 0; i < shotsInBurst; i++){
                    int index = i - shotsInBurst / 2;
                    
                    GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.forward * 1.5f, transform.rotation * Quaternion.Euler(0, 180 + ((index != 0) ? 45 / index : 0), 0));
                    bullet.transform.GetChild(0).transform.rotation = Quaternion.identity;

                    realInterval = shootInterval + Random.Range(-.2f, .2f);
                    shootTimer = 0;
                }
            }
            
        }
        else{
            transform.position += Vector3.up * moveSpeed * 2 * Time.deltaTime + Vector3.back * moveSpeed * Time.deltaTime;
            if(transform.position.y >= 20)
                Destroy(gameObject);
        }
    }

    public void ExitSection(){
        inCurrentSection = false;
    }
}
