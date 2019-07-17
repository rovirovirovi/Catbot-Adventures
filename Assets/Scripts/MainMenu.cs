using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   
    public int selectedButtonIndex = 0;
    public RectTransform selectedButton, btnPlay, btnQuit;
    float checkInputTimer;
    public Image transition;

    bool loadingGame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkInputTimer += Time.deltaTime;
        if(checkInputTimer >= .2f){
            if(Input.GetAxis("Vertical") < -0.2f || Input.GetAxis("Vertical") > 0.2f){
                checkInputTimer = 0;
                if(selectedButtonIndex == 0)
                    selectedButtonIndex = 1;
                else
                    selectedButtonIndex = 0;
            }
        }

        if(selectedButtonIndex == 0)
            selectedButton.position = btnPlay.position;
        else
            selectedButton.position = btnQuit.position;

        if(Input.GetButtonDown("Shoot")){
            if(selectedButtonIndex == 0){
                loadingGame = true;
            }
            else{
                Application.Quit();
            }
        }

        if(loadingGame){
            transition.color = Color.Lerp(transition.color, new Color(1,1,1,1), Time.deltaTime);
            if(transition.color.a >= .95f){
                SceneManager.LoadScene("Level1");
            }
        }
    }
}
