using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour {

	public GameObject interactionIcon;

	public InteractableEntity closestInteractable;

	public float minDistanceToInteract;

	Image textBoxBG;
	Text textBox;

	public bool interacting;


	//Dialogue
	Dialog currentDialog;
	int currentDialogLine;


	// Use this for initialization
	void Start () {

		textBoxBG = GameObject.Find("TextBoxBG").GetComponent<Image>();
		textBox = GameObject.Find("TextBox").GetComponent<Text>();
		interactionIcon = GameObject.Find("InteractionIcon");
	}
	
	// Update is called once per frame
	void Update () {
		if(interactionIcon == null || textBox == null || textBoxBG == null){
			interactionIcon = GameObject.Find("InteractionIcon");
			textBoxBG = GameObject.Find("TextBoxBG").GetComponent<Image>();
			textBox = GameObject.Find("TextBox").GetComponent<Text>();

			if (interactionIcon == null || textBox == null || textBoxBG == null)
				return;
		}
		if(closestInteractable != null){
			interactionIcon.SetActive(true);
			interactionIcon.transform.position = closestInteractable.transform.position + Vector3.up * closestInteractable.iconHeightOffset;
		}
		else{
			interactionIcon.SetActive(false);
		}

		if(Input.GetButtonDown("Interact") && closestInteractable != null && !interacting){
			closestInteractable.Interact();
			Vector3 relativePos = closestInteractable.gameObject.transform.position - transform.position;
			relativePos.Normalize();
			relativePos.y = 0;
			GetComponent<PlayerController>().targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
		}

		if(interacting)
			UpdateDialogueInteraction();
	}

	public void StartDialogue(Dialog dialog){
		interacting = true;
		currentDialog = dialog;
		currentDialogLine = -1;
		textBoxBG.enabled = true;
		textBox.enabled = true;
		textBox.text = dialog.dialogueParts[0];
		Camera.main.GetComponent<CameraOrbit>().DisableMovement();
		GetComponent<PlayerController>().canMove = false;
	}

	void UpdateDialogueInteraction(){
		if(Input.GetButtonDown("Interact")){
			if(currentDialogLine >= currentDialog.dialogueParts.Count - 1){
				interacting = false;
				currentDialog = null;
				currentDialogLine = 0;
				textBoxBG.enabled = false;
				textBox.text = "";
				textBox.enabled = false;
				Camera.main.GetComponent<CameraOrbit>().EnableMovement();
				GetComponent<PlayerController>().canMove = true;
			}
			else{
				currentDialogLine++;
				textBox.text = currentDialog.dialogueParts[currentDialogLine];
			}
		}
	}

	private void FixedUpdate() {
		if(!interacting){
			if(closestInteractable != null && Vector3.Distance(transform.position, closestInteractable.transform.position) > minDistanceToInteract)
				closestInteractable = null;

			Collider[] interactables = Physics.OverlapSphere(transform.position + transform.forward * minDistanceToInteract, minDistanceToInteract, LayerMask.GetMask("Interactable"));
			//Debug.DrawLine(transform.position, transform.position + transform.forward * minDistanceToInteract, Color.red, .1f);
			for(int i = 0; i < interactables.Length; i++){
				if(closestInteractable == null || Vector3.Distance(transform.position, closestInteractable.transform.position) > Vector3.Distance(transform.position, interactables[i].transform.position)){
					closestInteractable = interactables[i].GetComponent<InteractableEntity>();
				}
			}
		}
		else{
			Vector3 relativePos = closestInteractable.gameObject.transform.position - transform.position;
			relativePos.Normalize();
			relativePos.y = 0;
			GetComponent<PlayerController>().targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
		}
	}
}
