using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {

	public float sensitivity = 3f;
	public float distance = 5f;
	public Transform target;
	public Vector3 offset;
	Vector2 input;

	public Vector2 limitX, limitY;
	public bool xLimitEnabled, yLimitEnabled;
	public bool smoothY;
	public float smoothYSpeed = 10f;
	float lerpY;

	bool canRotate = true;

	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		if(canRotate && target != null){
			input.x += Input.GetAxis("Mouse X") * sensitivity;
			input.y -= Input.GetAxis("Mouse Y") * sensitivity;
		}

		if(xLimitEnabled)
			input.x = Mathf.Clamp(input.x, limitX.x, limitX.y);
		if(yLimitEnabled)
			input.y = Mathf.Clamp(input.y, limitY.x, limitY.y);
	}

	private void LateUpdate() {
		if(target == null){
			target = GameObject.FindGameObjectWithTag("Player").transform;
			if (target == null)
				return;
		}
		Quaternion rot = Quaternion.Euler(input.y, input.x, 0);
		transform.rotation = rot;

		Vector3 pos = rot * Vector3.back * distance;
		
		RaycastHit hit;
		if(Physics.Raycast(target.position, pos.normalized, out hit, distance, LayerMask.GetMask("Map"))){
			transform.position = hit.point + pos * -.1f;
		}
		else{
			if(smoothY){
				Vector3 targetPos = target.position;
				lerpY = Mathf.Lerp(lerpY, targetPos.y, smoothYSpeed * Time.deltaTime);
				transform.position = new Vector3(targetPos.x, lerpY, targetPos.z) + pos;
			}
			else{
				transform.position = target.position + pos + target.forward * offset.z + target.right * offset.x + target.up * offset.y;
			}
		}
	}

	public void EnableMovement(){
		canRotate = true;
	}

	public void DisableMovement(){
		canRotate = false;
	}

	public float GetRotationX(){
		return input.x;
	}

	public float GetRotationY(){
		return input.y;
	}
}
