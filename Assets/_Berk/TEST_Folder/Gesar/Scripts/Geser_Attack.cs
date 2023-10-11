using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Geser_Attack : MonoBehaviour
{
    // SALDIRI DE���KENLER�
    private GesarInput gesarInputAttack;                        // Input Systemin olu�turdu�u classtan bir nesne 
    [SerializeField] private bool isAttacking;                  // Sald�r� yap�l�p yap�lmad���n� belirten bool de�i�ken

    // AN�MASYON DE���KENLER�
    private Animator animatorAttack;                            // Karakter i�in bir Animator bile�eni


    //----------------------------UNITY MONOBEHAVIOR FONKS�YONLARI-------------------------------------------//
    private void Awake()
    {
        AttackInputActionsControl();    // Input System kontrollerini ba�lat
    }
    private void Start()
    {
        animatorAttack = GetComponent<Animator>(); // Animator bile�enini al
    }
    private void OnEnable()
    {
        gesarInputAttack.Gameplay.Enable(); // Gameplay action map'ini etkinle�tir
    }
    private void OnDisable()
    {
        gesarInputAttack.Gameplay.Disable(); // Gameplay action map'ini devre d��� b�rak
    }
    private void Update()
    {
        AttackAnimations();
        Attack();
    }

    //----------------------------AN�MASYON GEC�KME FONKS�YONLARI-------------------------------------------//
    private void AttackAnimations()
    {
        // Speed parametresine de�eri atayarak blend tree ge�i�ini kontrol et
        animatorAttack.SetBool("Sword", isAttacking);
    }

    //---------------------------- MEKAN�K FONKS�YONLAR-------------------------------------------//
    private void Attack()
    {

    }

    //----------------------------INPUT SYSTEM FONKS�YONLAR-------------------------------------------//
    private void AttackInputActionsControl()
    {
        gesarInputAttack = new GesarInput(); // GesarInput nesnesini olu�tur

        // Sald�r� girdilerini kontrol et
        gesarInputAttack.Gameplay.Attack.started += InputAttack;
        gesarInputAttack.Gameplay.Attack.canceled += InputAttack;

    }
    private void InputAttack(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton(); // Sald�r� girdilerini al
    }
}