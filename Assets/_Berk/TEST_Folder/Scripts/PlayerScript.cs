using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;               // Karakter rigidbody
    [SerializeField] private float moveSpeed = 5f;       // Hareket hýzý
    [SerializeField] private float rotationSpeed = 10f;  // Dönüþ hýzý
    private float horizontalInput, verticalInput;

    [SerializeField] private Animator animator;         // Karakter animator
    private bool hasAnimationInput;
    
    private PlayerInputActions playerInput;

    private enum Behaviors { Waiting, Walking, BowUp, BowDown, Roll, Attack }    // Davranýþlarý temsil eden enum 

    [SerializeField] private Behaviors behaviors = Behaviors.Waiting;        // Karakterin mevcut davranýþýný temsil eden enum deðiþken

    //----------------------------UNITY MONOBEHAVIOR FUNCTIONS-------------------------------------------//
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
    private void Awake()
    {
        InputActionsFunction();
    }
    private void Update()
    {
        Move();
        AnimControl();
    }


    //----------------------------PHYSICS-MECHANICS FUNCTIONS-------------------------------------------//
    private void Move()
    {
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Hareket vektörü ile karakteri ileri, geri, sola veya saða hareket ettirme.
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            Debug.Log("Moving");

            // Karakterin yeni dönüþ rotasyonunu hesaplar ve bu rotasyona döndürür.
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            Debug.Log("Rotating");

            behaviors = Behaviors.Walking;
        }
        else
        {
            Waiting();
        }
        
    }
    private void BowUp()
    {

        behaviors = Behaviors.BowUp;
        hasAnimationInput = true;
        Debug.Log("Yay Gerildi");
    }
    private void BowDown()
    {
        behaviors = Behaviors.BowDown;
        hasAnimationInput = true;
        
        Debug.Log("Yay Býrakýldý");
    }
    private void Roll()
    {
        behaviors = Behaviors.Roll;
    }

    
    
    //----------------------------ANIMATION-DELAY FUNCTIONS-------------------------------------------//
    private void AnimControl()
    {

        switch (behaviors)    // Karakterin davranýþýna göre animasyonlarý kontrol eder.
        {
            case Behaviors.Walking:
                animator.SetBool("Walking", true);       // Walking durumundayken "Walking" animasyonunu çalýþtýrýr.
                animator.SetBool("Waiting", false);
                animator.SetBool("BowUp", false);
                animator.SetBool("BowDown", false);
                animator.SetBool("Roll", false);
                break;

            case Behaviors.Waiting:
                animator.SetBool("Walking", false);      // Waiting durumundayken "Walking" animasyonunu durdurur.
                animator.SetBool("Waiting", true);
                animator.SetBool("BowUp", false);
                animator.SetBool("Roll", false);
                animator.SetBool("BowDown", false);
                break;
            case Behaviors.BowUp:
                animator.SetBool("BowUp", true);           // BowUp durumundayken "Bow" animasyonunu çalýþtýrýr.
                animator.SetBool("Waiting", false);
                animator.SetInteger("Attack", 0);
                break;
            case Behaviors.BowDown:
                animator.SetBool("BowDown", true);       // BowDown durumundayken "BowDown" animasyonunu çalýþtýrýr.
                animator.SetBool("BowUp", false);
                break;
            case Behaviors.Roll:
                animator.SetBool("Roll", true);
                animator.SetBool("Waiting", false);
                break;
            
        }

    }
    private void Waiting()
    {
        if (hasAnimationInput == true)
        {
            AnimControl();
        }
        else
        {
            behaviors = Behaviors.Waiting;
            Debug.Log("Bekliyor");
        }
       
        
    }
    private void HasAnimInputDelay()
    {
        hasAnimationInput = false;
    }

    

    //----------------------------INPUT SYSTEM FUNCTIONS-------------------------------------------//
    private void InputActionsFunction()
    {
        playerInput = new PlayerInputActions();
        playerInput.Gameplay.Bow.performed += bowUp => InputBowUp();
        playerInput.Gameplay.Bow.canceled += bowDown => InputBowDown();
        playerInput.Gameplay.Movement.performed += move => InputMove(move);
        playerInput.Gameplay.Movement.canceled += move => InputMove(move);
        playerInput.Gameplay.Roll.started += roll => InputRoll();
        playerInput.Gameplay.Roll.canceled += roll => InputRoll();
    }
    private void InputMove(InputAction.CallbackContext moveContext)
    {
        Vector2 movementVector = moveContext.ReadValue<Vector2>();
        horizontalInput = movementVector.x;
        verticalInput = movementVector.y;
    }
    private void InputBowUp()
    {
        BowUp();
    }
    private void InputBowDown()
    {
        BowDown();
        Invoke("HasAnimInputDelay", 0.5f);
        
    }
    private void InputRoll()
    {
        Roll();
        Invoke("HasAnimInputDelay", 0.5f);
        Debug.Log("Roll");
    }

}
