using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

	public float speedGrounded = 130f;
	public float acceleration = 100f;
	public float jumpForce = 6f;
	public float gravity = 20f;
	float speedSmoothDamp;

	public Vector3 velocity;
	public Vector3 velocityLimit;
	public float minVelocityToStop = .2f;
	public float dragScale = 10f;
	public float maxSlopeAngle = 45f;
	Rigidbody rigidbody;
	public CameraOrbit camera;
	Quaternion rotation;
	Vector3 input;

	bool grounded;
	Vector3 groundPos;
	public float heightOffGround = .3f;
	public float maxDistanceToUnstick = .6f;
	public bool canMove = true;

	public Quaternion targetRotation;
	public float rotationLerpDelta = 15f;
	public Animator animator;
	public UnityEvent onGroundedEvent;
	public Transform model;

	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		camera = Camera.main.gameObject.GetComponent<CameraOrbit>();
		groundPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
	}


	void Movement(){

		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f)){
			if(velocity.y <= 0f && hit.collider.gameObject != gameObject && Vector3.Angle(Vector3.up, hit.normal) <= maxSlopeAngle){
				if(hit.distance <= heightOffGround + 1.1f){
					Debug.DrawLine(transform.position, transform.position - Vector3.up * (1.1f + heightOffGround), Color.red, .1f);
					if(!grounded)
						onGroundedEvent.Invoke();
					grounded = true;
				}
				if(hit.distance <= maxDistanceToUnstick + 1.1f && grounded){
					Debug.DrawLine(transform.position, transform.position - Vector3.up * (1.1f + maxDistanceToUnstick), Color.red, .1f);
					groundPos = hit.point;
				}
				if(hit.distance > maxDistanceToUnstick + 1.1f){
					grounded = false;
				}

			}
		}
		else
		{
			Debug.DrawLine(transform.position, transform.position - Vector3.up * (1.1f + heightOffGround), Color.blue, .1f);
			grounded = false;
		}

		//Apply gravity
		if(!grounded){
			velocity.y -= gravity * Time.deltaTime;
		}
		else{
			velocity.y = 0;
		}

		//Handle input
		input.x = Input.GetAxisRaw("Horizontal");
		input.z = 0;
		input.y = 0;

		//Apply movement from input
		input = Camera.main.transform.TransformDirection(input);
		input.y = 0;

		Vector3 finalInput = input.normalized;

		input.x = 0;
		input.z = Input.GetAxisRaw("Vertical");
		input = Camera.main.transform.TransformDirection(input);
		input.y = 0;

		finalInput += input.normalized;

		finalInput.Normalize();

		if(!canMove){
			finalInput.x = 0;
			finalInput.z = 0;
		}

		velocity.x += speedGrounded * finalInput.x * Time.deltaTime;
		velocity.z += speedGrounded * finalInput.z * Time.deltaTime;

		//Limit velocity
		Vector3 velCopy = velocity;
		velCopy.y = 0;
		velCopy = Vector3.ClampMagnitude(velCopy, velocityLimit.x);
		velocity.x = velCopy.x;
		velocity.z = velCopy.z;
		
		//Handle jumping
		if(Input.GetButtonDown("Jump") && grounded && canMove){
			velocity.y = jumpForce;
			grounded = false;
		}

		//Apply drag
		UpdateDrag();

		Vector3 v = velocity;
		v.y = 0;

		print(Vector3.SignedAngle(v, transform.forward, Vector3.up));
		float rot = Vector3.SignedAngle(v, transform.forward, Vector3.up) / 2f;
		rot = Mathf.Clamp(rot, -10f, 10f);
		model.localRotation = Quaternion.Lerp(model.localRotation, Quaternion.Euler(0, 0, rot), 15 * Time.deltaTime);

		animator.SetFloat("inputMagnitude", v.magnitude / velocityLimit.x);
		animator.SetBool("grounded", grounded);
		v.Normalize();

		if (v.magnitude > 0){
			targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v, Vector3.up), rotationLerpDelta * Time.deltaTime);
		}

		targetRotation.Normalize();
		rigidbody.MoveRotation(targetRotation);

		rigidbody.velocity = Vector3.zero;

		rigidbody.MovePosition((grounded ? groundPos + Vector3.up * (heightOffGround + 1) : transform.position) + velocity * Time.deltaTime);
		
		
	}

	private void LateUpdate() {
		//Apply velocity
		

		//Apply rotation
		// rigidbody.MoveRotation(Quaternion.Euler(0, camera.GetRotationX(), 0));
	}

	void UpdateDrag(){
		if(velocity.magnitude <= minVelocityToStop)
			velocity = Vector2.zero;
		else{
			Vector3 drag = velocity.normalized * dragScale * Time.deltaTime;
			drag.y = 0;
			velocity -= drag;
		}
	}


}
