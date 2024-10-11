using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.XR;

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
    public Vector3 slopeNormal;
                                   // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component
    private bool onSteepSlope = false;

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private Ingredient.Tool heldTool=Ingredient.Tool.None; // Reference to the currently held object
    private int toolIndex=0;
    public List<Ingredient.Tool> ownedTools;
    private float scrollInput;
    public float pickUpRange = 3f; // Range within which objects can be picked up
    public bool holdingOscie = false;
    public InventoryManager inventory;
    public static event Action<string> pickedUp;

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

    public GameObject interactUI;
    public TextMeshProUGUI interactToolText, interactObjectText;
    public GameObject dialogUI;
    public GameObject itemHolder;

    [Header("TUTORIAL SETTINGS")]
    [Space(5)]
    public List<string> completedTasks;
    public List<string> uncompletedTasks;

    public static FirstPersonControls instance;
    Controls playerInput;

    private void Awake()
    {
        inventory = FindAnyObjectByType<InventoryManager>();
        characterController = GetComponent<CharacterController>();
        
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        playerInput = new Controls();

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


        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        //Subscribe to the pick-up input event
        playerInput.Player.ToggleInventory.performed += ctx => ToggleInventory(); // Call the PickUpObject method when pick-up input is performed

        // Subscribe to the SwitchTool input events
        playerInput.Player.SwitchTool.performed += ctx => scrollInput = ctx.ReadValue<float>(); // Update moveInput when movement input is performed
        playerInput.Player.SwitchTool.performed += ctx => SwitchTool();

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => Crouch(); // Call the PickUpObject method when pick-up input is performed

        playerInput.Player.Taste.performed += ctx => Taste();

    }

    

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        hoverItem();

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

    public void hoverItem()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            interactUI.SetActive(true);
            if (hit.collider.CompareTag("Interactable"))
            {
                interactObjectText.text = hit.collider.name;
                if (hit.collider.GetComponent<PickUp>() != null && holdingOscie)
                {
                    
                    if ((hit.collider.GetComponent<PickUp>().ingredient.toolNeeded) == heldTool)
                    {
                        interactToolText.text = ((Ingredient.Harvest)(hit.collider.GetComponent<PickUp>().ingredient.toolNeeded)).ToString();
                    }
                    else
                    {
                        interactToolText.text = "Incorrect tool";
                    }
                }
                else if (hit.collider.GetComponent<Tool>() && holdingOscie)
                {
                    interactToolText.text = "Take";
                    
                }
                else if (hit.collider.GetComponent<Oscie>())
                {
                    interactToolText.text = "Interact";
                    
                }
                else if (hit.collider.GetComponent<Obstacle>())
                {
                    interactToolText.text = ((Ingredient.Harvest)(hit.collider.GetComponent<Obstacle>().toolneeded)).ToString();

                }else if (hit.collider.GetComponent<Pot>())
                {
                    interactToolText.text = "Cook";
                }else if (hit.collider.GetComponent<Door>())
                {
                    interactToolText.text = "Enter";
                }
            }
            else
            {
                interactUI.SetActive(false);
            }

        }
        else
        {
            interactUI.SetActive(false);
        }
    }

    public void Jump()
    {
        CheckSteepSlope();
        if (characterController.isGrounded && !onSteepSlope)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else if(characterController.isGrounded && onSteepSlope)
        {
            
            //velocity = 5*Vector3.down;
        }
    }
   /* public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CheckSteepSlope();
        if(hit.gameObject.CompareTag("Ground") && onSteepSlope)
        {
            //characterController.Move(20 * new Vector3(hit.normal.x,-hit.normal.y,hit.normal.z) * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, transform.position + hit.normal*3, 0.5f);
        } 
    }*/
    private void CheckSteepSlope()
    {
        Vector3 rayOrigin = transform.position + Vector3.down * 1f;
        float rayLength = characterController.height / 2 + 0.5f; // Adjust the length to ensure it reaches the ground

        // Use raycast to determine slope angle beneath the player
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength))
        {
            slopeNormal = hit.normal;
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            Debug.Log(angle);
            onSteepSlope = angle >= GetComponent<CharacterController>().slopeLimit;
        }
        else
        {
            onSteepSlope = false;
        }
    }

    

    public void PickUpObject()
    {
      
        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.GetComponent<PickUp>() != null)
            {
                Ingredient hitIngredient = hit.collider.GetComponent<PickUp>().ingredient;

                if (hitIngredient.toolNeeded == heldTool && holdingOscie)
                {

                    //TODO: take hitIngredient template and create a new invItem with the same template in inventory
                    inventory.AddInventory(hitIngredient);
                    Destroy(hit.collider.gameObject);

                }


            }
            else if (hit.collider.GetComponent<Tool>() != null && holdingOscie)
            {
                ownedTools.Add(hit.collider.GetComponent<Tool>().tool);
                Destroy(hit.collider.gameObject);
                if (uncompletedTasks.Contains("PickupToolFirstTime"))
                {
                    string task = "PickupToolFirstTime";
                    completedTasks.Add(task);
                    uncompletedTasks.Remove(task);
                    pickedUp(task);
                }else if (uncompletedTasks.Contains("PickupToolLastTime") && completedTasks.Contains("PickupToolFirstTime"))
                {
                    string task = "PickupToolLastTime";
                    completedTasks.Add(task);
                    uncompletedTasks.Remove(task);
                    pickedUp(task);
                }

            }
            else if (hit.collider.GetComponent<Oscie>())
            {
                if (!dialogUI.GetComponent<Dialogue>().isTalking)
                {
                    pickedUp("PickupOscie");
                    StartCoroutine(waitForDialogue(hit));
                }
                
                

            }
            else if (hit.collider.GetComponent<Obstacle>())
            {
                if(heldTool==hit.collider.GetComponent<Obstacle>().toolneeded) hit.collider.GetComponent<Obstacle>().PlayDeath();
              
            }else if (hit.collider.GetComponent<Door>())
            {
                hit.collider.GetComponent<Door>().changeScene();
            }else if (hit.collider.GetComponent<Pot>())
            {
                hit.collider.GetComponent<Pot>().cookbookUI = GameObject.Find("Cookbook");
                hit.collider.GetComponent<Pot>().ToggleCookbook();
            }
           
        }
    }

    IEnumerator waitForDialogue(RaycastHit hit)
    {
        playerInput.Player.Movement.Disable();
        playerInput.Player.Jump.Disable();
        while (FindAnyObjectByType<Dialogue>().isTalking)
        {
            yield return null;
        }
        playerInput.Player.Movement.Enable();
        playerInput.Player.Jump.Enable();
        hit.collider.transform.position = holdPosition.position;
        hit.collider.transform.rotation = holdPosition.rotation;
        hit.collider.transform.Rotate(new Vector3(0, 180, 0));
        hit.collider.transform.parent = holdPosition;
        holdingOscie = true;
        ownedTools.Add(Ingredient.Tool.None);
    }

    public void SwitchTool()
    {
        if (holdingOscie)
        {
            toolIndex += (int)scrollInput;


            if (toolIndex < 0)
            {
                toolIndex = 0;
            }
            else if (toolIndex > ownedTools.Count - 1)
            {
                toolIndex = ownedTools.Count - 1;
            }
            
            heldTool = ownedTools[toolIndex];
            holdPosition.GetChild(0).GetComponent<Oscie>().showTool((int)heldTool);
          
        }
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!holdingOscie)
        {
            dialogUI.GetComponent<Dialogue>().startDialogue();
           
        }
        Destroy(hit.gameObject);
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
        if (cookbookUI.transform.localScale == Vector3.zero)
        {
            
            if (inventoryUI.transform.localScale == Vector3.one)
            {
                inventoryUI.transform.localScale = Vector3.zero;
                inventory.isOpen = false;
                if (itemHolder.transform.childCount > 0)
                {
                    Ingredient held = itemHolder.transform.GetChild(0).GetComponent<InvItem>().ingredient;
                    inventory.GetComponent<InventoryManager>().AddInventory(held);
                    Destroy(itemHolder.transform.GetChild(0).gameObject);
                }
            }
            else if (inventoryUI.transform.localScale == Vector3.zero)
            {
                inventoryUI.transform.localScale = Vector3.one;
                inventory.isOpen = true;
                if (holdingOscie && uncompletedTasks.Contains("OpenNotebookFirstTime"))
                {
                    completedTasks.Add("OpenNotebookFirstTime");
                    uncompletedTasks.Remove("OpenNotebookFirstTime");
                    pickedUp("OpenNotebookFirstTime");
                }else if(holdingOscie && uncompletedTasks.Contains("OpenNotebookFirstIngredient") && inventory.firstIn)
                {
                    completedTasks.Add("OpenNotebookFirstIngredient");
                    uncompletedTasks.Remove("OpenNotebookFirstIngredient");
                    pickedUp("OpenNotebookFirstIngredient");
                }
            }
        }
        else
        {
            cookbookUI.transform.localScale=Vector3.zero;
            cookbookUI.GetComponent<CookBookManager>().isOpen=false;
            inventoryUI.transform.localScale = Vector3.zero;
            inventory.isOpen = false;
        }
    }


    private void Taste()
    {
        if (itemHolder.GetComponentInChildren<InvItem>())
        {
            
            Ingredient tasteIng = itemHolder.GetComponentInChildren<InvItem>().ingredient;
            if (tasteIng.unrevealedFlavours.Count != 0)
            {
                tasteIng.tasteIngredient();
                itemHolder.GetComponentInChildren<InvItem>().displayInfo();
                itemHolder.GetComponentInChildren<InvItem>().RemoveItem();
            }
        }
    }


    public void holdItem()
    {
        if (uncompletedTasks.Contains("HoldItemFirstTime")&& !dialogUI.GetComponent<Dialogue>().isTalking){
            string task = "HoldItemFirstTime";
            completedTasks.Add(task);
            uncompletedTasks.Remove(task);
            pickedUp(task);
        }
    }

}
