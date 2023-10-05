using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GESAR : MonoBehaviour
{
    //MOVEMENT VARIABLES
    private GesarInput gesarInput;
    private Vector2 inputMovement;
    private float horizontalInput, verticalInput;    
    public float moveSpeed,rotationSpeed;
    private Rigidbody rb;

    //ANIMATION VARIABLES
    private Animator animator;   

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();
    }
    private void Awake()
    {
        gesarInput = new GesarInput();

        gesarInput.Gameplay.Movement.started += InputMove;
        gesarInput.Gameplay.Movement.performed += InputMove;
        gesarInput.Gameplay.Movement.canceled += InputMove;



    }
    private void Update()
    {
        Movement();
    }
    private void OnEnable()
    {
        gesarInput.Gameplay.Enable();
    }
    private void OnDisable()
    {
        gesarInput.Gameplay.Disable();
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        
    }
    private void Movement()
    {
        horizontalInput = inputMovement.x;
        verticalInput = inputMovement.y;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Hareket vektörü ile karakteri ileri, geri, sola veya saða hareket ettirme.
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            
            // Karakterin yeni dönüþ rotasyonunu hesaplar ve bu rotasyona döndürür.
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("Run", true);

        }
        else
        {
            animator.SetBool("Run", false);
        }
            
    }
}
