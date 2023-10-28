using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_Movement : MonoBehaviour
{
    // HAREKET DEÐÝÞKENLERÝ
    private GesarInput gesarInputMovement;                      // Input Systemin oluþturduðu classtan bir nesne 
    private Vector2 inputMovement,inputRotate;                  // InputSystemden alýnacak vector2 deðiþkeni atayacaðýmýz deðiþken
    private float horizontalInput, verticalInput;               // Vector2 deðiþkenin x ve y deðerlerini atayacaðýmýz Yatay ve dikey girdileri
    [SerializeField] private float moveSpeed, rotationSpeed;    // Hareket hýzý ve dönüþ hýzý
    private Rigidbody rb;                                       // Karakterin Rigidbody bileþeni

    Geser_StateSystem stateSystem;

    // ANÝMASYON DEÐÝÞKENLERÝ
    private Animator animatorMovement;                          // Karakter için bir Animator bileþeni
    private float animationSpeed;

    

    //----------------------------UNITY MONOBEHAVIOR FONKSÝYONLARI-------------------------------------------//
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody bileþenini al
        animatorMovement = GetComponent<Animator>(); // Animator bileþenini al
    }
    private void Awake()
    {
        MoveInputActionsControl(); // Input System kontrollerini baþlat
        stateSystem= GetComponent<Geser_StateSystem>();
    }
    private void OnEnable()
    {
        gesarInputMovement.Gameplay.Enable(); // Gameplay action map'ini etkinleþtir
    }

    private void OnDisable()
    {
        gesarInputMovement.Gameplay.Disable(); // Gameplay action map'ini devre dýþý býrak
    }
    private void Update()
    {
        Movement();   // Hareket fonksiyonunu çaðýr
        Rotate();                    // Rotasyon fonksiyonunu çaðýr
        MovementAnimations();        // Animasyon fonksiyonunu çaðýr

        
    }

    //----------------------------ANÝMASYON GECÝKME FONKSÝYONLARI-------------------------------------------//
    private void MovementAnimations()
    {
        
        if (stateSystem.currentState == Geser_StateSystem.AnimState.SwordWalk)
        {
            // Speed parametresine deðeri atayarak blend tree geçiþini kontrol et
            animatorMovement.SetFloat("Speed", animationSpeed);
        }
        else if (stateSystem.currentState == Geser_StateSystem.AnimState.SwordReady)
        {
            
            animatorMovement.SetFloat("Speed", 0);
        }
        
    }

    //---------------------------- MEKANÝK FONKSÝYONLAR-------------------------------------------//
    private void Movement()
    {
        if(stateSystem.currentState == Geser_StateSystem.AnimState.SwordWalk)
        {
            horizontalInput = inputMovement.x;        // Yatay deðere vector2 x bileþenini ata
            verticalInput = inputMovement.y;          // Dikey deðere vector2 y bileþenini ata

            if (horizontalInput != 0 || verticalInput != 0)
            {
                // Hareket vektörü ile karakteri ileri, geri, sola veya saða hareket ettir
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + movement);
            }

            // Hareket vektörünün büyüklüðünü kullanarak hýzý hesapla (x ve z bileþenleri)
            animationSpeed = new Vector2(horizontalInput, verticalInput).magnitude;
        }
            
    } 
    private void Rotate()
    {
        horizontalInput = inputMovement.x;        // Yatay deðere vector2 x bileþenini ata
        verticalInput = inputMovement.y;          // Dikey deðere vector2 y bileþenini ata
        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Karakterin yeni dönüþ rotasyonunu hesapla ve bu rotasyona döndür
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    
    //----------------------------INPUT SYSTEM FONKSÝYONLAR-------------------------------------------//
    private void MoveInputActionsControl()
    {
        gesarInputMovement = new GesarInput(); // GesarInput nesnesini oluþtur

        // Hareket girdilerini kontrol et
        gesarInputMovement.Gameplay.Movement.started += InputMove;
        gesarInputMovement.Gameplay.Movement.performed += InputMove;
        gesarInputMovement.Gameplay.Movement.canceled += DeInputMove;

    }

    private void InputMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>(); // Hareket girdilerini al
        stateSystem.currentState = Geser_StateSystem.AnimState.SwordWalk;

    }
    private void DeInputMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        stateSystem.currentState = Geser_StateSystem.AnimState.SwordReady;
    }
    
}
