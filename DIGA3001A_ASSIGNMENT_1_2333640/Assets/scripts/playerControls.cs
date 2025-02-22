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


   
    public bool isInFlintRange = false;

    //torch
    public bool hasLitTorch = false;
    public GameObject torchFlame;
    public bool isInOriginalFlameRange = false;

    public GameObject litTorchText;
    public GameObject lightTorchWords;

    //fire
    public campfireManager campfireManager;
    public GameObject linkFlameText;
    public GameObject flameLinkedText;
    
    public bool isInCampfireRange1 = false;
    public GameObject fire1;
    public bool flameLinked1 = false;

    public bool isInCampfireRange2 = false;
    public GameObject fire2;
    public bool flameLinked2 = false;

    public bool isInCampfireRange3 = false;
    public GameObject fire3;
    public bool flameLinked3 = false;

    public bool isInCampfireRange4 = false;
    public GameObject fire4;
    public bool flameLinked4 = false;

    public bool isInCampfireRange5 = false;
    public GameObject fire5;
    public bool flameLinked5 = false;

    //flint
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
       if(isInOriginalFlameRange == true && hasLitTorch == false)
        {
            torchFlame.SetActive(true);
            lightTorchWords.SetActive(false);    
            hasLitTorch = true; 
            StartCoroutine(TorchLitText());
        }
        
        if(isInCampfireRange1 == true && flameLinked1 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire1.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked1 = true;
            StartCoroutine(FlameLinkedText());
        }

        if (isInCampfireRange2 == true && flameLinked2 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire2.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked2 = true;
            StartCoroutine(FlameLinkedText());
        }

        if (isInCampfireRange3 == true && flameLinked3 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire3.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked3 = true;
            StartCoroutine(FlameLinkedText());
        }

        if (isInCampfireRange4 == true && flameLinked4 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire4.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked4 = true;
            StartCoroutine(FlameLinkedText());
        }

        if (isInCampfireRange5 == true && flameLinked5 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire5.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked5 = true;
            StartCoroutine(FlameLinkedText());
        }

    }

    private void Sprint()
    {
        Debug.Log("should sprint");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Campfire1" && flameLinked1 == false)
        {
            isInCampfireRange1 = true;
            linkFlameText.SetActive(true);
        }
        

        if (other.tag == "OriginalFlame" && hasLitTorch == false)
        {
            
            isInOriginalFlameRange = true;
            lightTorchWords.SetActive(true);
            Debug.Log("hello");
        }

        if (other.tag == "Campfire2" && flameLinked2 == false)
        {
            isInCampfireRange2 = true;
            linkFlameText.SetActive(true);
        }

        if (other.tag == "Campfire3" && flameLinked3 == false)
        {
            isInCampfireRange3 = true;
            linkFlameText.SetActive(true);
        }

        if (other.tag == "Campfire4" && flameLinked4 == false)
        {
            isInCampfireRange4 = true;
            linkFlameText.SetActive(true);
        }

        if (other.tag == "Campfire5" && flameLinked5 == false)
        {
            isInCampfireRange5 = true;
            linkFlameText.SetActive(true);
        }


    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "OriginalFlame")
        {
            isInOriginalFlameRange = false;
            lightTorchWords.SetActive(false);
            Debug.Log("text shouldn't show");
        }

        if (other.tag == "Campfire1")
        {
            isInCampfireRange1 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
        }

        if (other.tag == "Campfire2")
        {
            isInCampfireRange2 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
        }

        if (other.tag == "Campfire3")
        {
            isInCampfireRange3 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
        }

        if (other.tag == "Campfire4")
        {
            isInCampfireRange4 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
        }

        if (other.tag == "Campfire5")
        {
            isInCampfireRange5 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
        }


    }

    private IEnumerator StopText()
    {
        yield return new WaitForSeconds(2f);
        //pickUpFlintText.SetActive(false);
        linkFlameText.SetActive(false);
    }

    private IEnumerator FlameLinkedText()
    {
        yield return new WaitForSeconds(0f);
        flameLinkedText.SetActive(true);
        yield return new WaitForSeconds(4f);
        flameLinkedText.SetActive(false);
           
        
    }

    private IEnumerator TorchLitText()
    {
        yield return new WaitForSeconds(0f);
        litTorchText.SetActive(true);
        yield return new WaitForSeconds(4f);
        litTorchText.SetActive(false);


    }
}
