using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;               // Karakter rigidbody
    [SerializeField] private Animator animator;          // Karakter animator
    [SerializeField] private float moveSpeed = 5f;       // Hareket hýzý
    [SerializeField] private float rotationSpeed = 10f;  // Dönüþ hýzý

    

    private enum Behaviors {Waiting,Walking}    // Davranýþlarý temsil eden enum 

    [SerializeField] private Behaviors behaviors;        // Karakterin mevcut davranýþýný temsil eden enum deðiþken

    private void Update()
    {
        Move();                      // Karakteri hareket ettiren fonksiyon.
        Rotate();                    // Karakterin dönüþünü kontrol eden fonksiyon.
        AnimControl();               // Karakterin animasyon kontrolünü saðlayan fonksiyon.
        
    }
    
    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");          // Yatay (sol-sað) giriþ deðeri.
        float verticalInput = Input.GetAxis("Vertical");              // Dikey (ileri-geri) giriþ deðeri.

        
        // Hareket vektörü ile karakteri ileri, geri, sola veya saða hareket ettirme.
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed * Time.deltaTime; 
        rb.MovePosition(rb.position + movement);

        // Eðer karakter hiçbir yere gitmiyorsa , Waiting durumuna geçer.
        if (horizontalInput == 0 && verticalInput == 0)
        {
            behaviors = Behaviors.Waiting; 
        }
        
        // Karakter herhangi bir yöne hareket ediyorsa, Walking durumuna geçer.
        else
        {
            behaviors = Behaviors.Walking;
        }
    }
    private void Rotate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");        // Yatay (sol-sað) giriþ deðeri.
        float verticalInput = Input.GetAxis("Vertical");            // Dikey (ileri-geri) giriþ deðeri.

        if (horizontalInput != 0 || verticalInput != 0) 
        {
            // Karakterin yeni dönüþ rotasyonunu hesaplar ve bu rotasyona döndürür.
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput)); 
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);  
        }
    }

   
    private void AnimControl()
    {
        
        switch (behaviors)    // Karakterin davranýþýna göre animasyonlarý kontrol eder.
        {
            case Behaviors.Walking:         
                animator.SetBool("Walking", true);       // Walking durumundayken "Walking" animasyonunu çalýþtýrýr.
                break;
            
            case Behaviors.Waiting:
                animator.SetBool("Walking", false);      // Waiting durumundayken "Walking" animasyonunu durdurur.
                break;
        }
    }
   
}
