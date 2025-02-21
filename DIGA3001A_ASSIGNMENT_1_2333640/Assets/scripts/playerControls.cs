using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]

    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
    // Private variables to store input values and the character controller
    private Vector2 _moveInput; // Stores the movement input from the player
    private Vector2 _lookInput; // Stores the look input from the player
    private float _verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 _velocity; // Velocity of the player
    private CharacterController _characterController; // Reference to the CharacterController component

    
    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1f; //make short
    public float standingHeight = 2f; //make normal
    public float crouchSpeed = 1.5f; //short speed
    public bool isCrouching = false; //if short or normal


    public bool isInCampfireRange = false;
    public bool isInFlintRange1 = false;

    //fire
    public GameObject linkFlameText;
    public GameObject fire1;

    //flint
    public GameObject pickUpFlintText;
    public flintManager flintManager;
    public GameObject firstFlint;
    

    private void OnEnable()
    {

        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => _moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => _lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed

        //playerInput.Player.LookAround.performed += ctx => currentScheme = ctx.control;
        playerInput.Player.LookAround.canceled += ctx => _lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed


        // Subscribe to the pick-up input event
        playerInput.Player.LightFire.performed += ctx => LightFire(); // Call the PickUpObject method when pick-up input is performed

        //Subscribe to the Pause
        playerInput.Player.Sprint.performed += ctx => Sprint(); // pause the game

       
    }

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        _characterController = GetComponent<CharacterController>();
    }

        private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }

    private void Move()
    {
        
            // Create a movement vector based on the input
            Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);

            // Transform direction from local to world space
            move = transform.TransformDirection(move);

            var currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

            // Move the character controller based on the movement vector and speed
            _characterController.Move(move * currentSpeed * Time.deltaTime);
        
    }

    private void LookAround()
    {
       
            // Get horizontal and vertical look inputs and adjust based on sensitivity
            var lookX = _lookInput.x * lookSpeed;
            var lookY = _lookInput.y * lookSpeed;

            // Horizontal rotation: Rotate the player object around the y-axis
            transform.Rotate(0, lookX, 0);

            // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
            _verticalLookRotation -= lookY;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -15f, 20f);

            // Apply the clamped vertical rotation to the player camera
            playerCamera.localEulerAngles = new Vector3(_verticalLookRotation, 0, 0);
        
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -0.5f; // Small value to keep the player grounded
        }

        _velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        _characterController.Move(_velocity * Time.deltaTime); // Apply the velocity to the character
    }

    private void Jump()
    {

        if (_characterController.isGrounded)
        {
            // Calculate the jump velocity
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
            
       
    }

    private void LightFire()
    {
        if(isInCampfireRange == true && flintManager.flint > 0.99)
        {
            Debug.Log("should have lit fire");
            fire1.SetActive(true);
            isInCampfireRange = false;
            flintManager.decreaseFlint();
        }
        else
        {
            Debug.Log("No flint");
        }
        if(isInFlintRange1 == true)
        {
            Debug.Log("pick up flint");
            Destroy(firstFlint);
            flintManager.addFlint();
            
        }
            
    }

    private void Sprint()
    {
        Debug.Log("should sprint");
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Campfire")
        {
            isInCampfireRange = true;
            linkFlameText.SetActive(true);
        }
        else
        {
            isInCampfireRange= false;
            linkFlameText.SetActive(false);
        }

        if(other.tag == "Flint")
        {
            isInFlintRange1 = true;
            pickUpFlintText.SetActive(true);
        }
        else
        {
            isInFlintRange1 = false;
            pickUpFlintText.SetActive(false);
        }

        
    }
}
