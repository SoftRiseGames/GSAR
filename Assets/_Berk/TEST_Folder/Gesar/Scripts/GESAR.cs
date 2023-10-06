using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GESAR : MonoBehaviour
{
    // HAREKET DE���KENLER�
    private GesarInput gesarInput;                              // Input Systemin olu�turdu�u classtan bir nesne 
    private Vector2 inputMovement;                              // InputSystemden al�nacak vector2 de�i�keni atayaca��m�z de�i�ken
    private float horizontalInput, verticalInput;               // vector2 de�i�kenin x ve y de�erlerini atayaca��m�z Yatay ve dikey girdileri
    [SerializeField] private float moveSpeed, rotationSpeed;    // Hareket h�z� ve d�n�� h�z�
    private Rigidbody rb;                                       // Karakterin Rigidbody bile�eni

    // SALDIRI DE���KENLER�
    private bool isAttacking;                                   // Sald�r� yap�l�p yap�lmad���n� belirten bool de�i�ken
    private bool isBow;                                         // Yay kullan�l�p kullan�lmad���n� belirten bool de�i�ken
    
    

   // AN�MASYON DE���KENLER�
   private Animator animator;                                  // Karakter i�in bir Animator bile�eni
    private enum Behaviors { Waiting, Running, Attack, Bow }    // Karakter davran��lar�n� i�in enum

    [SerializeField] private Behaviors behaviors = Behaviors.Waiting; // Ba�lang��ta karakterin bekleme durumunda olaca�� davran��

    
    //----------------------------UNITY MONOBEHAVIOR FONKS�YONLARI-------------------------------------------//
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody bile�enini al
        animator = GetComponent<Animator>(); // Animator bile�enini al
    }

    private void Awake()
    {
        InputActionsControl(); // Input System kontrollerini ba�lat
    }

    private void Update()
    {
        if (behaviors != Behaviors.Attack || behaviors != Behaviors.Bow)
        {
            Movement(); // Hareket fonksiyonunu �a��r
        }
        Attack(); // Sald�r� fonksiyonunu �a��r
        Bow(); // Yay fonksiyonunu �a��r
        AnimControl(); // Animasyon kontrol fonksiyonunu �a��r
    }

    private void OnEnable()
    {
        gesarInput.Gameplay.Enable(); // Gameplay action map'ini etkinle�tir
    }

    private void OnDisable()
    {
        gesarInput.Gameplay.Disable(); // Gameplay action map'ini devre d��� b�rak
    }

    //----------------------------AN�MASYON GEC�KME FONKS�YONLARI-------------------------------------------//
    private void AnimControl()
    {
        // Karakter davran���na g�re animasyonlar� kontrol et
        switch (behaviors)
        {
            case Behaviors.Waiting:
                // Bekleme durumunda animasyonlar� kapat
                animator.SetBool("Run", false);
                animator.SetBool("Sword", false);
                animator.SetBool("Bow", false);
                break;
            case Behaviors.Running:
                // Ko�ma durumunda ko�ma animasyonunu a�, di�er animasyonlar� kapat
                animator.SetBool("Run", true);
                animator.SetBool("Sword", false);
                animator.SetBool("Bow", false);
                break;
            case Behaviors.Attack:
                // Sald�r� durumunda sald�r� animasyonunu a�, di�er animasyonlar� kapat
                animator.SetBool("Run", false);
                animator.SetBool("Sword", true);
                animator.SetBool("Bow", false);
                break;
            case Behaviors.Bow:
                // Yay durumunda yay animasyonunu a�, di�er animasyonlar� kapat
                animator.SetBool("Run", false);
                animator.SetBool("Sword", false);
                animator.SetBool("Bow", true);
                break;
        }
    }

    //----------------------------F�Z�K VE MEKAN�K FONKS�YONLAR-------------------------------------------//
    private void Movement()
    {
        horizontalInput = inputMovement.x;        // Yatay de�ere vector2 x bile�enini ata
        verticalInput = inputMovement.y;          // Dikey de�ere vector2 y bile�enini ata

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Hareket vekt�r� ile karakteri ileri, geri, sola veya sa�a hareket ettir
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);

            // Karakterin yeni d�n�� rotasyonunu hesapla ve bu rotasyona d�nd�r
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Ko�ma davran���n� ayarla
            behaviors = Behaviors.Running;
        }
        else
        {
            // Bekleme davran���n� ayarla
            behaviors = Behaviors.Waiting;
        }
    }

    private void Attack()
    {
        if (isAttacking)
        {
            // Sald�r� davran���n� ayarla
            behaviors = Behaviors.Attack;
        }
    }

    private void Bow()
    {
        if (isBow)
        {
            // Yay davran���n� ayarla
            behaviors = Behaviors.Bow;
        }
        
    }

    

    //----------------------------INPUT SYSTEM FONKS�YONLAR-------------------------------------------//
    private void InputActionsControl()
    {
        gesarInput = new GesarInput(); // GesarInput nesnesini olu�tur

        // Hareket girdilerini kontrol et
        gesarInput.Gameplay.Movement.started += InputMove;
        gesarInput.Gameplay.Movement.performed += InputMove;
        gesarInput.Gameplay.Movement.canceled += InputMove;

        // Sald�r� girdilerini kontrol et
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
        isAttacking = context.ReadValueAsButton(); // Sald�r� girdilerini al
    }

    private void InputBow(InputAction.CallbackContext context)
    {
        isBow = context.ReadValueAsButton(); // Yay girdilerini al
    }
}
