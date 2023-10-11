using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_RangedAttack : MonoBehaviour
{
    // RANGE DEÐÝÞKENLERÝ
    private GesarInput gesarInputRanged;            // Input Systemin oluþturduðu classtan bir nesne 
    [SerializeField] private bool isBow;            
    
    // ANÝMASYON DEÐÝÞKENLERÝ
    private Animator animatorRanged;

    //----------------------------UNITY MONOBEHAVIOR FONKSÝYONLARI-------------------------------------------//
    private void Start()
    {
        animatorRanged = GetComponent<Animator>();
    }
    private void Awake()
    {
        RangedInputActionsControl();
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
        Bow();
    }

    //----------------------------ANÝMASYON GECÝKME FONKSÝYONLARI-------------------------------------------//
    private void MovementAnimations()
    {
        // Speed parametresine deðeri atayarak blend tree geçiþini kontrol et
        animatorRanged.SetBool("Bow", isBow);
    }

    //---------------------------- MEKANÝK FONKSÝYONLAR-------------------------------------------//
    private void Bow()
    {

    }

    //----------------------------INPUT SYSTEM FONKSÝYONLAR-------------------------------------------//
    private void RangedInputActionsControl()
    {
        gesarInputRanged = new GesarInput(); // GesarInput nesnesini oluþtur

        // Yay girdilerini kontrol et
        gesarInputRanged.Gameplay.Bow.performed += InputBow;
        gesarInputRanged.Gameplay.Bow.canceled += InputBow;
    }
    private void InputBow(InputAction.CallbackContext context)
    {
        isBow = context.ReadValueAsButton(); // Yay girdilerini al
    }

}
