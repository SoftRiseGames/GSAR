using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_RangedAttack : MonoBehaviour
{
    // RANGE DEÐÝÞKENLERÝ
    private GesarInput gesarInputRanged;            // Input Systemin oluþturduðu classtan bir nesne 
    [SerializeField] private bool isBow;            
    
    Geser_StateSystem stateSystem;

    // ANÝMASYON DEÐÝÞKENLERÝ
    private Animator animatorRanged;

    private Geser_Movement geserMovement; // Geser_Movement scriptine eriþim için referans

    //----------------------------UNITY MONOBEHAVIOR FONKSÝYONLARI-------------------------------------------//
    private void Start()
    {
        geserMovement = GetComponent<Geser_Movement>(); // Geser_Movement scriptine eriþim için referansý al
        animatorRanged = GetComponent<Animator>();
    }
    private void Awake()
    {
        RangedInputActionsControl();
        stateSystem = GetComponent<Geser_StateSystem>();
    }
    private void OnEnable()
    {
        gesarInputRanged.Gameplay.Enable(); // Gameplay action map'ini etkinleþtir
    }
    private void OnDisable()
    {
        gesarInputRanged.Gameplay.Disable(); // Gameplay action map'ini devre dýþý býrak
    }
    private void Update()
    {
        MovementAnimations();
        
    }

    //----------------------------ANÝMASYON GECÝKME FONKSÝYONLARI-------------------------------------------//
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

    //---------------------------- MEKANÝK FONKSÝYONLAR-------------------------------------------//
    

    //----------------------------INPUT SYSTEM FONKSÝYONLAR-------------------------------------------//
    private void RangedInputActionsControl()
    {
        gesarInputRanged = new GesarInput(); // GesarInput nesnesini oluþtur

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
