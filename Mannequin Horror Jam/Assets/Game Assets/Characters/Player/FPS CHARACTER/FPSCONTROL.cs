using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputs))]
public class FPSCONTROL : MonoBehaviour
{

    [Header("Player Movement")]
    public Vector2 move;
    public Vector2 look;
    public float moveSpeed = 4f;
    public float sprintSpeed = 6f;
    public float crouchSpeed = 3f;
    public float rotationSpeed = 1f;


    [Header("Player Controls")]
    public bool isWalking;
    public bool isSprinting;
    public bool isCrouching;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;



    [Header("Grounded and Gravity Checks")]
    public float gravity;
    public bool isGrounded;
    public LayerMask groundLayers;

    [Header("Cinemachine")]
    public GameObject cinemachineCamera;
    public float topClamp = 90f;
    public float bottomClamp = -90f;


    //Private assignments
    CharacterController characterController;
    PlayerInput playerInput;
    GameObject mainCamera;

    const float threshold = 0.01f;

    //Cinemachine
    float cinemachineTargetPitch;

    //Player
    float rotationVelocity;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {

        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    #region Camera Controls
    void CameraRotation()
    {
        //if there is an input
        if(look.sqrMagnitude >= threshold)
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


    #region Movement Related




    #endregion

    #region Input Related
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    #endregion

}
