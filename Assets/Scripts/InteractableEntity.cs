using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableEntity : MonoBehaviour {

	public Dialog meetDialogue, repeatDialogue;
	public bool alreadyMet;

	Transform player;

	public UnityEvent eventOnInteract;
	public float iconHeightOffset;


	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	public void Interact(){
		eventOnInteract.Invoke();
	}

	public Dialog Talk(){
		if(alreadyMet)
			return repeatDialogue;
		else
			return meetDialogue;
	}

	public void PrintText(){
		print(Talk().dialogueParts[0]);
	}

	public void StartDialogue(){
		player.GetComponent<InteractionManager>().StartDialogue(Talk());
	}
}
