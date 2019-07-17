using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    public UnityEvent dustLeftEvent, dustRightEvent;
    public void SpawnStepDustLeft(){
        dustLeftEvent.Invoke();
    }
    public void SpawnStepDustRight(){
        dustRightEvent.Invoke();
    }
}
