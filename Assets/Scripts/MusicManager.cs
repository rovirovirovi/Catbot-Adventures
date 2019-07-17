using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource levelMusic, bossMusic;
    
    bool inBossFight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inBossFight){
            levelMusic.volume = Mathf.Lerp(levelMusic.volume, 0f, 1f * Time.deltaTime);
            bossMusic.volume = Mathf.Lerp(bossMusic.volume, 0f, 1f * Time.deltaTime);
        }
    }

    public void PlayBossMusic(){
        inBossFight = true;
        bossMusic.Play();
    }
    
}
