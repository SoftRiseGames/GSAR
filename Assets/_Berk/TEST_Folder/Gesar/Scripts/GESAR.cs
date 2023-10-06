using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GESAR : MonoBehaviour
{
    // HAREKET DEÐÝÞKENLERÝ
    private GesarInput gesarInput;                              // Input Systemin oluþturduðu classtan bir nesne 
    private Vector2 inputMovement;                              // InputSystemden alýnacak vector2 deðiþkeni atayacaðýmýz deðiþken
    private float horizontalInput, verticalInput;               // vector2 deðiþkenin x ve y deðerlerini atayacaðýmýz Yatay ve dikey girdileri
    [SerializeField] private float moveSpeed, rotationSpeed;    // Hareket hýzý ve dönüþ hýzý
    private Rigidbody rb;                                       // Karakterin Rigidbody bileþeni

    // SALDIRI DEÐÝÞKENLERÝ
    private bool isAttacking;                                   // Saldýrý yapýlýp yapýlmadýðýný belirten bool deðiþken
    private bool isBow;                                         // Yay kullanýlýp kullanýlmadýðýný belirten bool deðiþken
    
    

   // ANÝMASYON DEÐÝÞKENLERÝ
   private Animator animator;                                  // Karakter için bir Animator bileþeni
    private enum Behaviors { Waiting, Running, Attack, Bow }    // Karakter davranýþlarýný için enum

    [SerializeField] private Behaviors behaviors = Behaviors.Waiting; // Baþlangýçta karakterin bekleme durumunda olacaðý davranýþ

    
    //----------------------------UNITY MONOBEHAVIOR FONKSÝYONLARI-------------------------------------------//
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody bileþenini al
        animator = GetComponent<Animator>(); // Animator bileþenini al
    }

    private void Awake()
    {
        InputActionsControl(); // Input System kontrollerini baþlat
    }

    private void Update()
    {
        if (behaviors != Behaviors.Attack || behaviors != Behaviors.Bow)
        {
            Movement(); // Hareket fonksiyonunu çaðýr
        }
        Attack(); // Saldýrý fonksiyonunu çaðýr
        Bow(); // Yay fonksiyonunu çaðýr
        AnimControl(); // Animasyon kontrol fonksiyonunu çaðýr
    }

    private void OnEnable()
    {
        gesarInput.Gameplay.Enable(); // Gameplay action map'ini etkinleþtir
    }

    private void OnDisable()
    {
        gesarInput.Gameplay.Disable(); // Gameplay action map'ini devre dýþý býrak
    }

    //----------------------------ANÝMASYON GECÝKME FONKSÝYONLARI-------------------------------------------//
    private void AnimControl()
    {
        // Karakter davranýþýna göre animasyonlarý kontrol et
        switch (behaviors)
        {
            case Behaviors.Waiting:
                // Bekleme durumunda animasyonlarý kapat
                animator.SetBool("Run", false);
                animator.SetBool("Sword", false);
                animator.SetBool("Bow", false);
                break;
            case Behaviors.Running:
                // Koþma durumunda koþma animasyonunu aç, diðer animasyonlarý kapat
                animator.SetBool("Run", true);
                animator.SetBool("Sword", false);
                animator.SetBool("Bow", false);
                break;
            case Behaviors.Attack:
                // Saldýrý durumunda saldýrý animasyonunu aç, diðer animasyonlarý kapat
                animator.SetBool("Run", false);
                animator.SetBool("Sword", true);
                animator.SetBool("Bow", false);
                break;
            case Behaviors.Bow:
                // Yay durumunda yay animasyonunu aç, diðer animasyonlarý kapat
                animator.SetBool("Run", false);
                animator.SetBool("Sword", false);
                animator.SetBool("Bow", true);
                break;
        }
    }

    //----------------------------FÝZÝK VE MEKANÝK FONKSÝYONLAR-------------------------------------------//
    private void Movement()
    {
        horizontalInput = inputMovement.x;        // Yatay deðere vector2 x bileþenini ata
        verticalInput = inputMovement.y;          // Dikey deðere vector2 y bileþenini ata

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Hareket vektörü ile karakteri ileri, geri, sola veya saða hareket ettir
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);

            // Karakterin yeni dönüþ rotasyonunu hesapla ve bu rotasyona döndür
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Koþma davranýþýný ayarla
            behaviors = Behaviors.Running;
        }
        else
        {
            // Bekleme davranýþýný ayarla
            behaviors = Behaviors.Waiting;
        }
    }

    private void Attack()
    {
        if (isAttacking)
        {
            // Saldýrý davranýþýný ayarla
            behaviors = Behaviors.Attack;
        }
    }

    private void Bow()
    {
        if (isBow)
        {
            // Yay davranýþýný ayarla
            behaviors = Behaviors.Bow;
        }
        
    }

    

    //----------------------------INPUT SYSTEM FONKSÝYONLAR-------------------------------------------//
    private void InputActionsControl()
    {
        gesarInput = new GesarInput(); // GesarInput nesnesini oluþtur

        // Hareket girdilerini kontrol et
        gesarInput.Gameplay.Movement.started += InputMove;
        gesarInput.Gameplay.Movement.performed += InputMove;
        gesarInput.Gameplay.Movement.canceled += InputMove;

        // Saldýrý girdilerini kontrol et
        gesarInput.Gameplay.Attack.started += InputAttack;
        gesarInput.Gameplay.Attack.canceled += InputAttack;

        // Yay girdilerini kontrol et
        gesarInput.Gameplay.Bow.performed += InputBow;
        gesarInput.Gameplay.Bow.canceled += InputBow;
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>(); // Hareket girdilerini al
    }

    private void InputAttack(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton(); // Saldýrý girdilerini al
    }

    private void InputBow(InputAction.CallbackContext context)
    {
        isBow = context.ReadValueAsButton(); // Yay girdilerini al
    }
}
