using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_Movement : MonoBehaviour
{
    // HAREKET DEÐÝÞKENLERÝ
    private GesarInput gesarInputMovement;                      // Input Systemin oluþturduðu classtan bir nesne 
    private Vector2 inputMovement;                              // InputSystemden alýnacak vector2 deðiþkeni atayacaðýmýz deðiþken
    private float horizontalInput, verticalInput;               // Vector2 deðiþkenin x ve y deðerlerini atayacaðýmýz Yatay ve dikey girdileri
    [SerializeField] private float moveSpeed, rotationSpeed;    // Hareket hýzý ve dönüþ hýzý
    private Rigidbody rb;                                       // Karakterin Rigidbody bileþeni

    // ANÝMASYON DEÐÝÞKENLERÝ
    private Animator animatorMovement;                          // Karakter için bir Animator bileþeni
    private float animationSpeed;

    [SerializeField] private bool isShootingArrow;

    //----------------------------UNITY MONOBEHAVIOR FONKSÝYONLARI-------------------------------------------//
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody bileþenini al
        animatorMovement = GetComponent<Animator>(); // Animator bileþenini al
    }
    private void Awake()
    {
        MoveInputActionsControl(); // Input System kontrollerini baþlat
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
        if (!isShootingArrow)
        {
            Movement();   // Hareket fonksiyonunu çaðýr
        }
        Rotate();                    // Rotasyon fonksiyonunu çaðýr
        MovementAnimations();        // Animasyon fonksiyonunu çaðýr
    }

    //----------------------------ANÝMASYON GECÝKME FONKSÝYONLARI-------------------------------------------//
    private void MovementAnimations()
    {
        // Speed parametresine deðeri atayarak blend tree geçiþini kontrol et
        animatorMovement.SetFloat("Speed", animationSpeed);
    }

    //---------------------------- MEKANÝK FONKSÝYONLAR-------------------------------------------//
    private void Movement()
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
    public void SetShootingArrow(bool isShooting)
    {
        isShootingArrow = isShooting;           // Ok atma durumunu kontrol eden fonksiyon
    }
    //----------------------------INPUT SYSTEM FONKSÝYONLAR-------------------------------------------//
    private void MoveInputActionsControl()
    {
        gesarInputMovement = new GesarInput(); // GesarInput nesnesini oluþtur

        // Hareket girdilerini kontrol et
        gesarInputMovement.Gameplay.Movement.started += InputMove;
        gesarInputMovement.Gameplay.Movement.performed += InputMove;
        gesarInputMovement.Gameplay.Movement.canceled += InputMove;

    }

    private void InputMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>(); // Hareket girdilerini al

    }

}
