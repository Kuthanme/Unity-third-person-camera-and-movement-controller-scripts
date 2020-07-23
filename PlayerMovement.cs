//Reference script:https://www.youtube.com/watch?v=qITXjT9s9do

using UnityEngine;
using System.Collections;
using System.IO.IsolatedStorage;

public class PlayerMovement : MonoBehaviour
{
	public float currentSpeed;
	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = -9.8f;
	public float jumpHeight = 1;
	public float velocityY = 0;
	public bool isGrounded = true;
	public bool isJumping = false;

	[Range(0, 1)]
	public float airControlPercent;
	public float turnSmoothTime = 0.17f;
	public float ShootingTurnSmoothTime = 0.13f;
	public float speedSmoothTime = 0.13f;
	public float airSmoothTime = 0.45f;

	float turnSmoothVelocity;
	float speedSmoothVelocity;
	float targetRotation;
	float targetSpeed;

	Animator animator;
	Transform cameraT;
	CharacterController controller;

	void Start()
	{
		animator = GetComponent< Animator >();
		cameraT = Camera.main.transform;
		controller = GetComponent< CharacterController >();
	}

	void Update()
	{
		isGrounded = controller.isGrounded;
		// input (WASD)
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0,Input.GetAxisRaw("Vertical"));
		Vector3 inputDir = input.normalized;
		bool isRunning = Input.GetKey(KeyCode.LeftShift);

		Movement(inputDir, isRunning);
		JumpSystem();

		// animator
		float animationSpeedPercent = ((isRunning) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
		animator.SetBool("isJumping", isJumping);
		animator.SetBool("isGrounded", isGrounded);
		animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);	
	}

	void Movement(Vector3 inputDir, bool isRunning)
	{
		if (inputDir != Vector3.zero)
		{ // player have input 
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
		}

		targetSpeed = ((isRunning) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetEnvSmoothTime(inputDir, speedSmoothTime));
		velocityY += Time.deltaTime * gravity;


		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
		controller.Move(velocity * Time.deltaTime);
		currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

		if (controller.isGrounded)
		{
			velocityY = 0;
		}
	}

	void JumpSystem()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded )
		{
			float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
			isJumping = true;
		}
		else
		{
			isJumping = false;
		}
	}
	
	float GetEnvSmoothTime(Vector3 inputDir, float smoothTime)
	{ // smooth time in which enviroment
		if (isGrounded)
		{ // Character controller on the ground
			return smoothTime;
		}
		else if (airControlPercent == 0 && inputDir != Vector3.zero)
		{ // Character controller in the air without air control, and player have input
			return float.MaxValue;
		}
		else if (airControlPercent == 0)
		{ // Character controller in the air without air control, and player have no input 
			return airSmoothTime;
		}
		else
		{ // Character controller in the air with air control
			return smoothTime / airControlPercent; 
		}
	}
}