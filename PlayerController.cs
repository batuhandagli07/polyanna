using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]    
public class PlayerController : MonoBehaviour
{
    // Character Movement from Documentation
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    // Our Input Actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction sprintAction;

    // Animator
    private Animator animator;

    // Animation on Blendtree
    int jumpAnimation;
    int recoilAnimation;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;

    // Player properties from Documentation
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    // Bullet properties
    [SerializeField] private GameObject bulletPrefab;    // Bullet shape
    [SerializeField] private Transform barrelTransform;  // Gun's forward (Silahýn Ucu)
    [SerializeField] private Transform bulletParent;     // Bullets are getting together in empty object after shooting
    [SerializeField] private float bulletHitMissDistance=25f;  

    // Animation time with transformation
    [SerializeField] private float animationSmoothTime = 0.1f;
    [SerializeField] private float animationPlayTransition = 0.15f;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float aimDistance=1f;

    // Animation necessaries
    public bool sprint = false;
    public bool Shoot=false;
    
    // Player's Audio
    [SerializeField]  AudioSource jumpsource;
    [SerializeField]  AudioClip  jumpclip;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;
    private void Awake()
    {
        // Player Controller's Keys
        playerInput =GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        sprintAction = playerInput.actions["Sprint"];

        // Animation objects in base layer
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Pistol Jump");
        recoilAnimation = Animator.StringToHash("Pistol Shoot Recoil");
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");

    }

    
    private void OnEnable()  // When game starts
    { 
        // Input actions. They are triggered from mouse buttons or keys
        shootAction.performed += _ => ShootGun();  // Shoot action when be performed, call ShootGun function

        sprintAction.performed += _ => StartSprint();
        sprintAction.canceled += _ => StopSprint();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();

        sprintAction.performed -= _ => StartSprint();
        sprintAction.canceled -= _ => StopSprint();
    }

    public void ShootGun()  
    {
        Shoot=true;
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent); // Bullet identify
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
             
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }

        animator.CrossFade(recoilAnimation, animationPlayTransition);  // Call animation without transition
    }

    private void StartSprint()
    {
        
        playerSpeed += 4f;
        animator.SetBool("isSprint", true);
        sprint= true;

    }

    private void StopSprint()
    {
        playerSpeed -= 4f;
        animator.SetBool("isSprint", false);
        sprint= false;
    }


    void Update()
    {

        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance; 

        groundedPlayer = controller.isGrounded;  // We are arranging to our aim according to camera
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input= moveAction.ReadValue<Vector2>();  // Reading data coming from keys(w,a,s,d)
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);  //Above the row, we are getting healing the blendtree
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized; 
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);  // Player's speed


        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);  // We are building the points on blendtree as x1, x2 like that 
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)    // Character jumping at first time until touch to the ground
        {
            
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition); ;
            jumpsource.PlayOneShot(jumpclip);
        }
        
        playerVelocity.y += gravityValue * Time.deltaTime; 
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate towards camera direction with mouse
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}