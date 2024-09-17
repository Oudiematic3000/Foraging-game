using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class FirstPersonControls : MonoBehaviour
{
   

    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -10.2f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
    public float airSpeed;
                                   // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component
    private bool onSteepSlope = false;


    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
   

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private Ingredient.Tool heldTool=Ingredient.Tool.None; // Reference to the currently held object
    private float scrollInput;
    public float pickUpRange = 3f; // Range within which objects can be picked up
    private bool holdingGun = false;
    public InventoryManager inventory;

    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1;
    public float standHeight;
    public float crouchSpeed = 1.5f;
    public bool isCrouching = false;

    [Header("UI SETTINGS")]
    [Space(5)]
    public TextMeshProUGUI toolUI;
    public GameObject inventoryUI;
    public GameObject cookbookUI;

    



    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        //Subscribe to the pick-up input event
        playerInput.Player.ToggleInventory.performed += ctx => ToggleInventory(); // Call the PickUpObject method when pick-up input is performed

        //Subscribe to the pick-up input event
        playerInput.Player.ToggleCookbook.performed += ctx => ToggleCookbook(); // Call the PickUpObject method when pick-up input is performed


        // Subscribe to the SwitchTool input events
        playerInput.Player.SwitchTool.performed += ctx => scrollInput = ctx.ReadValue<float>(); // Update moveInput when movement input is performed
        playerInput.Player.SwitchTool.performed += ctx => SwitchTool();

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => Crouch(); // Call the PickUpObject method when pick-up input is performed


    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }

    public void Move()
    {
        float currentSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if(!characterController.isGrounded)
        {
            currentSpeed = airSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * currentSpeed * Time.deltaTime);

        
    }

    public void LookAround()
    {
        if (!inventory.isOpen) {
            // Get horizontal and v
            // vertical look inputs and adjust based on sensitivity
            float LookX = lookInput.x * lookSpeed;
            float LookY = lookInput.y * lookSpeed;

            // Horizontal rotation: Rotate the player object around the y-axis
            transform.Rotate(0, LookX, 0);

            // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
            verticalLookRotation -= LookY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

            // Apply the clamped vertical rotation to the player camera
            playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
        }
        
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        CheckSteepSlope();
        if (characterController.isGrounded && !onSteepSlope)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void CheckSteepSlope()
    {
        Vector3 rayOrigin = transform.position + Vector3.down * 1f;
        float rayLength = characterController.height / 2 + 0.5f; // Adjust the length to ensure it reaches the ground

        // Use raycast to determine slope angle beneath the player
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            Debug.Log(angle);
            onSteepSlope = angle >= GetComponent<CharacterController>().slopeLimit;
        }
        else
        {
            onSteepSlope = false;
        }
    }

    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
       /* if (heldTool != null)
        {
            heldTool.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            heldTool.transform.parent = null;
            holdingGun = false;
        }
       */
        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.GetComponent<PickUp>()!=null)
            {
                Ingredient hitIngredient=hit.collider.GetComponent<PickUp>().ingredient;
            
                if (hitIngredient.toolNeeded==heldTool) {

                    //TODO: take hitIngredient template and create a new invItem with the same template in inventory
                    inventory.AddInventory(hitIngredient);
                    Destroy(hit.collider.gameObject);

                }

               
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                
            }
        }
    }

    public void SwitchTool()
    {
        
        heldTool += (int)scrollInput;
        if (heldTool < 0)
        {
            heldTool= 0;
        }
        else if ((int)heldTool > 1)
        {
            heldTool = Ingredient.Tool.Drill;
        }
        toolUI.text = heldTool.ToString();
    }

    public void Crouch()
    {
        if (isCrouching)
        {
           
           characterController.height=standHeight;
            isCrouching = false;
            
        }
        else
        {
            characterController.height = crouchHeight;
            isCrouching = true;
            
           
        }
    }

    public void ToggleInventory()
    {
        if(inventoryUI.transform.localScale == Vector3.one)
        {
            inventoryUI.transform.localScale = Vector3.zero;
            inventory.isOpen=false;
        }
        else if(inventoryUI.transform.localScale == Vector3.zero) {
            inventoryUI.transform.localScale = Vector3.one;
            inventory.isOpen=true;
        }
    }

    public void ToggleCookbook()
    {
        if (cookbookUI.transform.localScale == Vector3.one)
        {
            cookbookUI.transform.localScale = Vector3.zero;
            inventory.isOpen = false;


        }
        else if (cookbookUI.transform.localScale == Vector3.zero)
        {
            cookbookUI.transform.localScale = Vector3.one;
            inventory.isOpen = true;


        }
    }



    // Pick up the object         --Leaving this old code here in case we find a use for it
    /*  heldTool = hit.collider.gameObject;
      heldTool.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

      // Attach the object to the hold position
      heldTool.transform.position = holdPosition.position;
      heldTool.transform.rotation = holdPosition.rotation;
      heldTool.transform.parent = holdPosition;
    */

    // Pick up the object
    /*  heldTool = hit.collider.gameObject;
      heldTool.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

      // Attach the object to the hold position
      heldTool.transform.position = holdPosition.position;
      heldTool.transform.rotation = holdPosition.rotation;
      heldTool.transform.parent = holdPosition;

      holdingGun = true;
    */

}
