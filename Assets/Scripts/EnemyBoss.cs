using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyBoss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 1f;
    float realInterval = 1f;
    float shootTimer;
    Transform player;
    public bool inCurrentSection = true;
    public float moveSpeed = 10f;
    public Transform bulletSpawnOutsideBottom, bulletSpawnOutsideTop;
    public GameObject shellTop, shellBottom, core;
    public float shellRotateSpeed = 90f;
    int rotDirection = 1;

    float stateTimer;
    public float stateDuration = 5f;
    float realStateDuration = 5f;
    bool defeated;

    public enum ShootState
    {
        OuterCirclesVertical,
        OuterCirclesHorizontal,
        OuterCirclesBoth,
        CenterAndOutside
    }
    public ShootState currentState;

    int explosions;
    public int maxExplosions = 15;
    public GameObject explosionPrefab;
    public float explosionInterval = .5f;
    float explosionTimer;
    public Image screenTransition;
    float transitionAlpha = 0;

    public AudioSource music;

    public Color coreRed, coreBlue;
    public Material coreMaterial;
    EnemyHealth coreHealth;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        music = GameObject.Find("MusicManager").GetComponent<AudioSource>();
        screenTransition = GameObject.Find("ScreenTransition").GetComponent<Image>();
        coreMaterial = core.GetComponent<Renderer>().materials[1];
        coreMaterial.SetColor("_Tint", coreBlue);
        coreHealth = core.GetComponent<EnemyHealth>();
        coreHealth.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(core != null){
            stateTimer += Time.deltaTime;
            if(stateTimer >= realStateDuration){
                realStateDuration = stateDuration + Random.Range(-3f, 3f);
                stateTimer = 0;
                int state = Random.Range(0, 4);
                switch(state){
                    case 0: currentState = ShootState.OuterCirclesHorizontal;
                        break;
                    case 1: currentState = ShootState.OuterCirclesVertical;
                        break;
                    case 2: currentState = ShootState.OuterCirclesBoth;
                        break;
                    case 3: currentState = ShootState.CenterAndOutside;
                        break;
                    
                }
            }

            shootTimer += Time.deltaTime;
            if(shootTimer >= shootInterval){
                if(shellBottom != null){
                    GameObject bulletBottom = (GameObject)Instantiate(bulletPrefab, bulletSpawnOutsideBottom.position, bulletSpawnOutsideBottom.rotation);
                    bulletBottom.transform.GetChild(0).transform.rotation = Quaternion.identity;
                }
                if(shellTop != null){
                    GameObject bulletTop = (GameObject)Instantiate(bulletPrefab, bulletSpawnOutsideTop.position, bulletSpawnOutsideTop.rotation);
                    bulletTop.transform.GetChild(0).transform.rotation = Quaternion.identity;
                }
                if(shellBottom == null && shellTop == null){
                    GameObject bulletCenter1 = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
                    bulletCenter1.transform.GetChild(0).transform.rotation = Quaternion.identity;

                    GameObject bulletCenter2 = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Inverse(transform.rotation));
                    bulletCenter2.transform.GetChild(0).transform.rotation = Quaternion.identity;
                }

                shootTimer = 0;
            }
            

            if(shellBottom == null && shellTop == null){
                coreHealth.enabled = true;
                coreMaterial.SetColor("_Tint", coreRed);
                transform.Rotate(Vector3.up, 360 * rotDirection * Time.deltaTime, Space.Self);
                transform.Rotate(Vector3.forward, 60f * Time.deltaTime, Space.World);
                if(transform.eulerAngles.y > 45)
                    rotDirection = -1;
                if(transform.eulerAngles.y < -45)
                    rotDirection = 1;
            }
            else{
                if(currentState == ShootState.OuterCirclesHorizontal){
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), 4f * Time.deltaTime);
                    if(shellTop != null){
                        shellTop.transform.Rotate(Vector3.forward * shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellTop.transform.localPosition = Vector3.Lerp(shellTop.transform.localPosition, Vector3.zero, 4f * Time.deltaTime);
                    }
                    if(shellBottom != null){
                        shellBottom.transform.Rotate(Vector3.forward * -shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellBottom.transform.localPosition = Vector3.Lerp(shellBottom.transform.localPosition, Vector3.zero, 4f * Time.deltaTime);
                    }
                }
                else if(currentState == ShootState.OuterCirclesVertical){
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,90), 4f * Time.deltaTime);
                    if(shellTop != null){
                        shellTop.transform.Rotate(Vector3.forward * shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellTop.transform.localPosition = Vector3.Lerp(shellTop.transform.localPosition, Vector3.zero, 4f * Time.deltaTime);
                    }
                    if(shellBottom != null){
                        shellBottom.transform.Rotate(Vector3.forward * -shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellBottom.transform.localPosition = Vector3.Lerp(shellBottom.transform.localPosition, Vector3.zero, 4f * Time.deltaTime);
                    }
                }
                else if(currentState == ShootState.OuterCirclesBoth){
                    transform.Rotate(Vector3.forward, 30f * Time.deltaTime);
                    if(shellTop != null){
                        shellTop.transform.Rotate(Vector3.forward * shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellTop.transform.localPosition = Vector3.Lerp(shellTop.transform.localPosition, Vector3.zero, 4f * Time.deltaTime);
                    }
                    if(shellBottom != null){
                        shellBottom.transform.Rotate(Vector3.forward * -shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellBottom.transform.localPosition = Vector3.Lerp(shellBottom.transform.localPosition, Vector3.zero, 4f * Time.deltaTime);
                    }
                }
                else if(currentState == ShootState.CenterAndOutside){
                    transform.Rotate(Vector3.forward, 60f * Time.deltaTime);
                    if(shellTop != null){
                        shellTop.transform.Rotate(Vector3.forward * shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellTop.transform.localPosition = Vector3.Lerp(shellTop.transform.localPosition, Vector3.up * .5f, 4f * Time.deltaTime);
                    }
                    if(shellBottom != null){
                        shellBottom.transform.Rotate(Vector3.forward * -shellRotateSpeed * Time.deltaTime, Space.Self);
                        shellBottom.transform.localPosition = Vector3.Lerp(shellBottom.transform.localPosition, Vector3.up * -.5f, 4f * Time.deltaTime);
                    }
                }
            }
        }
        else{
            defeated = true;
            explosionTimer += Time.deltaTime;
            if(explosionTimer >= explosionInterval && explosions < 15){
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosionTimer = 0;
                explosions ++;
            }
            if(defeated){
                // Switch to thanks for playing scene
                transitionAlpha += Time.deltaTime / 4f;
                screenTransition.color = new Color(1,1,1, transitionAlpha);
                music.volume = 1f - transitionAlpha;
                if(transitionAlpha >= 1f){
                    SceneManager.LoadScene("ThanksScene");
                }
            }
        }
    }



    public void HandleState(){
        switch(currentState){
            // case OuterCirclesHorizontal:

        }
    }

    public void ExitSection(){
        inCurrentSection = false;
    }
}
