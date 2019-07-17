using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;
    public float inDamageDuration = .05f;
    float inDamageTimer;
    public List<Renderer> renderers;
    public AudioClip sfxGetHurt;
    public GameObject explosionFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PlayerBullet")){
            Destroy(other.gameObject);
            TakeDamage(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        inDamageTimer += Time.deltaTime;
        if(inDamageTimer <= inDamageDuration){
            // send message to shader to set to white
            foreach(Renderer renderer in renderers){
                foreach(Material material in renderer.materials){
                    material.SetFloat("_InDamage", 1);
                }
            }

        }
        else{
            foreach(Renderer renderer in renderers){
                foreach(Material material in renderer.materials){
                    material.SetFloat("_InDamage", 0);
                }
            }
        }
    }

    public void TakeDamage(int amount){
        GetComponent<AudioSource>().PlayOneShot(sfxGetHurt);
        health -= amount;
        inDamageTimer = 0;
        if(health <= 0){
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
