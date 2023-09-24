using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;               // Karakter rigidbody
    [SerializeField] private Animator animator;          // Karakter animator
    [SerializeField] private float moveSpeed = 5f;       // Hareket h�z�
    [SerializeField] private float rotationSpeed = 10f;  // D�n�� h�z�

    

    private enum Behaviors {Waiting,Walking}    // Davran��lar� temsil eden enum 

    [SerializeField] private Behaviors behaviors;        // Karakterin mevcut davran���n� temsil eden enum de�i�ken

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
        if (horizontalInput == 0 && verticalInput == 0)
        {
            behaviors = Behaviors.Waiting; 
        }
        
        // Karakter herhangi bir y�ne hareket ediyorsa, Walking durumuna ge�er.
        else
        {
            behaviors = Behaviors.Walking;
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

   
    private void AnimControl()
    {
        
        switch (behaviors)    // Karakterin davran���na g�re animasyonlar� kontrol eder.
        {
            case Behaviors.Walking:         
                animator.SetBool("Walking", true);       // Walking durumundayken "Walking" animasyonunu �al��t�r�r.
                break;
            
            case Behaviors.Waiting:
                animator.SetBool("Walking", false);      // Waiting durumundayken "Walking" animasyonunu durdurur.
                break;
        }
    }
   
}
