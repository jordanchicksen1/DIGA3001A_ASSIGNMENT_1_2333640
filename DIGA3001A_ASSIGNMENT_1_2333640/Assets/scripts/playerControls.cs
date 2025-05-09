using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]

    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed = 8f; // Speed at which the player moves
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

    public Rigidbody rb;
   
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
    public bool isInFuelRange4 = false;
    public bool isInFuelRange5 = false;
    public bool isInFuelRange6 = false;
    public fuelManager fuelManager;
    public ParticleSystem usedFuel;

    public GameObject fuel;
    public GameObject fuel1;
    public GameObject fuel2;
    public GameObject fuel3;
    public GameObject fuel4;
    public GameObject fuel5;
    public GameObject fuel6;

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


    //killbox teleporter
    public GameObject killboxTeleporter;

    //telepotation menu
    public GameObject teleportationMenu;
    public GameObject theAbyss; //campfire2
    public GameObject easternCell; //campfire1
    public GameObject northernCell; //campfire3
    public GameObject southernCell; //campfire5

    public GameObject sitText;

    public bool isUsingTeleportationMenu = false;

    //rock door
    public GameObject rockDoor;
    public GameObject doorDestination;
    public float doorSpeed = 5f;

    public Collider rockDoorCollider;

    public GameObject invisibleDoor;

    //sfx
    public AudioSource sfxs;
    public AudioClip startFireSFX;
    public AudioClip pickUpSFX;
    public AudioClip gateOpeningSFX;
    public AudioClip gateLockedSFX;
    public AudioClip teleportSFX;
    public AudioClip rockFallingSFX;
    public AudioClip fuelSFX;

    //location text
    public GameObject originalFlameText;
    public GameObject easternCellText;
    public GameObject southernCellText;
    public GameObject northernCellText;
    public GameObject theAbyssText;

    //pause screen stuff
    public GameObject pauseScreen;
    public bool isPaused = false;

    //dash stuff
    public bool canDash = true;
    public float dashSpeed = 1.0f;

    //enemies
    public bool isBeingChased = false;
    public bool isBeingChased1 = false;
    public bool isBeingChased2 = false;
    public bool isBeingChased3 = false;
    public bool isBeingChased4 = false;
    //public Vector3 enemyForce = new Vector3(50f, 50f, 0);
    public GameObject redBar;

    public GameObject enemy;
    public float enemySpeed = 8f;
    
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    public Transform pointE;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    public AudioSource ghostSFX;
    public AudioSource gettingHitSFX;
    public ParticleSystem hitVFX;
    public bool canBeHit = true;

    public GameObject pauseText;

    public animationControl animationControl;

    //private Animator playerAnim;
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

        //Subscribe to the pause
        playerInput.Player.Pause.performed += ctx => Pause(); // use fuel
    }

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        _characterController = GetComponent<CharacterController>();
       // playerAnim = GetComponent<Animator>();
        
    }

        private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        CheckBeingChased();
    }

    public void CheckBeingChased()
    {
        if(isBeingChased == true)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, this.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is in enemy trigger");
        }

        if(isBeingChased == false)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, pointA.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is not in enemy trigger");
        }

        if (isBeingChased1 == true)
        {
            enemy1.transform.position = Vector3.MoveTowards(enemy1.transform.position, this.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is in enemy trigger");
        }

        if (isBeingChased1 == false)
        {
            enemy1.transform.position = Vector3.MoveTowards(enemy1.transform.position, pointB.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is not in enemy trigger");
        }

        if (isBeingChased2 == true)
        {
            enemy2.transform.position = Vector3.MoveTowards(enemy2.transform.position, this.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is in enemy trigger");
        }

        if (isBeingChased2 == false)
        {
            enemy2.transform.position = Vector3.MoveTowards(enemy2.transform.position, pointC.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is not in enemy trigger");
        }

        if (isBeingChased3 == true)
        {
            enemy3.transform.position = Vector3.MoveTowards(enemy3.transform.position, this.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is in enemy trigger");
        }

        if (isBeingChased3 == false)
        {
            enemy3.transform.position = Vector3.MoveTowards(enemy3.transform.position, pointD.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is not in enemy trigger");
        }

        if (isBeingChased4 == true)
        {
            enemy4.transform.position = Vector3.MoveTowards(enemy4.transform.position, this.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is in enemy trigger");
        }

        if (isBeingChased4 == false)
        {
            enemy4.transform.position = Vector3.MoveTowards(enemy4.transform.position, pointE.transform.position, enemySpeed * Time.deltaTime);
            Debug.Log("is not in enemy trigger");
        }
    }

    private void Move()
    {
        if( isUsingTeleportationMenu == false && isPaused == false)
        {
            // Create a movement vector based on the input
            Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);

            // Transform direction from local to world space
            move = transform.TransformDirection(move);

            var currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

            // Move the character controller based on the movement vector and speed
            _characterController.Move(move * currentSpeed * Time.deltaTime);

            //animationControl.DoWalkAnimation();
        }
       

        if (_moveInput.x >0 ||  _moveInput.y >0 || _moveInput.x < 0 || _moveInput.y < 0)
        {
            animationControl.DoWalkAnimation();
        }

        else
        {
            animationControl.DoIdleAnimation();
        }




    }

    private void LookAround()
    {
       if ( isUsingTeleportationMenu == false && isPaused == false)
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
           
        
    }

    private void Pause()
    {
        if(isPaused == false)
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("should pause");
        }

        else if(isPaused == true)
        {
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            Debug.Log("should unpaused");
        }
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

        if (_characterController.isGrounded && isUsingTeleportationMenu == false && isPaused == false)
        {
            // Calculate the jump velocity
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animationControl.DoJumpAnimation(); 
        }
        
            
       
    }

    private void UseFuel()
    {
        if(fuelManager.fuel > 0.99 && hopeManager.currentHope < 100 && isPaused == false)
        {
            Debug.Log("do the thing");
            fuelManager.subtractFuel();
            hopeManager.UseFuel();
            sfxs.clip = fuelSFX;
            sfxs.Play();
            usedFuel.Play();
        }
    }

    private void LightFire()
    {
       if(isInOriginalFlameRange == true && hasLitTorch == false)
        {
            animationControl.DoLinkFlameAnimation();
            StartCoroutine(OriginalFlameStuff());
        }

       //teleport to the original flame
        if(isInOriginalFlameRange == true && hasLitTorch == true) 
        {
            isUsingTeleportationMenu = true;
            teleportationMenu.SetActive(true);
            hopeManager.RestoreAllHope();

            //sfxs.clip = teleportSFX;
            //sfxs.Play();
        }
        
        if(isInCampfireRange1 == true && flameLinked1 == false && hasLitTorch == true)
        {
            animationControl.DoLinkFlameAnimation();
            StartCoroutine(Campfire1Stuff());
            
        }

        //teleport to eastern cell
        if (isInCampfireRange1 == true && flameLinked1 == true)
        {
            isUsingTeleportationMenu = true;
            teleportationMenu.SetActive(true);
            hopeManager.RestoreAllHope();

            //sfxs.clip = teleportSFX;
           // sfxs.Play();
        }

        if (isInCampfireRange2 == true && flameLinked2 == false && hasLitTorch == true)
        {
            animationControl.DoLinkFlameAnimation();
            StartCoroutine(Campfire2Stuff());
            
        }

        //teleport to the abyss
        if (isInCampfireRange2 == true && flameLinked2 == true)
        {
            isUsingTeleportationMenu = true;
            teleportationMenu.SetActive(true);
            hopeManager.RestoreAllHope();

           // sfxs.clip = teleportSFX;
           // sfxs.Play();
        }

        if (isInCampfireRange3 == true && flameLinked3 == false && hasLitTorch == true)
        {
            animationControl.DoLinkFlameAnimation();
            StartCoroutine(Campfire3Stuff());
            
        }

        //teleport to northen cell
        if (isInCampfireRange3 == true && flameLinked3 == true)
        {
            isUsingTeleportationMenu = true;
            teleportationMenu.SetActive(true);
            hopeManager.RestoreAllHope();

           // sfxs.clip = teleportSFX;
           // sfxs.Play();
        }

        if (isInCampfireRange4 == true && flameLinked4 == false && hasLitTorch == true)
        {
            Debug.Log("should have lit fire");
            fire4.SetActive(true);
            campfireManager.addCampfire();
            linkFlameText.SetActive(false);
            //flameLinked4 = true;
            StartCoroutine(FlameLinked4());
            StartCoroutine(FlameLinkedText());
            increaseHope = true;
            decreaseHope = false;
            animationControl.DoLinkFlameAnimation();
            sfxs.clip = startFireSFX;
            sfxs.Play();
        }

        

        if (isInCampfireRange5 == true && flameLinked5 == false && hasLitTorch == true)
        {
            animationControl.DoLinkFlameAnimation();
            StartCoroutine (Campfire5Stuff());
            

        }

        //teleport to southern cell
        if (isInCampfireRange5 == true && flameLinked5 == true)
        {
            isUsingTeleportationMenu = true;
            teleportationMenu.SetActive(true);
            hopeManager.RestoreAllHope();

            //sfxs.clip = teleportSFX;
            // sfxs.Play();
        }

        if (isInFuelRange == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInFuelRange1 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel1);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange1 = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInFuelRange2 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel2);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange2 = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInFuelRange3 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel3);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange3 = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInFuelRange4 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel4);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange4 = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInFuelRange5 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel5);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange5 = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInFuelRange6 == true)
        {
            Debug.Log("pick up fuel");
            Destroy(fuel6);
            fuelManager.addFuel();
            pickUpFuelText.SetActive(false);
            isInFuelRange6 = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInGreenGateRange == true && hasGreenKey == true)
        {
            Debug.Log("open gate");
            Destroy(greenGate);
            isInGreenGateRange = false;
            openGateText.SetActive(false);

            sfxs.clip = gateOpeningSFX;
            sfxs.Play();
        }

        if(isInGreenKeyRange == true)
        {
            Debug.Log("pick up key");
            hasGreenKey = true;
            Destroy(greenKey);
            pickupKeyText.SetActive(false);
            greenKeyPic.SetActive(true );
            isInGreenKeyRange = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInGreenGateRange == true && hasGreenKey == false)
        {
            Debug.Log("don't have key");
            isInGreenGateRange = false;
            openGateText.SetActive(false);
            StartCoroutine(NeedKey());

            sfxs.clip = gateLockedSFX;
            sfxs.Play();
        }

        if (isInBlueGateRange == true && hasBlueKey == true)
        {
            Debug.Log("open gate");
            Destroy(blueGate);
            isInBlueGateRange = false;
            openGateText.SetActive(false);

            sfxs.clip = gateOpeningSFX;
            sfxs.Play();
        }

        if (isInBlueKeyRange == true)
        {
            Debug.Log("pick up key");
            hasBlueKey = true;
            Destroy(blueKey);
            pickupKeyText.SetActive(false);
            blueKeyPic.SetActive(true);
            isInBlueKeyRange = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();
        }

        if (isInBlueGateRange == true && hasBlueKey == false)
        {
            Debug.Log("don't have key");
            isInBlueGateRange = false;
            openGateText.SetActive(false);
            StartCoroutine(NeedKey());

            sfxs.clip = gateLockedSFX;
            sfxs.Play();
        }

        if (isInRedGateRange == true && hasRedKey == true)
        {
            Debug.Log("open gate");
            Destroy(redGate);
            isInRedGateRange = false;
            openGateText.SetActive(false);

            sfxs.clip = gateOpeningSFX;
            sfxs.Play();
        }

        if (isInRedKeyRange == true)
        {
            Debug.Log("pick up key");
            hasRedKey = true;
            Destroy(redKey);
            pickupKeyText.SetActive(false);
            redKeyPic.SetActive(true);
            isInRedKeyRange = false;

            sfxs.clip = pickUpSFX;
            sfxs.Play();  
        }

        if (isInRedGateRange == true && hasRedKey == false)
        {
            Debug.Log("don't have key");
            isInRedGateRange = false;
            openGateText.SetActive(false);
            StartCoroutine(NeedKey());

            sfxs.clip = gateLockedSFX;
            sfxs.Play();
        }
    }

    private void Sprint()
    {
        if(canDash == true)
        {
            Debug.Log("should sprint");
            StartCoroutine(TheDash());
        }
       

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Campfire1" && flameLinked1 == false && hasLitTorch == true)
        {
            isInCampfireRange1 = true;
            linkFlameText.SetActive(true);
            
            easternCellText.SetActive(true);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            originalFlameText.SetActive(false);
        }

        if(other.tag == "Campfire1" && flameLinked1 == true)
        {
            increaseHope = true;
            decreaseHope = false;
            isInCampfireRange1 = true;
            sitText.SetActive(true);

            easternCellText.SetActive(true);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            originalFlameText.SetActive(false);
        }
        

        if (other.tag == "OriginalFlame" && hasLitTorch == false)
        {
            
            isInOriginalFlameRange = true;
            lightTorchWords.SetActive(true);
            Debug.Log("hello");
            
            originalFlameText.SetActive(true);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "OriginalFlame" && hasLitTorch == true)
        {
            increaseHope = true;
            decreaseHope = false;
            isInOriginalFlameRange = true;
            sitText.SetActive(true);

            originalFlameText.SetActive(true);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }


        if (other.tag == "Campfire2" && flameLinked2 == false && hasLitTorch == true)
        {
            isInCampfireRange2 = true;
            linkFlameText.SetActive(true);

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(true);
            easternCellText.SetActive(false);


        }
        
        if (other.tag == "Campfire2" && flameLinked2 == true)
        {
            increaseHope = true;
            decreaseHope = false;
            isInCampfireRange2 = true;
            sitText.SetActive(true);

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(true);
            easternCellText.SetActive(false);

        }


        if (other.tag == "Campfire3" && flameLinked3 == false && hasLitTorch == true)
        {
            isInCampfireRange3 = true;
            linkFlameText.SetActive(true);

            originalFlameText.SetActive(false);
            northernCellText.SetActive(true);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire3" && flameLinked3 == true)
        {
            increaseHope = true;
            decreaseHope = false;
            isInCampfireRange3 = true;
            sitText.SetActive(true);

            originalFlameText.SetActive(false);
            northernCellText.SetActive(true);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
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
            isInCampfireRange4 = true;
            sitText.SetActive(true);
        }

        if (other.tag == "Campfire5" && flameLinked5 == false && hasLitTorch == true)
        {
            isInCampfireRange5 = true;
            linkFlameText.SetActive(true);

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(true);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);

        }

        if (other.tag == "Campfire5" && flameLinked5 == true)
        {
            increaseHope = true;
            decreaseHope = false;
            isInCampfireRange5 = true;
            sitText.SetActive(true);

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(true);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if(other.tag == "FuelTrigger" && isInFuelRange == false)
        {
            isInFuelRange = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger1" && isInFuelRange1 == false)
        {
            isInFuelRange1 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger2" && isInFuelRange2 == false)
        {
            isInFuelRange2 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger3" && isInFuelRange3 == false)
        {
            isInFuelRange3 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger4" && isInFuelRange4 == false)
        {
            isInFuelRange4 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger4" && isInFuelRange4 == false)
        {
            isInFuelRange4 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger5" && isInFuelRange5 == false)
        {
            isInFuelRange5 = true;
            pickUpFuelText.SetActive(true);
            Debug.Log("show fuel text");
        }

        if (other.tag == "FuelTrigger6" && isInFuelRange6 == false)
        {
            isInFuelRange6 = true;
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
            StartCoroutine(Killbox());
            Debug.Log("should've teleported player");
        }

        if(other.tag == "DoorTrigger" && hasLitTorch == true)
        {
            rockDoorCollider.enabled = false;
            Destroy(rockDoor, 20f);
            Destroy(invisibleDoor, 3.3f);
            sfxs.clip = rockFallingSFX;
            sfxs.Play();
        }

        if(other.tag == "Enemy" && canBeHit == true)
        {
            hopeManager.EnemyAttack();
            StartCoroutine(GettingHit());
            StartCoroutine(IsBeingHit());
            hitVFX.Play();
            gettingHitSFX.Play();
        }

        if (other.tag == "EnemyTrigger")
        {

            ghostSFX.Play();
        }

        if (other.tag == "EnemyTrigger1")
        {

            ghostSFX.Play();
        }

        if (other.tag == "EnemyTrigger2")
        {

            ghostSFX.Play();
        }

        if (other.tag == "EnemyTrigger3")
        {

            ghostSFX.Play();
        }

        if (other.tag == "EnemyTrigger4")
        {

            ghostSFX.Play();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "OriginalFlame" && hasLitTorch == true)
        {
            isInOriginalFlameRange = false;
            lightTorchWords.SetActive(false);
            sitText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "OriginalFlame" && hasLitTorch == false)
        {
            isInOriginalFlameRange = false;
            lightTorchWords.SetActive(false);
            Debug.Log("text shouldn't show");

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire1" && flameLinked1 == true)
        {
            isInCampfireRange1 = false;
            linkFlameText.SetActive(false);
            sitText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire1" && flameLinked1 == false)
        {
            isInCampfireRange1 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);

        }

        if (other.tag == "Campfire2" && flameLinked2 == true)
        {
            isInCampfireRange2 = false;
            linkFlameText.SetActive(false);
            sitText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire2" && flameLinked2 == false)
        {
            isInCampfireRange2 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire3" && flameLinked3 == true)
        {
            isInCampfireRange3 = false;
            linkFlameText.SetActive(false);
            sitText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire3" && flameLinked3 == false)
        {
            isInCampfireRange3 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire4" && flameLinked4 == true)
        {
            isInCampfireRange4 = false;
            linkFlameText.SetActive(false);
            sitText.SetActive(false);
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
            sitText.SetActive(false);
            Debug.Log("text shouldn't show");
            decreaseHope = true;
            increaseHope = false;

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);
        }

        if (other.tag == "Campfire5" && flameLinked5 == false)
        {
            isInCampfireRange5 = false;
            linkFlameText.SetActive(false);
            Debug.Log("text shouldn't show");

            originalFlameText.SetActive(false);
            northernCellText.SetActive(false);
            southernCellText.SetActive(false);
            theAbyssText.SetActive(false);
            easternCellText.SetActive(false);

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

        if (other.tag == "FuelTrigger4" && isInFuelRange4 == true)
        {
            pickUpFuelText.SetActive(false);
            isInFuelRange4  = false;
            Debug.Log("fuel text should go away");
        }

        if (other.tag == "FuelTrigger5" && isInFuelRange5 == true)
        {
            pickUpFuelText.SetActive(false);
            isInFuelRange5 = false;
            Debug.Log("fuel text should go away");
        }

        if (other.tag == "FuelTrigger6" && isInFuelRange6 == true)
        {
            pickUpFuelText.SetActive(false);
            isInFuelRange6 = false;
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

        if (other.tag == "EnemyTrigger")
        {
            
            isBeingChased = false;
        }

        if (other.tag == "EnemyTrigger1")
        {

            isBeingChased1 = false;
        }

        if (other.tag == "EnemyTrigger2")
        {

            isBeingChased2 = false;
        }

        if (other.tag == "EnemyTrigger3")
        {

            isBeingChased3 = false;
        }

        if (other.tag == "EnemyTrigger4")
        {

            isBeingChased4 = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyTrigger")
        {
            
            isBeingChased = true;
            
        }

        if (other.tag == "EnemyTrigger1")
        {

            isBeingChased1 = true;

        }

        if (other.tag == "EnemyTrigger2")
        {

            isBeingChased2 = true;

        }

        if (other.tag == "EnemyTrigger3")
        {

            isBeingChased3 = true;

        }

        if (other.tag == "EnemyTrigger4")
        {

            isBeingChased4 = true;

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

    private IEnumerator Killbox()
    {
        yield return new WaitForSeconds(0f);
        this.transform.position = new Vector3(-58.8f, 12.1f, -147.2f);
        _characterController.enabled = false;
        yield return new WaitForSeconds(0.01f);
        _characterController.enabled = true;
    }

    private IEnumerator HasLitTorchBool()
    {
        yield return new WaitForSeconds(0.5f);
        hasLitTorch = true;
        Debug.Log("it should've turned the bool on");
    }

    private IEnumerator FlameLinked1()
    {
        yield return new WaitForSeconds(0.5f);
        flameLinked1 = true;
        Debug.Log("it should've turned the bool on");
    }

    private IEnumerator FlameLinked2()
    {
        yield return new WaitForSeconds(0.5f);
        flameLinked2 = true;
        Debug.Log("it should've turned the bool on");
    }

    private IEnumerator FlameLinked3()
    {
        yield return new WaitForSeconds(0.5f);
        flameLinked3 = true;
        Debug.Log("it should've turned the bool on");
    }
    private IEnumerator FlameLinked4()
    {
        yield return new WaitForSeconds(0.5f);
        flameLinked4 = true;
        Debug.Log("it should've turned the bool on");
    }

    private IEnumerator FlameLinked5()
    {
        yield return new WaitForSeconds(0.5f);
        flameLinked5 = true;
        Debug.Log("it should've turned the bool on");
    }

    private IEnumerator TheDash()
    {
        yield return new WaitForSeconds(0f);
        canDash = false;
        moveSpeed = moveSpeed + 8f;
        yield return new WaitForSeconds(0.3f);
        moveSpeed = moveSpeed - 8f;
        yield return new WaitForSeconds(2f);
        canDash = true;
    }

    private IEnumerator GettingHit()
    {
        yield return new WaitForSeconds(0f);
        redBar.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        redBar.SetActive(false);
    }

    private IEnumerator IsBeingHit()
    {
        yield return new WaitForSeconds(0f);
        canBeHit = false;
        yield return new WaitForSeconds(1.5f);
        canBeHit = true;
    }

    private IEnumerator PressPause()
    {
        yield return new WaitForSeconds(0f);
        pauseText.SetActive(true);
        yield return new WaitForSeconds(4f);
        pauseText.SetActive(false);
    }

    public void Start()
    {
        StartCoroutine(PressPause());
    }

    private IEnumerator OriginalFlameStuff()
    {
        yield return new WaitForSeconds(0.5f);
        torchFlame.SetActive(true);
        lightTorchWords.SetActive(false);
        // hasLitTorch = true; 
        StartCoroutine(HasLitTorchBool());
        StartCoroutine(TorchLitText());
        increaseHope = true;
        decreaseHope = false;
        hopeUI.SetActive(true);
        // Destroy(rockDoor);

        sfxs.clip = startFireSFX;
        sfxs.Play();
    }

    private IEnumerator Campfire1Stuff()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("should have lit fire");
        fire1.SetActive(true);
        campfireManager.addCampfire();
        linkFlameText.SetActive(false);
        // flameLinked1 = true;
        StartCoroutine(FlameLinked1());
        StartCoroutine(FlameLinkedText());
        increaseHope = true;
        decreaseHope = false;
        easternCell.SetActive(true);

        sfxs.clip = startFireSFX;
        sfxs.Play();

    }

    private IEnumerator Campfire2Stuff()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("should have lit fire");
        fire2.SetActive(true);
        campfireManager.addCampfire();
        linkFlameText.SetActive(false);
        //flameLinked2 = true;
        StartCoroutine(FlameLinked2());
        StartCoroutine(FlameLinkedText());
        increaseHope = true;
        decreaseHope = false;
        theAbyss.SetActive(true);

        sfxs.clip = startFireSFX;
        sfxs.Play();

    }

    private IEnumerator Campfire3Stuff()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("should have lit fire");
        fire3.SetActive(true);
        campfireManager.addCampfire();
        linkFlameText.SetActive(false);
        //flameLinked3 = true;
        StartCoroutine(FlameLinked3());
        StartCoroutine(FlameLinkedText());
        increaseHope = true;
        decreaseHope = false;
        northernCell.SetActive(true);

        sfxs.clip = startFireSFX;
        sfxs.Play();

    }

    private IEnumerator Campfire5Stuff()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("should have lit fire");
        fire5.SetActive(true);
        campfireManager.addCampfire();
        linkFlameText.SetActive(false);
        //flameLinked5 = true;
        StartCoroutine(FlameLinked5());
        StartCoroutine(FlameLinkedText());
        increaseHope = true;
        decreaseHope = false;
        southernCell.SetActive(true);

        sfxs.clip = startFireSFX;
        sfxs.Play();
    }
}
