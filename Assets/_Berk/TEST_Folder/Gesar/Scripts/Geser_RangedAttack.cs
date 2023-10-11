using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Geser_RangedAttack : MonoBehaviour
{
    // RANGE DE���KENLER�
    private GesarInput gesarInputRanged;            // Input Systemin olu�turdu�u classtan bir nesne 
    [SerializeField] private bool isBow;            
    
    // AN�MASYON DE���KENLER�
    private Animator animatorRanged;

    //----------------------------UNITY MONOBEHAVIOR FONKS�YONLARI-------------------------------------------//
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
        gesarInputRanged.Gameplay.Enable(); // Gameplay action map'ini etkinle�tir
    }
    private void OnDisable()
    {
        gesarInputRanged.Gameplay.Disable(); // Gameplay action map'ini devre d��� b�rak
    }
    private void Update()
    {
        MovementAnimations();
        Bow();
    }

    //----------------------------AN�MASYON GEC�KME FONKS�YONLARI-------------------------------------------//
    private void MovementAnimations()
    {
        // Speed parametresine de�eri atayarak blend tree ge�i�ini kontrol et
        animatorRanged.SetBool("Bow", isBow);
    }

    //---------------------------- MEKAN�K FONKS�YONLAR-------------------------------------------//
    private void Bow()
    {

    }

    //----------------------------INPUT SYSTEM FONKS�YONLAR-------------------------------------------//
    private void RangedInputActionsControl()
    {
        gesarInputRanged = new GesarInput(); // GesarInput nesnesini olu�tur

        // Yay girdilerini kontrol et
        gesarInputRanged.Gameplay.Bow.performed += InputBow;
        gesarInputRanged.Gameplay.Bow.canceled += InputBow;
    }
    private void InputBow(InputAction.CallbackContext context)
    {
        isBow = context.ReadValueAsButton(); // Yay girdilerini al
    }

}
