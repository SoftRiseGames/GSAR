using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_RangedAttack : MonoBehaviour
{
    // RANGE DEĞİŞKENLERİ
    private GesarInput gesarInputRanged;            // Input Systemin oluşturduğu classtan bir nesne 
    [SerializeField] private bool isBow;
    [SerializeField] private GameObject arrow, arrowPoint;
    public float arrowSpeed;
    
    Geser_StateSystem stateSystem;

    // ANİMASYON DEĞİŞKENLERİ
    private Animator animatorRanged;

    private Geser_Movement geserMovement; // Geser_Movement scriptine erişim için referans

    //----------------------------UNITY MONOBEHAVIOR FONKSİYONLARI-------------------------------------------//
    private void Start()
    {
        geserMovement = GetComponent<Geser_Movement>(); // Geser_Movement scriptine erişim için referansı al
        animatorRanged = GetComponent<Animator>();
    }
    private void Awake()
    {
        RangedInputActionsControl();
        stateSystem = GetComponent<Geser_StateSystem>();
    }
    private void OnEnable()
    {
        gesarInputRanged.Gameplay.Enable(); // Gameplay action map'ini etkinleştir
    }
    private void OnDisable()
    {
        gesarInputRanged.Gameplay.Disable(); // Gameplay action map'ini devre dışı bırak
    }
    private void Update()
    {
        MovementAnimations();
        
        
    }

    //----------------------------ANİMASYON GECİKME FONKSİYONLARI-------------------------------------------//
    private void MovementAnimations()
    {
        if(stateSystem.currentState== Geser_StateSystem.AnimState.BowReady)
        {
            animatorRanged.SetBool("Bow", isBow);
        }
        else if (stateSystem.currentState == Geser_StateSystem.AnimState.SwordReady)
        {
            animatorRanged.SetBool("Bow", false);
        }
        
    }

    //---------------------------- MEKANİK FONKSİYONLAR-------------------------------------------//
    private void ArrowSpawn()
    {
        GameObject cloneArrow = Instantiate(arrow, arrowPoint.transform.position, arrowPoint.transform.rotation);
        Rigidbody rbArrow = cloneArrow.GetComponent<Rigidbody>();
        rbArrow.AddForce(arrowPoint.transform.forward*arrowSpeed,ForceMode.Impulse);
    }

    
    
    //----------------------------INPUT SYSTEM FONKSİYONLAR-------------------------------------------//
    private void RangedInputActionsControl()
    {
        gesarInputRanged = new GesarInput(); // GesarInput nesnesini oluştur

        // Yay girdilerini kontrol et
        gesarInputRanged.Gameplay.Bow.performed += InputBow;
        gesarInputRanged.Gameplay.Bow.canceled += DeInputBow;
    }
    private void InputBow(InputAction.CallbackContext context)
    {
        isBow = context.ReadValueAsButton(); // Yay girdilerini al
        stateSystem.currentState = Geser_StateSystem.AnimState.BowReady;
    }
    private void DeInputBow(InputAction.CallbackContext context)
    {
        stateSystem.currentState = Geser_StateSystem.AnimState.SwordReady;
        ArrowSpawn();
    }


}
