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

    public flintManager flintManager; //this is actually now the campfire manager

    //fuel
    public GameObject pickUpFuelText;
    public bool isInFuelRange = false;
    public bool isInFuelRange1 = false;
    public bool isInFuelRange2 = false;
    public bool isInFuelRange3 = false;
    public fuelManager fuelManager;

    public GameObject fuel;
    public GameObject fuel1;
    public GameObject fuel2;
    public GameObject fuel3;

    //hope
    public hopeManager hopeManager;
    public bool decreaseHope = false;
    public bool increaseHope = false;
    public GameObject hopeUI;

    //keys and gates
    public GameObject openGateText;
    public GameObject noKeyText;
    public GameObject pickupKeyText;

    public bool isInGreenGateRange = false;
    public GameObject greenGate;
   
    public bool isInGreenKeyRange = false;
    public bool hasGreenKey;
    public GameObject greenKey;
    public GameObject greenKeyPic;

    public bool isInBlueGateRange = false;
    public GameObject blueGate;

    public bool isInBlueKeyRange = false;
    public bool hasBlueKey;
    public GameObject blueKey;
    public GameObject blueKeyPic;

    public bool isInRedGateRange = false;
    public GameObject redGate;

    public bool isInRedKeyRange = false;
    public bool hasRedKey;
    public GameObject redKey;
    public GameObject redKeyPic;

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


        // Subscribe to the light fire input event
        playerInput.Player.LightFire.performed += ctx => LightFire(); // Call the PickUpObject method when pick-up input is performed

        //Subscribe to the sprint
        playerInput.Player.Sprint.performed += ctx => Sprint(); // sprint

        //Subscribe to the UseFuel
        playerInput.Player.UseFuel.performed += ctx => UseFuel(); // use fuel

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

    private void UseFuel()
    {
        Debug.Log("do the thing");
        fuelManager.subtractFuel();
        hopeManager.UseFuel();

    }

    private void LightFire()
    {
       if(isInOriginalFlameRange == true && hasLitTorch == false)
        {
            torchFlame.SetActive(true);
            lightTorchWords.SetActive(false);    
            hasLitTorch = true; 
            StartCoroutine(TorchLitText());
            increaseHope = true;
            decreaseHope = false;
            hopeUI.SetActive(true);
        }
        
        if(isInCampfireRange1 == true && flameLinked1 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire1.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked1 = true;
            StartCoroutine(FlameLinkedText());
            increaseHope = true;
            decreaseHope = false;
        }

        if (isInCampfireRange2 == true && flameLinked2 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire2.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked2 = true;
            StartCoroutine(FlameLinkedText());
            increaseHope = true;
            decreaseHope = false;
        }

        if (isInCampfireRange3 == true && flameLinked3 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire3.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked3 = true;
            StartCoroutine(FlameLinkedText());
            increaseHope = true;
            decreaseHope = false;
        }

        if (isInCampfireRange4 == true && flameLinked4 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire4.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked4 = true;
            StartCoroutine(FlameLinkedText());
            increaseHope = true;
            decreaseHope = false;
        }

        if (isInCampfireRange5 == true && flameLinked5 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire5.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            flameLinked5 = true;
            StartCoroutine(FlameLinkedText());
            increaseHope = true;
            decreaseHope = false;
        }

        if(isInFuelRange == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange = false;
        }

        if (isInFuelRange1 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel1);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange1 = false;
        }

        if (isInFuelRange2 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel2);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange2 = false;
        }

        if (isInFuelRange3 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel3);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange3 = false;
        }

        if(isInGreenGateRange == true && hasGreenKey == true)
        {
            Debug.Log("open gate");
            Destroy(greenGate);
            isInGreenGateRange = false;
            openGateText.SetActive(false);
        }

        if(isInGreenKeyRange == true)
        {
            Debug.Log("pick up key");
            hasGreenKey = true;
            Destroy(greenKey);
            pickupKeyText.SetActive(false);
            greenKeyPic.SetActive(true );
            isInGreenKeyRange = false;
        }

        if (isInGreenGateRange == true && hasGreenKey == false)
        {
            Debug.Log("don't have key");
            isInGreenGateRange = false;
            openGateText.SetActive(false);
            StartCoroutine(NeedKey());
        }

        if (isInBlueGateRange == true && hasBlueKey == true)
        {
            Debug.Log("open gate");
            Destroy(blueGate);
            isInBlueGateRange = false;
            openGateText.SetActive(false);
        }

        if (isInBlueKeyRange == true)
        {
            Debug.Log("pick up key");
            hasBlueKey = true;
            Destroy(blueKey);
            pickupKeyText.SetActive(false);
            blueKeyPic.SetActive(true);
            isInBlueKeyRange = false;
        }

        if (isInBlueGateRange == true && hasBlueKey == false)
        {
            Debug.Log("don't have key");
            isInBlueGateRange = false;
            openGateText.SetActive(false);
            StartCoroutine(NeedKey());
        }

        if (isInRedGateRange == true && hasRedKey == true)
        {
            Debug.Log("open gate");
            Destroy(redGate);
            isInRedGateRange = false;
            openGateText.SetActive(false);
        }

        if (isInRedKeyRange == true)
        {
            Debug.Log("pick up key");
            hasRedKey = true;
            Destroy(redKey);
            pickupKeyText.SetActive(false);
            redKeyPic.SetActive(true);
            isInRedKeyRange = false;
        }

        if (isInRedGateRange == true && hasRedKey == false)
        {
            Debug.Log("don't have key");
            isInRedGateRange = false;
            openGateText.SetActive(false);
            StartCoroutine(NeedKey());
        }
    }

    private void Sprint()
    {
        Debug.Log("should sprint");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Campfire1" && flameLinked1 == false && hasLitTorch == true)
        {
            isInCampfireRange1 = true;
            linkFlameText.SetActive(true);
        }

        if(other.tag == "Campfire1" && flameLinked1 == true)
        {
            increaseHope = true;
            decreaseHope = false;
        }
        

        if (other.tag == "OriginalFlame" && hasLitTorch == false)
        {
            
            isInOriginalFlameRange = true;
            lightTorchWords.SetActive(true);
            Debug.Log("hello");
        }

        if (other.tag == "OriginalFlame" && hasLitTorch == true)
        {
            increaseHope = true;
            decreaseHope = false;
        }


        if (other.tag == "Campfire2" && flameLinked2 == false && hasLitTorch == true)
        {
            isInCampfireRange2 = true;
            linkFlameText.SetActive(true);
           
        }
        
        if (other.tag == "Campfire2" && flameLinked2 == true)
        {
            increaseHope = true;
            decreaseHope = false;
        }


        if (other.tag == "Campfire3" && flameLinked3 == false && hasLitTorch == true)
        {
            isInCampfireRange3 = true;
            linkFlameText.SetActive(true);
           
        }

        if (other.tag == "Campfire3" && flameLinked3 == true)
        {
            increaseHope = true;
            decreaseHope = false;
        }

        if (other.tag == "Campfire4" && flameLinked4 == false && hasLitTorch == true)
        {
            isInCampfireRange4 = true;
            linkFlameText.SetActive(true);
         
        }

        if (other.tag == "Campfire4" && flameLinked4 == true)
        {
            increaseHope = true;
            decreaseHope = false;
        }

        if (other.tag == "Campfire5" && flameLinked5 == false && hasLitTorch == true)
        {
            isInCampfireRange5 = true;
            linkFlameText.SetActive(true);
            
        }

        if (other.tag == "Campfire5" && flameLinked5 == true)
        {
            increaseHope = true;
            decreaseHope = false;
        }

        if(other.tag == "FuelTrigger" && isInFuelRange == false)
        {
            isInFuelRange = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger1" && isInFuelRange == false)
        {
            isInFuelRange1 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger2" && isInFuelRange == false)
        {
            isInFuelRange2 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger3" && isInFuelRange == false)
        {
            isInFuelRange3 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "GreenGateTrigger" && isInGreenGateRange == false)
        {
            openGateText.SetActive(true);
            isInGreenGateRange = true;
            Debug.Log("gate text should show up");

        }

        if (other.tag == "GreenKeyTrigger" && isInGreenKeyRange == false)
        {
            pickupKeyText.SetActive(true);
            isInGreenKeyRange = true;
            Debug.Log("key text should show up");

        }

        if (other.tag == "BlueGateTrigger" && isInBlueGateRange == false)
        {
            openGateText.SetActive(true);
            isInBlueGateRange = true;
            Debug.Log("gate text should show up");

        }

        if (other.tag == "BlueKeyTrigger" && isInBlueKeyRange == false)
        {
            pickupKeyText.SetActive(true);
            isInBlueKeyRange = true;
            Debug.Log("key text should show up");

        }

        if (other.tag == "RedGateTrigger" && isInRedGateRange == false)
        {
            openGateText.SetActive(true);
            isInRedGateRange = true;
            Debug.Log("gate text should show up");

        }

        if (other.tag == "RedKeyTrigger" && isInRedKeyRange == false)
        {
            pickupKeyText.SetActive(true);
            isInRedKeyRange = true;
            Debug.Log("key text should show up");

        }

        if (other.tag == "Killbox")
        {
            transform.position = new Vector3(-71.13f, -1f, -104.81f);
            Debug.Log("should've teleported player");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "OriginalFlame" && hasLitTorch == true)
        {
            isInOriginalFlameRange = false;
            lightTorchWords.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;
        }

        if (other.tag == "OriginalFlame" && hasLitTorch == false)
        {
            isInOriginalFlameRange = false;
            lightTorchWords.SetActive(false);
            Debug.Log("text shouldn't show");
        }

        if (other.tag == "Campfire1" && flameLinked1 == true)
        {
            isInCampfireRange1 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;
        }

        if (other.tag == "Campfire1" && flameLinked1 == false)
        {
            isInCampfireRange1 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
           
        }

        if (other.tag == "Campfire2" && flameLinked2 == true)
        {
            isInCampfireRange2 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;
        }

        if (other.tag == "Campfire2" && flameLinked2 == false)
        {
            isInCampfireRange2 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
        }

        if (other.tag == "Campfire3" && flameLinked3 == true)
        {
            isInCampfireRange3 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;
        }

        if (other.tag == "Campfire3" && flameLinked3 == false)
        {
            isInCampfireRange3 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
           
        }

        if (other.tag == "Campfire4" && flameLinked4 == true)
        {
            isInCampfireRange4 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;
        }

        if (other.tag == "Campfire4" && flameLinked4 == false)
        {
            isInCampfireRange4 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
            
        }

        if (other.tag == "Campfire5" && flameLinked5 == true)
        {
            isInCampfireRange5 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;
        }

        if (other.tag == "Campfire5" && flameLinked5 == false)
        {
            isInCampfireRange5 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");
            
        }

        if (other.tag == "FuelTrigger" && isInFuelRange == true)
        {
            pickUpFuelText.SetActive(false) ;
            isInFuelRange = false ;
            Debug.Log("fuel text should go away");
        }

        if (other.tag == "FuelTrigger1" && isInFuelRange1 == true)
        {
            pickUpFuelText.SetActive(false);
            isInFuelRange1 = false;
            Debug.Log("fuel text should go away");
        }

        if (other.tag == "FuelTrigger2" && isInFuelRange2 == true)
        {
            pickUpFuelText.SetActive(false);
            isInFuelRange2 = false;
            Debug.Log("fuel text should go away");
        }

        if (other.tag == "FuelTrigger3" && isInFuelRange3 == true)
        {
            pickUpFuelText.SetActive(false);
            isInFuelRange3 = false;
            Debug.Log("fuel text should go away");
        }

        if (other.tag == "GreenGateTrigger" && isInGreenGateRange == true)
        {
            openGateText.SetActive(false);
            isInGreenGateRange = false ;
            noKeyText.SetActive(false) ;
            Debug.Log("gate text should go away");

        }

        if (other.tag == "GreenKeyTrigger" && isInGreenKeyRange == true)
        {
            pickupKeyText.SetActive(false);
            isInGreenKeyRange = false;
            Debug.Log("key text should go away");

        }

        if (other.tag == "BlueGateTrigger" && isInBlueGateRange == true)
        {
            openGateText.SetActive(false);
            isInBlueGateRange = false;
            noKeyText.SetActive(false);
            Debug.Log("gate text should go away");

        }

        if (other.tag == "BlueKeyTrigger" && isInBlueKeyRange == true)
        {
            pickupKeyText.SetActive(false);
            isInBlueKeyRange = false;
            Debug.Log("key text should go away");

        }

        if (other.tag == "RedGateTrigger" && isInRedGateRange == true)
        {
            openGateText.SetActive(false);
            isInRedGateRange = false;
            noKeyText.SetActive(false);
            Debug.Log("gate text should go away");

        }

        if (other.tag == "RedKeyTrigger" && isInRedKeyRange == true)
        {
            pickupKeyText.SetActive(false);
            isInRedKeyRange = false;
            Debug.Log("key text should go away");

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

    private IEnumerator NeedKey()
    {
        yield return new WaitForSeconds(0f);
        noKeyText.SetActive(true) ;
        yield return new WaitForSeconds(1.5f);
        noKeyText.SetActive(false);
    }
}
