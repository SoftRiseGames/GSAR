using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Geser_Attack : MonoBehaviour
{
    // SALDIRI DEĞİŞKENLERİ
    private GesarInput gesarInputAttack;                        // Input Systemin oluşturduğu classtan bir nesne 
    [SerializeField] private bool isAttacking;                  // Saldırı yapılıp yapılmadığını belirten bool değişken
    private Rigidbody rbAttack;                                 // Karakter rigidbody
    [SerializeField] float attackForce = 10f;                   // Saldırı kuvveti, istediğiniz değere ayarlayabilirsiniz

    
    
    // ANİMASYON DEĞİŞKENLERİ
    private Animator animatorAttack;                            // Karakter için bir Animator bileşeni


    //----------------------------UNITY MONOBEHAVIOR FONKSİYONLARI-------------------------------------------//
    private void Awake()
    {
        AttackInputActionsControl();    // Input System kontrollerini başlat
        
    }
    private void Start()
    {
        animatorAttack = GetComponent<Animator>(); // Animator bileşenini al
        rbAttack = GetComponent<Rigidbody>();   //Rigidbody bileşeni al
    }
    private void OnEnable()
    {
        gesarInputAttack.Gameplay.Enable(); // Gameplay action map'ini etkinleştir
    }
    private void OnDisable()
    {
        gesarInputAttack.Gameplay.Disable(); // Gameplay action map'ini devre dışı bırak
    }
    private void Update()
    {
        AttackAnimations();
        //Attack();
    }
    

    //----------------------------ANİMASYON GECİKME FONKSİYONLARI-------------------------------------------//
    private void AttackAnimations()
    {
        // Speed parametresine değeri atayarak blend tree geçişini kontrol et
        animatorAttack.SetBool("Sword", isAttacking);
    }
    

    //---------------------------- MEKANİK FONKSİYONLAR-------------------------------------------//
    public void Attack()
    {
            // Saldırı durumundayken karakteri z ekseni boyunca öne doğru hareket ettirmek için bir kuvvet uygula
            Vector3 attackDirection = transform.forward; // Karakterin yönüne doğru saldırı yapması için kullanılan vektör

            // Rigidbody'ye kuvvet uygula
            rbAttack.AddForce(attackDirection * attackForce, ForceMode.Impulse);
        
    }

    //----------------------------INPUT SYSTEM FONKSİYONLAR-------------------------------------------//
    private void AttackInputActionsControl()
    {
        gesarInputAttack = new GesarInput(); // GesarInput nesnesini oluştur

        // Saldırı girdilerini kontrol et
        gesarInputAttack.Gameplay.Attack.started += InputAttack;
        gesarInputAttack.Gameplay.Attack.canceled += InputAttack;

    }
    private void InputAttack(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton(); // Saldırı girdilerini al
        
    }
}
