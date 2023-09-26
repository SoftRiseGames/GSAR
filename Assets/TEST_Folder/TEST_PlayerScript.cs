using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;               // Karakter rigidbody
    [SerializeField] private Animator animator;          // Karakter animator
    [SerializeField] private float moveSpeed = 5f;       // Hareket hýzý
    [SerializeField] private float rotationSpeed = 10f;  // Dönüþ hýzý

    

    private enum Behaviors {Waiting,Walking,BowUp,BowDown,Roll,Attack}    // Davranýþlarý temsil eden enum 

    [SerializeField] private Behaviors behaviors= Behaviors.Waiting;        // Karakterin mevcut davranýþýný temsil eden enum deðiþken

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

        // Eðer hareket var ise , karakter Walking durumuna geçer.
        if (horizontalInput != 0 || verticalInput != 0)          
        {
            
            behaviors = Behaviors.Walking;      

        }
        // Eðer hareket yok ise ne durumda olduðunu kontrol etmek için fonksiyona bakar.
        else
        {
            BowControl();
            SwordControl();
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            behaviors = Behaviors.Roll;
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
    
    private void BowControl()
    {
        

        if (Input.GetMouseButton(1))            //Eðer mouse sað týk basýlý ise BowUp duruma geçer.
        {
            behaviors = Behaviors.BowUp;
        }
        else if (Input.GetMouseButtonUp(1))     //Mouse basýlý tutulmayý býraktýðýnda BowDown duruma geçer.
        {

            behaviors = Behaviors.BowDown;
        }
        else                                    //Hiçbir hareket yok ise Waiting duruma geçer.
        {
            behaviors = Behaviors.Waiting;
        }


    }
    
    private void SwordControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            behaviors = Behaviors.Attack;

        }
        
    }
    private void AttackComboSystem()
    {
        if (Input.GetMouseButtonDown(0) && behaviors==Behaviors.Attack && animator.GetInteger("Attack")==0)
        {
            animator.SetInteger("Attack", 1);
        }
        else if (Input.GetMouseButtonDown(0) && behaviors == Behaviors.Attack && animator.GetInteger("Attack") == 1)
        {
            animator.SetInteger("Attack", 2);
        }
        else if (Input.GetMouseButtonDown(0) && behaviors == Behaviors.Attack && animator.GetInteger("Attack") == 2)
        {
            animator.SetInteger("Attack", 3);
        }
       
        

    }
    private void AttackFinished()
    {
        animator.SetInteger("Attack", 0);
    }
    private void AnimControl()
    {
        
        switch (behaviors)    // Karakterin davranýþýna göre animasyonlarý kontrol eder.
        {
            case Behaviors.Walking:         
                animator.SetBool("Walking", true);       // Walking durumundayken "Walking" animasyonunu çalýþtýrýr.
                animator.SetBool("Waiting", false);      
                animator.SetBool("BowUp", false);
                animator.SetBool("BowDown", false);
                break;
            
            case Behaviors.Waiting:
                animator.SetBool("Walking", false);      // Waiting durumundayken "Walking" animasyonunu durdurur.
                animator.SetBool("Waiting", true);
                animator.SetBool("BowUp", false);
                animator.SetBool("Roll", false);
                animator.SetBool("BowDown", false);
                break;
            case Behaviors.BowUp:
                animator.SetBool("BowUp", true);           // BowUp durumundayken "Bow" animasyonunu çalýþtýrýr.
                animator.SetBool("Waiting", false);
                animator.SetInteger("Attack", 0);
                break;
            case Behaviors.BowDown:
                animator.SetBool("BowDown", true);       // BowDown durumundayken "BowDown" animasyonunu çalýþtýrýr.
                animator.SetBool("BowUp", false);
                break;
            case Behaviors.Roll:
                animator.SetBool("Roll", true);
                animator.SetBool("Waiting", false);
                break;
            case Behaviors.Attack:
                AttackComboSystem();
                animator.SetBool("BowUp", false);
                break;
        }

    }
   
}
