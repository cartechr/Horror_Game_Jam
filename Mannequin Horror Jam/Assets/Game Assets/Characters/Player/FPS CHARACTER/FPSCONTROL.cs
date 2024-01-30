using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class FPSCONTROL : MonoBehaviour
{

    [Header("Player Movement")]
    public Vector2 move;
    public Vector2 look;
    public float moveSpeed = 4f;
    public float sprintSpeed = 6f;
    public float crouchSpeed = 3f;
    public float rotationSpeed = 1f;
    public float speedChangeRate = 10f;


    [Header("Player Controls")]
    public bool isWalking;
    public bool isSprinting;
    public bool isCrouching;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Grounded and Gravity Checks")]
    public float gravity;
    public float fallSpeed;
    public bool isGrounded;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayers;
    private float fallTimeoutDelta;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;
    private float terminalVelocity = 53.0f;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.5f;

    [Header("Cinemachine")]
    public GameObject cinemachineCamera;
    public float topClamp = 90f;
    public float bottomClamp = -90f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;


    //Private assignments
    CharacterController characterController;
    PlayerInput playerInput;
    GameObject mainCamera;
    public Animator animator;

    const float threshold = 0.01f;

    //Cinemachine
    float cinemachineTargetPitch;

    //Player
    float rotationVelocity;
    float verticalVelocity;
    float speed;
    float animationBlend;

    //Animation Related
    public bool hasAnimator;
    int animIDSpeed;
    int animIDGrounded;
    int animIDFreeFall;
    int animIDMotionSpeed;

    private void Awake()
    {
        // get a reference to our main camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {

        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        AssignAnimationIDs();

    }

    private void Update()
    {

        hasAnimator = TryGetComponent(out animator);
        
        Move();

        CheckCrouchingBackwards();

        GravityControls();

        GroundedCheck();

    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    #region Animation Related

    void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDGrounded = Animator.StringToHash("Grounded");
    }


    #endregion

    #region Camera Controls
    void CameraRotation()
    {
        //if there is an input
        if(look.sqrMagnitude >= threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = 1f;

            cinemachineTargetPitch += look.y * rotationSpeed * deltaTimeMultiplier;
            rotationVelocity = look.x * rotationSpeed * deltaTimeMultiplier;

            //Clamp the pitch rotation
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            //Update Cinemachine camera target pitch
            cinemachineCamera.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0f, 0f);

            //Rotate the player left and right
            transform.Rotate(Vector3.up * rotationVelocity);

        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    #endregion

    #region Cursor State

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }


    #endregion

    #region Movement Related

    void Move()
    {
        //if the player is sprinting then use sprint speed if not then move speed
        float targetSpeed;

        if (isCrouching && !isSprinting)
        {
            targetSpeed = crouchSpeed;
        }
        else
        {
            
            targetSpeed = isSprinting ? sprintSpeed : moveSpeed;
        }
        

        //if there is no input, set the target speed to 0
        if (move == Vector2.zero) targetSpeed = 0f;

        //a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0f, 
            characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        float inputMagnitude = analogMovement ? move.magnitude : 1f;

        //accelerate or decelerate to target speed
        if(currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            //Creates curved result rather than a linear one giving a more organic speed change
            //note T in Lerp is clamped, so we don't need to clamp our speed
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                Time.deltaTime * speedChangeRate);

            //round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;

        }
        else
        {
            speed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        //normalise input direction
        Vector3 inputDirection = new Vector3(move.x, 0f, move.y).normalized;

        if(move != Vector2.zero)
        {
            //move 
            inputDirection = transform.right * move.x + transform.forward * move.y;
        }

        //move the player
        characterController.Move(inputDirection.normalized * (speed * Time.deltaTime) 
            + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);

        if (hasAnimator)
        {
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        }


        if (move.y == -1 && !isCrouching)
        {
            animator.SetBool("standingBackwards", true);
        }
        
        if(move.y >= 0 && !isCrouching)
        {
            animator.SetBool("standingBackwards", false);
        }

        float strafeValue = move.x;

        if(move.x < 0 || move.x > 0)
        {
            animator.SetBool("Strafing", true);
            animator.SetFloat("StrafeValue", strafeValue);
        }
        else
        {
            animator.SetBool("Strafing", false);
        }

        if (move.x < 0 || move.x > 0 && isCrouching)
        {
            animator.SetBool("crouchStrafing", true);
            animator.SetFloat("StrafeValue", strafeValue);
        }
        else
        {
            animator.SetBool("crouchStrafing", false);
        }

    }

    void CheckCrouchingBackwards()
    {

        if (move.y == -1)
        {
            animator.SetBool("crouchingBackwards", true);
        }

        if (move.y >= 0)
        {
            animator.SetBool("crouchingBackwards", false);
        }
    }


    #endregion

    #region Input Related

    //Movement Call
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    //Sprint Call
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);

        if(isCrouching && value.isPressed)
        {
            //Cancel crouching
            HandleCrouch(false);
        }

    }

    //Look Call
    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    //Crouch Call
    public void OnCrouch(InputValue value)
    {
        if (value.isPressed)
        {
            isCrouching = !isCrouching;

            HandleCrouch(isCrouching);
        }
    }

    //Movement Input
    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    //Sprint Input
    public void SprintInput(bool newSprintState)
    {
        isSprinting = newSprintState;
    }

    //Look Input
    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    //Crouch Input
    public void HandleCrouch(bool crouch)
    {
        isCrouching = crouch;

        if (crouch)
        {
            animator.SetBool("isCrouching", true);

            characterController.height = 1.5f;

            //Debug.Log("Crouching activated");
            
        }
        else
        {
            animator.SetBool("isCrouching", false);

            characterController.height = 2.5f;

            //Debug.Log("Crouching deactivated");
        }


       
    }

    #endregion

    #region Gravity Related

    void GravityControls()
    {

        if (isGrounded)
        {
            // reset the fall timeout timer
            fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

        }
        else
        {

            // fall timeout
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }

        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    #endregion

}
