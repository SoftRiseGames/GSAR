using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;



public class Geser_Attack : MonoBehaviour
{
    private GesarInput gesarInputAttack;                        // Input Systemin oluşturduğu classtan bir nesne 
    [SerializeField] private bool isAttacking;                  // Saldırı yapılıp yapılmadığını belirten bool değişken
    private Rigidbody rbAttack;                                 // Karakter rigidbody
    [SerializeField] float attackForce = 10f;                   // Saldırı kuvveti, istediğiniz değere ayarlayabilirsiniz
    
    public GameObject geser;
    private Vector3 _frontVector;
    public float dashLength;

    private Geser_StateSystem stateSystem;
    public Transform attackPoint;
    public float AttackRange;
    public LayerMask enemyLayer;
    // ANİMASYON DEĞİŞKENLERİ
    private Animator animatorAttack;                            // Karakter için bir Animator bileşeni
    Collider2D[] hitenemies;
    public int comboIndex = 0;

    //----------------------------UNITY MONOBEHAVIOR FONKSİYONLARI-------------------------------------------//
    private void Awake()
    {
        AttackInputActionsControl();    // Input System kontrollerini başlat
        stateSystem = GetComponent<Geser_StateSystem>();
    }
    private void Start()
    {
        hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, enemyLayer);
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
        foreach (Collider2D enemy in hitenemies)
        {
            Debug.Log("hit");
        }
        AttackAnimations();
        //Attack();
    }
    

    //----------------------------ANİMASYON GECİKME FONKSİYONLARI-------------------------------------------//
    private void AttackAnimations()
    {
        
        
        animatorAttack.SetInteger("ComboIndex",comboIndex);
        
    }

    public void AttackDash()
    {
        _frontVector = geser.transform.position + (geser.transform.forward * dashLength);
        geser.transform.DOMove(_frontVector, 0.5f);
    }

    public void ComboIndexReset()
    {
        comboIndex=0;
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawSphere(attackPoint.position, AttackRange);
    }

    void AttackSystemColliders()
    {
  
        
       
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
        gesarInputAttack.Gameplay.Attack.canceled += DeInputAttack;

    }
    private void InputAttack(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton(); // Saldırı girdilerini al
        stateSystem.currentState = Geser_StateSystem.AnimState.SwordAttack;
        comboIndex++;
    }
    private void DeInputAttack(InputAction.CallbackContext context)
    {
        stateSystem.currentState = Geser_StateSystem.AnimState.SwordReady;
        
    }
}
