using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_RangedAttack : MonoBehaviour
{
    // RANGE DE���KENLER�
    private GesarInput gesarInputRanged;            // Input Systemin olu�turdu�u classtan bir nesne 
    [SerializeField] private bool isBow;            
    
    Geser_StateSystem stateSystem;

    // AN�MASYON DE���KENLER�
    private Animator animatorRanged;

    private Geser_Movement geserMovement; // Geser_Movement scriptine eri�im i�in referans

    //----------------------------UNITY MONOBEHAVIOR FONKS�YONLARI-------------------------------------------//
    private void Start()
    {
        geserMovement = GetComponent<Geser_Movement>(); // Geser_Movement scriptine eri�im i�in referans� al
        animatorRanged = GetComponent<Animator>();
    }
    private void Awake()
    {
        RangedInputActionsControl();
        stateSystem = GetComponent<Geser_StateSystem>();
    }
    private void OnEnable()
    {
        gesarInputRanged.Gameplay.Enable(); // Gameplay action map'ini etkinle�tir
    }
    private void OnDisable()
    {
        gesarInputRanged.Gameplay.Disable(); // Gameplay action map'ini devre d��� b�rak
    }
    private void Update()
    {
        MovementAnimations();
        
    }

    //----------------------------AN�MASYON GEC�KME FONKS�YONLARI-------------------------------------------//
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

    //---------------------------- MEKAN�K FONKS�YONLAR-------------------------------------------//
    

    //----------------------------INPUT SYSTEM FONKS�YONLAR-------------------------------------------//
    private void RangedInputActionsControl()
    {
        gesarInputRanged = new GesarInput(); // GesarInput nesnesini olu�tur

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
    }


}
