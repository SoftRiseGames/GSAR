using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_Movement : MonoBehaviour
{
    // HAREKET DE���KENLER�
    private GesarInput gesarInputMovement;                      // Input Systemin olu�turdu�u classtan bir nesne 
    private Vector2 inputMovement;                              // InputSystemden al�nacak vector2 de�i�keni atayaca��m�z de�i�ken
    private float horizontalInput, verticalInput;               // Vector2 de�i�kenin x ve y de�erlerini atayaca��m�z Yatay ve dikey girdileri
    [SerializeField] private float moveSpeed, rotationSpeed;    // Hareket h�z� ve d�n�� h�z�
    private Rigidbody rb;                                       // Karakterin Rigidbody bile�eni

    // AN�MASYON DE���KENLER�
    private Animator animatorMovement;                          // Karakter i�in bir Animator bile�eni
    private float animationSpeed;

    [SerializeField] private bool isShootingArrow;

    //----------------------------UNITY MONOBEHAVIOR FONKS�YONLARI-------------------------------------------//
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody bile�enini al
        animatorMovement = GetComponent<Animator>(); // Animator bile�enini al
    }
    private void Awake()
    {
        MoveInputActionsControl(); // Input System kontrollerini ba�lat
    }
    private void OnEnable()
    {
        gesarInputMovement.Gameplay.Enable(); // Gameplay action map'ini etkinle�tir
    }

    private void OnDisable()
    {
        gesarInputMovement.Gameplay.Disable(); // Gameplay action map'ini devre d��� b�rak
    }
    private void Update()
    {
        if (!isShootingArrow)
        {
            Movement();   // Hareket fonksiyonunu �a��r
        }
        Rotate();                    // Rotasyon fonksiyonunu �a��r
        MovementAnimations();        // Animasyon fonksiyonunu �a��r
    }

    //----------------------------AN�MASYON GEC�KME FONKS�YONLARI-------------------------------------------//
    private void MovementAnimations()
    {
        // Speed parametresine de�eri atayarak blend tree ge�i�ini kontrol et
        animatorMovement.SetFloat("Speed", animationSpeed);
    }

    //---------------------------- MEKAN�K FONKS�YONLAR-------------------------------------------//
    private void Movement()
    {
        horizontalInput = inputMovement.x;        // Yatay de�ere vector2 x bile�enini ata
        verticalInput = inputMovement.y;          // Dikey de�ere vector2 y bile�enini ata
        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Hareket vekt�r� ile karakteri ileri, geri, sola veya sa�a hareket ettir
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }

        // Hareket vekt�r�n�n b�y�kl���n� kullanarak h�z� hesapla (x ve z bile�enleri)
        animationSpeed = new Vector2(horizontalInput, verticalInput).magnitude;

        
    } 
    private void Rotate()
    {
        horizontalInput = inputMovement.x;        // Yatay de�ere vector2 x bile�enini ata
        verticalInput = inputMovement.y;          // Dikey de�ere vector2 y bile�enini ata
        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Karakterin yeni d�n�� rotasyonunu hesapla ve bu rotasyona d�nd�r
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public void SetShootingArrow(bool isShooting)
    {
        isShootingArrow = isShooting;           // Ok atma durumunu kontrol eden fonksiyon
    }
    //----------------------------INPUT SYSTEM FONKS�YONLAR-------------------------------------------//
    private void MoveInputActionsControl()
    {
        gesarInputMovement = new GesarInput(); // GesarInput nesnesini olu�tur

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
