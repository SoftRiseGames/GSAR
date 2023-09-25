using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;               // Karakter rigidbody
    [SerializeField] private Animator animator;          // Karakter animator
    [SerializeField] private float moveSpeed = 5f;       // Hareket h�z�
    [SerializeField] private float rotationSpeed = 10f;  // D�n�� h�z�

    

    private enum Behaviors {Waiting,Walking,BowUp,BowDown}    // Davran��lar� temsil eden enum 

    [SerializeField] private Behaviors behaviors= Behaviors.Waiting;        // Karakterin mevcut davran���n� temsil eden enum de�i�ken

    private void Update()
    {
        Move();                      // Karakteri hareket ettiren fonksiyon.
        Rotate();                    // Karakterin d�n���n� kontrol eden fonksiyon.
        AnimControl();               // Karakterin animasyon kontrol�n� sa�layan fonksiyon.
        
    }
    
    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");          // Yatay (sol-sa�) giri� de�eri.
        float verticalInput = Input.GetAxis("Vertical");              // Dikey (ileri-geri) giri� de�eri.

        
        // Hareket vekt�r� ile karakteri ileri, geri, sola veya sa�a hareket ettirme.
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed * Time.deltaTime; 
        rb.MovePosition(rb.position + movement);

        // E�er karakter hi�bir yere gitmiyorsa , Waiting durumuna ge�er.
        if (horizontalInput != 0 || verticalInput != 0)
        {
            
            behaviors = Behaviors.Walking;

        }
        else 
        {
            EquipmentControl();
        }
        
        
    }
    private void Rotate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");        // Yatay (sol-sa�) giri� de�eri.
        float verticalInput = Input.GetAxis("Vertical");            // Dikey (ileri-geri) giri� de�eri.

        if (horizontalInput != 0 || verticalInput != 0) 
        {
            // Karakterin yeni d�n�� rotasyonunu hesaplar ve bu rotasyona d�nd�r�r.
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput)); 
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);  
        }
    }
    
    private void EquipmentControl()
    {
        

        if (Input.GetMouseButton(1))
        {
            behaviors = Behaviors.BowUp;
        }
        else if (Input.GetMouseButtonUp(1))
        {

            behaviors = Behaviors.BowDown;
        }


    }
   
    private void AnimControl()
    {
        
        switch (behaviors)    // Karakterin davran���na g�re animasyonlar� kontrol eder.
        {
            case Behaviors.Walking:         
                animator.SetBool("Walking", true);       // Walking durumundayken "Walking" animasyonunu �al��t�r�r.
                animator.SetBool("Waiting", false);
                animator.SetBool("Bow", false);
                break;
            
            case Behaviors.Waiting:
                animator.SetBool("Walking", false);      // Waiting durumundayken "Walking" animasyonunu durdurur.
                animator.SetBool("Waiting", true);
                animator.SetBool("Bow", false);
                break;
            case Behaviors.BowUp:
                animator.SetBool("Bow", true);
                animator.SetBool("Waiting", false);
                break;
            case Behaviors.BowDown:
                animator.SetBool("BowDown", true);
                animator.SetBool("Bow", false);
                break;

        }

    }
   
}
