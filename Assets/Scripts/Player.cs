using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health = 20;
    public float movementSpeed = 5f;
    Vector3 movementVector;
    Rigidbody rb;
    Vector3 velocity;
    public float drag = .98f;
    public float accleration = 50f;
    public bool canMove = true;

    public float shootInterval = .4f;
    float shootTimer;
    public GameObject bulletPrefab;
    public GameObject bulletSpawnFX;
    public float bulletSpawnFXDuration = .05f;
    public AudioClip sfxGetHurt, sfxShoot;
    public float damageDuration = .1f;
    float damageTimer;

    public List<Renderer> renderers;

    public Animator animator;
    public Image transitionDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transitionDeath = GameObject.Find("ScreenTransitionDeath").GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("EnemyBullet")){
            Destroy(other.gameObject);
            GetComponent<AudioSource>().PlayOneShot(sfxShoot);
            TakeDamage(1);
        }
    }


    void Update()
    {
        if(canMove){
            movementVector.x = Input.GetAxis("Horizontal");
            movementVector.y = Input.GetAxis("Vertical");
        }
        else{
            movementVector.x = 0;
            movementVector.y = 0;
        }
        movementVector.Normalize();
        movementVector.z = 0;

        animator.SetFloat("xInput", movementVector.x);
        animator.SetFloat("yInput", movementVector.y);

        velocity.x += movementVector.x * accleration * Time.deltaTime;
        velocity.y += movementVector.y * accleration * Time.deltaTime;

        Vector3.ClampMagnitude(velocity, movementSpeed);

        rb.MovePosition(rb.position + Vector3.right * velocity.x * Time.deltaTime + Vector3.up * velocity.y * Time.deltaTime);
    
        velocity += -drag * velocity * Time.deltaTime;

        shootTimer += Time.deltaTime;
        if(Input.GetButton("Shoot") && shootTimer >= shootInterval){
            shootTimer = 0;
            Instantiate(bulletPrefab, transform.position + Vector3.forward * 1.5f, Quaternion.identity);
            GetComponent<AudioSource>().PlayOneShot(sfxShoot);
        }
        if(shootTimer <= bulletSpawnFXDuration)
            bulletSpawnFX.SetActive(true);
        else
            bulletSpawnFX.SetActive(false);

        damageTimer += Time.deltaTime;

        if(damageTimer < damageDuration)
            foreach(Renderer r in renderers){
                foreach (Material material in r.materials)
                {
                    material.SetFloat("_InDamage", 1);
                }
            }
        else
            foreach(Renderer r in renderers){
                foreach (Material material in r.materials)
                {
                    material.SetFloat("_InDamage", 0);
                }
            }
        
        if(health <= 0){
            transitionDeath.color = Color.Lerp(transitionDeath.color, new Color(1,1,1,1), Time.deltaTime);
            if(transitionDeath.color.a >= .95f){
                SceneManager.LoadScene("Menu");
            }
        }
    }

    public void TakeDamage(int amount){
        if(damageTimer > damageDuration){
            health -= amount;
            damageTimer = 0;
            Camera.main.GetComponent<CameraController>().Shake(-.2f, .1f);
        }
    }
}
