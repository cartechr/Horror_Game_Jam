using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public bool isGrounded;
    public LayerMask groundLayers;

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
    Animator animator;

    const float threshold = 0.01f;

    //Cinemachine
    float cinemachineTargetPitch;

    //Player
    float rotationVelocity;
    float verticalVelocity;
    float speed;
    float animationBlend;

    //Animation Related
    bool hasAnimator;
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
        float targetSpeed = isSprinting ? sprintSpeed : moveSpeed;

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
    }

    //Look Call
    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
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

    #endregion



}
