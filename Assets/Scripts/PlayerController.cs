using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]


public class PlayerController : MonoBehaviour
{
    private Vector2 m_input;
    private CharacterController m_controller;
    private Vector3 m_moveDir = Vector3.zero;
    private float m_verticalVelocity;
    private float m_gravityValue = 8.81f;
    public float m_turnSmoothTime = 0.1f;
    public float m_turnSmoothVelocity;
    public Transform m_cam;
    public Camera m_camera;
    public float m_Force = 3.0f;    
	public float m_JumpForce = 8f;
	public float m_RotationSpeed = 500f;

    
    public Vector3 m_lastMoveDir = Vector3.zero;
    public float m_smoothTime = 0f;
    public float m_targetSpeed = 0;
	public Vector3 m_PlayerVelocity;
	[SerializeField] public float m_fSpeed = 5.0f;


	private void Awake()
	{
        m_controller = GetComponent<CharacterController>();
        m_camera = Camera.main;
		m_cam = m_camera.transform;
	}



	void HandleMovement()
	{
		if (m_moveDir.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(m_moveDir.x, m_moveDir.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_RotationSpeed, 0.1f);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

			m_PlayerVelocity = moveDirection * m_fSpeed;

		}
	}


	void ApplyGravity()
	{
		float dTime = Time.fixedDeltaTime;

		bool groundedPlayer = m_controller.isGrounded;
		if (groundedPlayer == false)
		{
			m_verticalVelocity -= m_gravityValue * Time.deltaTime;
		}


		if (groundedPlayer && m_verticalVelocity < 0f)
		{
			m_verticalVelocity = 0f;
		}
	}




	/// <summary>
	/// Callback event from the input action library.  This will send in 
	/// new information regarding direction the player is moving.
	/// </summary>
	/// <param name="context"></param>
	public void Move(InputAction.CallbackContext context)
	{
		m_input = context.ReadValue<Vector2>();
		m_moveDir = new Vector3(m_input.x, 0.0f, m_input.y * 2.0f);
	}





    // Update is called once per frame
    void Update()
    {
        float dTime = Time.deltaTime;

		ApplyGravity();

		Vector3 move = m_moveDir;
        move = transform.TransformDirection(move.normalized);
        if (move == Vector3.zero)
        {
            m_smoothTime = 0.8f;
            m_targetSpeed = 0f;
        }
        else
        {
            m_lastMoveDir = move;
            m_targetSpeed = 1.0f;
            m_smoothTime = 0.3f;
        }

        m_moveDir.y = m_verticalVelocity;
		m_controller.Move(m_moveDir * m_fSpeed * dTime);
	}
}
