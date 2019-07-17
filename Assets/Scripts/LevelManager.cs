using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
    public List<EnemySection> sections;
    List<EnemySection> sectionList;
    float timerSections;

    public GameObject enemySimple, enemyMiddle, enemyHard, enemyBoss;
    public EnemySection currentSection, nextSection;
    public GameObject levelBoss;
    bool reachedEnd;

    private void Start() {
        sectionList = new List<EnemySection>(sections);
        currentSection = sectionList[0];
        sectionList.RemoveAt(0);
        SpawnEnemies(currentSection);
    }
    private void Update() {
        if(!reachedEnd)
            timerSections += Time.deltaTime;

        if(timerSections >= currentSection.duration){
            timerSections = 0;
            
            if(sectionList.Count >= 1){
                currentSection = sectionList[0];
                SpawnEnemies(currentSection);
                sectionList.RemoveAt(0);
            }
            else{
                reachedEnd = true;
                Instantiate(levelBoss, Vector3.forward * 10f, Quaternion.identity);
            }
            
        }
        
    }

    void SpawnEnemies(EnemySection section){
        if(section.sectionEnd)
            StopCurrentSection();
        for(int j = 0; j < section.simpleEnemyPos.Count; j ++){
            Instantiate(enemySimple, section.simpleEnemyPos[j], Quaternion.identity);
        }
        for(int j = 0; j < section.mediumEnemyPos.Count; j ++){
            Instantiate(enemyMiddle, section.mediumEnemyPos[j], Quaternion.identity);
        }
        for(int j = 0; j < section.hardEnemyPos.Count; j ++){
            Instantiate(enemyHard, section.hardEnemyPos[j], Quaternion.identity);
        }
    }

    void StopCurrentSection(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++){
            enemies[i].SendMessage("ExitSection");
        }
    }
}