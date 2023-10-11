using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Geser_Attack : MonoBehaviour
{
    // SALDIRI DEÐÝÞKENLERÝ
    private GesarInput gesarInputAttack;                        // Input Systemin oluþturduðu classtan bir nesne 
    [SerializeField] private bool isAttacking;                  // Saldýrý yapýlýp yapýlmadýðýný belirten bool deðiþken
    private Rigidbody rbAttack;
    [SerializeField] float attackForce = 10f;                   // Saldýrý kuvveti, istediðiniz deðere ayarlayabilirsiniz
    
    
    // ANÝMASYON DEÐÝÞKENLERÝ
    private Animator animatorAttack;                            // Karakter için bir Animator bileþeni


    //----------------------------UNITY MONOBEHAVIOR FONKSÝYONLARI-------------------------------------------//
    private void Awake()
    {
        AttackInputActionsControl();    // Input System kontrollerini baþlat
    }
    private void Start()
    {
        animatorAttack = GetComponent<Animator>(); // Animator bileþenini al
        rbAttack = GetComponent<Rigidbody>();   //Rigidbody bileþeni al
    }
    private void OnEnable()
    {
        gesarInputAttack.Gameplay.Enable(); // Gameplay action map'ini etkinleþtir
    }
    private void OnDisable()
    {
        gesarInputAttack.Gameplay.Disable(); // Gameplay action map'ini devre dýþý býrak
    }
    private void Update()
    {
        AttackAnimations();
        Attack();
    }
    

    //----------------------------ANÝMASYON GECÝKME FONKSÝYONLARI-------------------------------------------//
    private void AttackAnimations()
    {
        // Speed parametresine deðeri atayarak blend tree geçiþini kontrol et
        animatorAttack.SetBool("Sword", isAttacking);
    }
    

    //---------------------------- MEKANÝK FONKSÝYONLAR-------------------------------------------//
    public void Attack()
    {
        if (isAttacking == true)
        {
            // Saldýrý durumundayken karakteri z ekseni boyunca öne doðru hareket ettirmek için bir kuvvet uygula
            Vector3 attackDirection = transform.forward; // Karakterin yönüne doðru saldýrý yapmasý için kullanýlan vektör

            // Rigidbody'ye kuvvet uygula
            rbAttack.AddForce(attackDirection * attackForce, ForceMode.Impulse);
        }
    }

    //----------------------------INPUT SYSTEM FONKSÝYONLAR-------------------------------------------//
    private void AttackInputActionsControl()
    {
        gesarInputAttack = new GesarInput(); // GesarInput nesnesini oluþtur

        // Saldýrý girdilerini kontrol et
        gesarInputAttack.Gameplay.Attack.started += InputAttack;
        gesarInputAttack.Gameplay.Attack.canceled += InputAttack;

    }
    private void InputAttack(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton(); // Saldýrý girdilerini al
    }
}
